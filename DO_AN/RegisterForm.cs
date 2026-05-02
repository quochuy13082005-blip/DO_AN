using DO_AN;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DO_AN
{
    public partial class RegisterForm : Form
    {
        string excelPath = Path.Combine(Application.StartupPath, @"Data\data.xlsx");
        public RegisterForm()
        {

            InitializeComponent(); 
            btnRegister.Click += BtnRegister_Click;
            btnCancel.Click += (s, e) => this.Close();
        }

        private void BtnRegister_Click(object sender, EventArgs e) 
        {
            // 1. Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                cbGender.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc (*)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 2. Kiểm tra trùng ID
            if (Program.Tree.Search(txtID.Text.Trim()) != null)
            {
                MessageBox.Show("Số CCCD này đã tồn tại trên hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Kiểm tra định dạng địa chỉ và trích xuất tỉnh
            string citizenID = txtID.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (!IsAddressValid(address, out string inputProvince))
            {
                MessageBox.Show("Địa chỉ phải đúng định dạng: Số nhà, Tên đường, Phường, Tỉnh/Thành", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Lấy mã tỉnh từ CCCD (3 số đầu)
            string provinceCode = citizenID.Length >= 3 ? citizenID.Substring(0, 3) : "";

            // Kiểm tra logic khớp mã tỉnh từ DataLoader.provinceMap
            if (DataLoader.provinceMap.TryGetValue(provinceCode, out string expectedProvince))
            {
                // So sánh tỉnh từ địa chỉ (inputProvince) với tỉnh thực tế (expectedProvince)
                if (!inputProvince.ToLower().Contains(expectedProvince.ToLower()))
                {
                    MessageBox.Show($"Mã CCCD '{provinceCode}' thuộc về tỉnh/TP: {expectedProvince}.\n" +
                                    $"Địa chỉ bạn nhập là '{inputProvince}' không khớp!",
                                    "Lỗi logic địa phương", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Mã tỉnh trên CCCD không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 3. Tạo đối tượng Citizen mới 
            Citizen newCitizen = new Citizen
            {
                CitizenID = txtID.Text.Trim(),
                FullName = txtName.Text.Trim(),
                DateOfBirth = dtpDOB.Value,
                Gender = cbGender.SelectedItem.ToString(),
                Address = txtAddress.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                FatherID = "null",
                MotherID = "null",
                Occupation = "Tự do"
            };
            // 4. Tạo mật khẩu tự động
            try
            {
                string[] nameParts = newCitizen.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string lastName = nameParts.Length > 0 ? nameParts.Last() : "User";
                string idSuffix = newCitizen.CitizenID.Length >= 3 ?
                                 newCitizen.CitizenID.Substring(newCitizen.CitizenID.Length - 3) : "123";
                newCitizen.Password = lastName + "@" + idSuffix;

                // 5. Chèn vào cây BST
                Program.Tree.Insert(newCitizen);
                DataLoader.SaveToExcel(Program.Tree, excelPath);

                MessageBox.Show($"Đăng ký thành công!\nMật khẩu của bạn là: {newCitizen.Password}",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsAddressValid(string address, out string detectedProvince) 
        {
            detectedProvince = "";
            if (string.IsNullOrWhiteSpace(address)) return false;

            // Giả định định dạng: "Số 1, Đường ABC, Phường XYZ, Hà Nội"
            string[] parts = address.Split(',');

            if (parts.Length < 4) return false;

            detectedProvince = parts[parts.Length - 1].Trim();
            return true;
        }
    }
}
