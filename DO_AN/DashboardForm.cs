using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace DO_AN
{
    public partial class DashboardForm : Form
    {
        // =========================================================
        //1. Cấu trúc Chung và Khởi tạo Form        
        // =========================================================
        // ===== BIẾN THÀNH VIÊN =====
        private Citizen currentUser; 
        private DataGridView currentGrid;
        string excelPath = Path.Combine(Application.StartupPath, @"Data\data.xlsx");
        // Các TextBox để nhập liệu công dân
        private TextBox txtCitizenID, txtCitizenName, txtCitizenPass, txtCitizenAddress; 
        private TextBox txtCitizenFatherID, txtCitizenMotherID, txtCitizenOccupation, txtCitizenPhone, txtCitizenSpouseID, txtCitizenNationality;
        private DateTimePicker dtpCitizenDOB;
        private ComboBox cbCitizenGender;

        // ===== HÀM KHỞI TẠO ===== 
        public DashboardForm(Citizen c)
        {
            InitializeComponent(); 
            this.Icon = Properties.Resources.logo; 
            currentUser = c; 
            InitCustomGrid();
        }

        // ===== SỰ KIỆN FORM LOAD =====
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            if (currentUser.CitizenID.StartsWith("admin"))
            {
                ShowAdminUI();
            }
            else
                ShowUserUI();
        }

        // ===== CÁC HÀM TIỆN ÍCH CHUNG =====
        private void Logout(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void Dummy(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng đang phát triển", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =========================================================
        // 2. QUẢN TRỊ VIÊN 
        // =========================================================
        private void ShowAdminUI()
        {
            // =====Thiết lập giao diện chính======
            this.panelContent.BackgroundImage = Properties.Resources.background;
            this.panelContent.BackgroundImageLayout = ImageLayout.Stretch;
            this.panelContent.Padding = new Padding(30);
            this.panelContent.Controls.Clear();

            // --- Cấu hình menu bên trái ----
            this.panelMenu.BackColor = Color.FromArgb(26, 37, 47);
            this.panelContent.Padding = new Padding(30);
            this.panelContent.Controls.Clear();

            // 1. Header với logo và tên hệ thống
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.FromArgb(26, 37, 47), };
            panelMenu.Controls.Add(pnlHeader);

            PictureBox picLogo = new PictureBox
            {
                Image = Properties.Resources.logo_rm,
                SizeMode = PictureBoxSizeMode.Zoom,
                Height = 100,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 20, 0, 0)
            };

            Label lblSystem = new Label
            {
                Text = "HỆ THỐNG\nĐỊNH DANH ĐIỆN TỬ",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            Label lblUser = new Label
            {
                Text = "QUẢN TRỊ VIÊN",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };

            pnlHeader.Controls.Add(lblUser);
            pnlHeader.Controls.Add(lblSystem);
            pnlHeader.Controls.Add(picLogo);

            //2. Nút thoát
            UIHelper.AddMenuButton("Thoát", Logout, panelMenu, true);

            // 3. Các nút chức năng chính
            Panel pnlButtonContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(0, 60, 0, 0) };
            panelMenu.Controls.Add(pnlButtonContainer);
            pnlButtonContainer.BringToFront();

            UIHelper.AddMenuButton("🔄 Làm mới", (s, e) => { btnRefresh(); }, pnlButtonContainer, false);
            UIHelper.AddMenuButton("➕ Thêm mới", AddCitizen, pnlButtonContainer, false);
            UIHelper.AddMenuButton("📝 Chỉnh sửa", EditCitizen, pnlButtonContainer, false);
            UIHelper.AddMenuButton("🗑️ Xóa bỏ", DeleteCitizen, pnlButtonContainer, false);
            UIHelper.AddMenuButton("👨‍👩‍👧 Phả hệ", ViewFamily, pnlButtonContainer, false);

            // --- Cấu hình khu vực nội dung chính ---
            // 1. Khu vực nhập liệu 
            GroupBox gbInput = new GroupBox
            {
                Text = "📝 THÔNG TIN CÔNG DÂN",
                Dock = DockStyle.Top,
                Height = 200,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };

            // Các TextBox và DateTimePicker cho nhập liệu
            txtCitizenID = new TextBox() { Location = new Point(120, 35), Width = 150 };
            txtCitizenPass = new TextBox() { Location = new Point(120, 75), Width = 150 };
            txtCitizenFatherID = new TextBox() { Location = new Point(120, 115), Width = 150 };
            dtpCitizenDOB = new DateTimePicker() { Location = new Point(120, 155), Width = 150, Format = DateTimePickerFormat.Short };

            txtCitizenName = new TextBox() { Location = new Point(400, 35), Width = 180 };
            txtCitizenAddress = new TextBox() { Location = new Point(400, 75), Width = 180 };
            txtCitizenMotherID = new TextBox() { Location = new Point(400, 115), Width = 180 };
            txtCitizenSpouseID = new TextBox() { Location = new Point(400, 155), Width = 180 };

            cbCitizenGender = new ComboBox() { Location = new Point(720, 35), Width = 150 };
            cbCitizenGender.Items.AddRange(new string[] { "Nam", "Nữ" });
            txtCitizenPhone = new TextBox() { Location = new Point(720, 75), Width = 150 };
            txtCitizenOccupation = new TextBox() { Location = new Point(720, 115), Width = 150 };
            txtCitizenNationality = new TextBox() { Location = new Point(720, 155), Width = 150 };

            // Thêm nhãn cho từng TextBox
            gbInput.Controls.Add(new Label() { Text = "Số CCCD:", Location = new Point(25, 38), AutoSize = true });
            gbInput.Controls.Add(txtCitizenID);
            gbInput.Controls.Add(new Label() { Text = "Họ và Tên:", Location = new Point(310, 38), AutoSize = true });
            gbInput.Controls.Add(txtCitizenName);
            gbInput.Controls.Add(new Label() { Text = "Giới tính:", Location = new Point(620, 38), AutoSize = true });
            gbInput.Controls.Add(cbCitizenGender);

            gbInput.Controls.Add(new Label() { Text = "Mật khẩu:", Location = new Point(25, 78), AutoSize = true });
            gbInput.Controls.Add(txtCitizenPass);
            gbInput.Controls.Add(new Label() { Text = "Quê quán:", Location = new Point(310, 78), AutoSize = true });
            gbInput.Controls.Add(txtCitizenAddress);
            gbInput.Controls.Add(new Label() { Text = "SĐT:", Location = new Point(620, 78), AutoSize = true });
            gbInput.Controls.Add(txtCitizenPhone);

            gbInput.Controls.Add(new Label() { Text = "ID Cha:", Location = new Point(25, 118), AutoSize = true });
            gbInput.Controls.Add(txtCitizenFatherID);
            gbInput.Controls.Add(new Label() { Text = "ID Mẹ:", Location = new Point(310, 118), AutoSize = true });
            gbInput.Controls.Add(txtCitizenMotherID);
            gbInput.Controls.Add(new Label() { Text = "Nghề nghiệp:", Location = new Point(595, 118), AutoSize = true });
            gbInput.Controls.Add(txtCitizenOccupation);

            gbInput.Controls.Add(new Label() { Text = "Ngày sinh:", Location = new Point(25, 158), AutoSize = true });
            gbInput.Controls.Add(dtpCitizenDOB);
            gbInput.Controls.Add(new Label() { Text = "ID Vợ/Chồng:", Location = new Point(290, 158), AutoSize = true });
            gbInput.Controls.Add(txtCitizenSpouseID);
            gbInput.Controls.Add(new Label() { Text = "Quốc tịch:", Location = new Point(610, 158), AutoSize = true });
            gbInput.Controls.Add(txtCitizenNationality);

            //2. Khu vực tìm kiếm 
            Panel searchPanel = new Panel() { Dock = DockStyle.Top, Height = 50, Padding = new Padding(10, 5, 10, 5) };

            TextBox txtSearch = new TextBox() { Location = new Point(120, 15), Width = 300, Font = new Font("Segoe UI", 10) };

            Button btnSearch = new Button() { Text = "🔍 Tìm kiếm", Location = new Point(430, 12), Size = new Size(100, 30),
                                            BackColor = Color.FromArgb(52, 73, 94), ForeColor = Color.White, FlatStyle = FlatStyle.Flat};

            searchPanel.Controls.Add(new Label() { Text = "Tra cứu nhanh:", Location = new Point(25, 18), Font = new Font("Segoe UI", 9, FontStyle.Italic) });

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            btnSearch.Click += (s, e) => { SearchCitizen(txtSearch.Text); };

            panelContent.Controls.Add(currentGrid);
            panelContent.Controls.Add(searchPanel);
            panelContent.Controls.Add(gbInput);

            RefreshGrid();
        }
        private void InitCustomGrid()
        {
            if (currentGrid != null) return; 

            currentGrid = new DataGridView
            { 
                Dock = DockStyle.Fill, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,  AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false,
                ReadOnly = true, RowHeadersVisible = false
            };
            currentGrid.RowTemplate.Height = 35;

            DataGridViewCellStyle headerStyle = currentGrid.ColumnHeadersDefaultCellStyle;
            currentGrid.EnableHeadersVisualStyles = false;
            headerStyle.BackColor = Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            currentGrid.CellClick += CurrentGrid_CellClick;

        }
        private void CurrentGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = currentGrid.Rows[e.RowIndex];

                txtCitizenID.Text = GetVal(row, "CitizenID"); txtCitizenName.Text = GetVal(row, "FullName");
                txtCitizenAddress.Text = GetVal(row, "Address"); txtCitizenPass.Text = GetVal(row, "Password");
                txtCitizenOccupation.Text = GetVal(row, "Occupation"); txtCitizenPhone.Text = GetVal(row, "PhoneNumber");
                txtCitizenFatherID.Text = GetVal(row, "FatherID"); txtCitizenMotherID.Text = GetVal(row, "MotherID");
                txtCitizenSpouseID.Text = GetVal(row, "SpouseID"); txtCitizenNationality.Text = GetVal(row, "Nationality");
                // Xử lý ngày sinh 
                object dobValue = row.Cells["DateOfBirth"].Value;
                if (dobValue is DateTime)
                {
                    dtpCitizenDOB.Value = (DateTime)dobValue;
                }
                // Xử lý Giới tính 
                if (cbCitizenGender != null)
                {
                    string genderInGrid = GetVal(row, "Gender").Trim();
                    int index = cbCitizenGender.FindStringExact(genderInGrid);
                    cbCitizenGender.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị chi tiết: " + ex.Message);
            }
        }
        private string GetVal(DataGridViewRow row, string colName)
        {
            if (row.Cells[colName]?.Value != null)
            {
                return row.Cells[colName].Value.ToString();
            }
            return "";
        }
        private void RefreshGrid()
        {
            if (currentGrid == null) return;

            List<Citizen> dataSource = GetAll(Program.Tree.Root);

            currentGrid.DataSource = null;
            currentGrid.AutoGenerateColumns = true;
            currentGrid.DataSource = dataSource;

            // Kiểm tra xem bảng đã tự tạo cột xong chưa
            if (currentGrid.Columns.Count > 0)
            {
                // Thiết lập tiêu đề 
                SetColumnHeader("CitizenID", "Số CCCD");
                SetColumnHeader("FullName", "Họ và Tên");
                SetColumnHeader("DateOfBirth", "Ngày sinh");
                SetColumnHeader("Gender", "Giới tính");
                SetColumnHeader("PhoneNumber", "SĐT");
                SetColumnHeader("Address", "Quê quán");
                SetColumnHeader("Nationality", "Quốc tịch");
                SetColumnHeader("Occupation", "Nghề nghiệp");
                SetColumnHeader("FatherID", "ID Cha");
                SetColumnHeader("MotherID", "ID Mẹ");
                SetColumnHeader("SpouseID", "ID Vợ/Chồng");
                SetColumnHeader("Password", "Mật khẩu");

                // Định dạng ngày tháng
                if (currentGrid.Columns.Contains("DateOfBirth"))
                {
                    currentGrid.Columns["DateOfBirth"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }
        // Hàm phụ để kiểm tra cột trước khi gán, tránh lỗi crash
        private void SetColumnHeader(string columnName, string headerText)
        {
            if (currentGrid.Columns.Contains(columnName))
            {
                currentGrid.Columns[columnName].HeaderText = headerText;
            }
        }
        // LOGIC QUẢN TRỊ VIÊN: THÊM/SỬA/XÓA/TÌM KIẾM CÔNG DÂN
        //1. Làm mới và Hiển thị Grid
        private void btnRefresh()
        {
            try
            {
                RefreshGrid();

                ClearTextBoxes();

                if (currentGrid.Rows.Count > 0)
                {
                    currentGrid.ClearSelection();
                }

                MessageBox.Show("Dữ liệu đã được làm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới: " + ex.Message);
            }
        }
        //2. Thêm công dân mới
        private void AddCitizen(object sender, EventArgs e)
        {
            // -----Kiểm tra tính hợp lệ của dữ liệu nhập vào-----
            if (!IsInputValid()) return;
            
            Citizen existing = Program.Tree.Search(txtCitizenID.Text);
            if (existing != null)
            {
                MessageBox.Show("Số Citizen này đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra định dạng địa chỉ và trích xuất tỉnh
            string citizenID = txtCitizenID.Text.Trim();
            string address = txtCitizenAddress.Text.Trim();

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
            //Kiểm tra tồn tại của IDcha 
            string fatherID = txtCitizenFatherID.Text.Trim();
            if (!string.IsNullOrEmpty(fatherID) && fatherID.ToLower() != "null" && fatherID.ToLower() != "n/a")
            {
                if (Program.Tree.Search(fatherID) == null)
                {
                    MessageBox.Show("ID Cha không tồn tại! Để trống nếu không rõ.", "Lỗi quan hệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else { fatherID = "N/A"; }
            // Kiểm tra ID Mẹ 
            string motherID = txtCitizenMotherID.Text.Trim();
            if (!string.IsNullOrEmpty(motherID) && motherID.ToLower() != "null" && motherID.ToLower() != "n/a")
            {
                if (Program.Tree.Search(motherID) == null)
                {
                    MessageBox.Show("ID Mẹ không tồn tại! Để trống nếu không rõ.", "Lỗi quan hệ");
                    return;
                }
            }
            else { motherID = "N/A"; }
            // Kiểm tra ID Vợ/Chồng
            string spouseID = txtCitizenSpouseID.Text.Trim();
            if (!string.IsNullOrEmpty(motherID) && motherID.ToLower() != "null" && motherID.ToLower() != "n/a")
            {
                if (Program.Tree.Search(motherID) == null)
                {
                    MessageBox.Show("ID Vợ/Chồng không tồn tại! Để trống nếu không rõ.", "Lỗi quan hệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else { motherID = "N/A"; }

            Citizen c = new Citizen
            {
                CitizenID = txtCitizenID.Text,
                FullName = txtCitizenName.Text,
                Password = txtCitizenPass.Text == "" ? "123@abc" : txtCitizenPass.Text,
                Gender = cbCitizenGender.SelectedItem.ToString(),
                DateOfBirth = dtpCitizenDOB.Value,
                Address = txtCitizenAddress.Text,
                Nationality = txtCitizenNationality.Text,
                Occupation = txtCitizenOccupation.Text,
                PhoneNumber = txtCitizenPhone.Text,
                FatherID = fatherID,
                MotherID = motherID,
                SpouseID = spouseID
            };

            Program.Tree.Insert(c);
            MessageBox.Show("Đã thêm công dân:" + c.FullName);
            ClearTextBoxes();
            RefreshGrid();
            DataLoader.SaveToExcel(Program.Tree, excelPath); 
        }
        //3. Chỉnh sửa thông tin công dân
        private void EditCitizen(object sender, EventArgs e)
        {
            if (!IsInputValid()) return;

            if (currentGrid.CurrentRow == null) return;

            // Lấy ID từ Grid 
            string id = currentGrid.CurrentRow.Cells["CitizenID"].Value.ToString();

            // Tìm đối tượng trong cây
            Citizen c = Program.Tree.Search(id);

            if (c != null)
            {
                string selectedGender = cbCitizenGender.Text; 
                bool isChanged = (c.FullName != txtCitizenName.Text || c.Password != txtCitizenPass.Text ||
                          c.Address != txtCitizenAddress.Text || c.Gender != selectedGender  ||
                          c.DateOfBirth.Date != dtpCitizenDOB.Value.Date || c.FatherID != txtCitizenFatherID.Text ||
                          c.MotherID != txtCitizenMotherID.Text || c.SpouseID != txtCitizenSpouseID.Text ||
                          c.Nationality != txtCitizenNationality.Text || c.Occupation != txtCitizenOccupation.Text ||
                          c.PhoneNumber != txtCitizenPhone.Text);
                if (!isChanged)
                {
                    MessageBox.Show("Dữ liệu không thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    return;
                }
                c.FullName = txtCitizenName.Text; c.Password = txtCitizenPass.Text;
                c.Address = txtCitizenAddress.Text; c.Gender = cbCitizenGender.SelectedItem.ToString();
                c.DateOfBirth = dtpCitizenDOB.Value; c.FatherID = txtCitizenFatherID.Text;
                c.MotherID = txtCitizenMotherID.Text; c.SpouseID = txtCitizenSpouseID.Text;
                c.Nationality = txtCitizenNationality.Text;
                c.Occupation = txtCitizenOccupation.Text.Trim(); c.PhoneNumber = txtCitizenPhone.Text.Trim();

                MessageBox.Show("Cập nhật thông tin công dân thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCitizenID.ReadOnly = false;
                RefreshGrid();
                ClearTextBoxes();
                DataLoader.SaveToExcel(Program.Tree, excelPath); 
            }
        }
        //4. Xóa công dân
        private void DeleteCitizen(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCitizenID.Text))
            {
                MessageBox.Show("Vui lòng chọn công dân cần xóa từ danh sách!");
                return;
            }

            string id = txtCitizenID.Text;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa công dân này?", "Xác nhận", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Program.Tree.Delete(id);

                MessageBox.Show("Đã xóa công dân thành công!");

                RefreshGrid();
                ClearTextBoxes();
                DataLoader.SaveToExcel(Program.Tree, excelPath); 
            }
        }
        //5. Tìm kiếm công dân
        private void SearchCitizen(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) { RefreshGrid(); return; }

            List<Citizen> results = new List<Citizen>();
            Citizen exactMatch = Program.Tree.Search(keyword);
            if (exactMatch != null)
                results.Add(exactMatch);
            else
            {
                List<Citizen> all = GetAll(Program.Tree.Root);

                foreach (Citizen c in all)
                {
                    if (c.CitizenID.Contains(keyword) || c.FullName.ToLower().Contains(keyword.ToLower()))
                    {
                        results.Add(c);
                    }
                }
                currentGrid.DataSource = null; currentGrid.DataSource = results;

                if (results.Count == 1)
                {
                    Citizen c = results[0];
                    txtCitizenID.Text = c.CitizenID; txtCitizenName.Text = c.FullName;
                    txtCitizenPass.Text = c.Password; txtCitizenAddress.Text = c.Address;
                    cbCitizenGender.SelectedItem = c.Gender; txtCitizenOccupation.Text = c.Occupation;
                    txtCitizenFatherID.Text = c.FatherID; txtCitizenMotherID.Text = c.MotherID;
                    txtCitizenPhone.Text = c.PhoneNumber;
                    if (c.DateOfBirth != DateTime.MinValue) dtpCitizenDOB.Value = c.DateOfBirth;
                    txtCitizenSpouseID.Text = c.SpouseID; txtCitizenNationality.Text = c.Nationality;
                }
            }
        }
        //6. Hàm xóa trắng các TextBox sau khi thêm/sửa/xóa
        private void ClearTextBoxes()
        {
            txtCitizenID.Clear(); txtCitizenName.Clear(); txtCitizenPass.Clear();
            txtCitizenAddress.Clear(); cbCitizenGender.Items.Clear(); dtpCitizenDOB.Value = DateTime.Now.AddYears(-18);
            txtCitizenFatherID.Clear(); txtCitizenMotherID.Clear(); txtCitizenSpouseID.Clear(); txtCitizenNationality.Clear();
            txtCitizenOccupation.Clear(); txtCitizenPhone.Clear();
        }
        //7. Hàm kiểm tra tính hợp lệ của dữ liệu nhập vào trước khi thêm/sửa
        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(txtCitizenName.Text) ||
                string.IsNullOrWhiteSpace(txtCitizenAddress.Text) ||
                string.IsNullOrWhiteSpace(cbCitizenGender.Text) ||
                string.IsNullOrWhiteSpace(txtCitizenPhone.Text))
            {
                MessageBox.Show("Vui lòng cung cấp đầy đủ các thông tin bắt buộc:\n" +
                                "- Họ tên\n- Quê quán\n- Giới tính\n- Số điện thoại",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        // DUYỆT CÂY ĐỂ LẤY DANH SÁCH CÔNG DÂN HIỆN CÓ TRONG HỆ THỐNG
        private List<Citizen> GetAll(AVLNode node)
        {
            List<Citizen> list = new List<Citizen>();

            InOrderTraversal(node, list);
            return list;
        }
        private void InOrderTraversal(AVLNode node, List<Citizen> list)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, list);
            list.Add(node.Data);
            InOrderTraversal(node.Right, list);
        }
        //  HELPER 
        private void ShowList(object sender, EventArgs e)
        {
            currentGrid = new DataGridView();
            currentGrid.Dock = DockStyle.Fill;

            List<Citizen> list = GetAll(Program.Tree.Root);
            currentGrid.DataSource = list;

            panelContent.Controls.Clear();
            panelContent.Controls.Add(currentGrid);
        }
        private void ViewFamily(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCitizenID.Text)) return;
            Citizen c = Program.Tree.Search(txtCitizenID.Text);
            if (c == null) return;

            Citizen f = Program.Tree.Search(c.FatherID);
            Citizen m = Program.Tree.Search(c.MotherID);

            string relation = $"Công dân: {c.FullName}\n" +
                             $"- Cha: {(f != null ? f.FullName : "Không rõ")}\n" +
                             $"- Mẹ: {(m != null ? m.FullName : "Không rõ")}";
            MessageBox.Show(relation, "Thông tin phả hệ");
        }
        // Hàm kiểm tra định dạng địa chỉ và trích xuất tỉnh
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
        // =========================================================
        // 3. USER
        // ========================================================
        private void ShowUserUI()
        {
            // =====THIẾT LẬP GIAO DIỆN CHÍNH======
            panelContent.BackgroundImage = Properties.Resources.background_dung;
            panelContent.BackgroundImageLayout = ImageLayout.Stretch;
            panelContent.Controls.Clear();

            // ===== Cấu hình menu bên trái =====
            panelMenu.Controls.Clear();
            panelMenu.BackColor = Color.FromArgb(20, 40, 100);

            PictureBox picLogo = new PictureBox()
            {
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.logo_rm,
                BackColor = Color.Transparent,
                Location = new Point((panelMenu.Width - 100) / 2, 40)
            };

            Label lblSystemName = new Label()
            {
                Text = "HỆ THỐNG\nQUẢN LÝ DÂN CƯ",
                ForeColor = Color.White,
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Width = panelMenu.Width,
                Height = 60,
                Location = new Point(10, 150)
            };

            panelMenu.Controls.Add(picLogo);
            panelMenu.Controls.Add(lblSystemName);

            //====== Main dashboard ======
            //1. Thông tin cá nhân ở phần đầu
            Panel pnlTopInfo = new Panel() { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(220, 192, 0, 0) };

            //Tên công dân 
            Label lblFullName = new Label()
            {
                Text = currentUser.FullName.ToUpper(),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            //Nút thoát
            Button btnExitTop = new Button()
            {
                Text = "Thoát",
                Size = new Size(80, 35),
                Location = new Point(pnlTopInfo.Width - 100, 10),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(192, 0, 0),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnExitTop.Click += Logout;

            pnlTopInfo.Controls.Add(btnExitTop);
            pnlTopInfo.Controls.Add(lblFullName);
            btnExitTop.BringToFront();

            //2. Panel chứa QR code ở giữa
            Panel pnlQR = new Panel() { Dock = DockStyle.Top, Height = 130, BackColor = Color.FromArgb(180, 255, 255, 255), Padding = new Padding(10) };

            pnlQR.Paint += UIHelper.Panel_Paint_BoGoc_Nhe;

            // Card QR 1: Thẻ CCCD
            Panel cardQR_CCCD = CreateQRCard("Mã QR\nThẻ CCCD", Dummy);
            cardQR_CCCD.Location = new Point(20, 15);

            // Card QR 2: Định danh điện tử
            Panel cardQR_DDDT = CreateQRCard("Mã QR\nĐịnh danh\nđiện tử", Dummy);
            cardQR_DDDT.Location = new Point(cardQR_CCCD.Right + 30, 20);

            pnlQR.Controls.Add(cardQR_CCCD);
            pnlQR.Controls.Add(cardQR_DDDT);

            //3. Panel chính chứa các tiện ích yêu thích và dịch vụ phổ biến
            FlowLayoutPanel flowBody = new FlowLayoutPanel() { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true, BackColor = Color.Transparent };
            panelContent.Controls.Add(flowBody);
            panelContent.Controls.Add(pnlQR);
            panelContent.Controls.Add(pnlTopInfo);

            flowBody.BringToFront();

            // Tiện ích yêu thích
            AddGroupTitle(flowBody, "Tiện ích yêu thích");
            flowBody.Controls.Add(CreateServiceCard("Thẻ CCCD", Properties.Resources.citizen, ShowCCCD));
            flowBody.Controls.Add(CreateServiceCard("GPLX", Properties.Resources.gplx, Dummy));
            flowBody.Controls.Add(CreateServiceCard("BHYT", Properties.Resources.bhyt, Dummy));
            flowBody.Controls.Add(CreateServiceCard("Thông tin cư trú", Properties.Resources.ttct, Dummy));
            flowBody.Controls.Add(CreateServiceCard("Đổi mật khẩu", Properties.Resources.password, ShowChangePassword));

            // Dịch vụ phổ biến
            AddGroupTitle(flowBody, "Dịch vụ phổ biến");
            flowBody.Controls.Add(CreateServiceCard("Thủ tục\nhành chính", Properties.Resources.tthc, Dummy));
            flowBody.Controls.Add(CreateServiceCard("An sinh\nxã hội", Properties.Resources.asxh, Dummy));
            flowBody.Controls.Add(CreateServiceCard("Hồ sơ\nsức khỏe", Properties.Resources.hssk, Dummy));
            flowBody.Controls.Add(CreateServiceCard("Dịch vụ\nkhác", Properties.Resources.khac, Dummy));

            panelContent.Controls.Add(flowBody);
            panelContent.Controls.Add(pnlQR);

            panelContent.Controls.Add(pnlTopInfo);

        }
        // CÁC HÀM HỖ TRỢ 
        private Panel CreateServiceCard(string title, Image avt, EventHandler clickEvent)
        {
            Panel card = new Panel { Size = new Size(125, 135), BackColor = Color.White, Margin = new Padding(15), Cursor = Cursors.Hand, Padding = new Padding(10) };

            card.Click += clickEvent;

            UIHelper.ApplyRegion(card, 15);

            PictureBox picAvt = new PictureBox
            {
                Image = avt,
                Dock = DockStyle.Fill,
                Size = new Size(45, 45),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point((card.Width - 50) / 2, 15),
                BackColor = Color.Transparent,
                Enabled = false
            };
            picAvt.Click += clickEvent;

            Label lbl = new Label
            {
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 45,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Enabled = false
            };
            lbl.Click += clickEvent;

            card.Controls.Add(picAvt);
            card.Controls.Add(lbl);

            card.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(245, 245, 245); card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.BackColor = Color.White; card.Invalidate(); };

            return card;
        }
        private Panel CreateQRCard(string text, EventHandler clickEvent)
        {
            Panel card = new Panel { Size = new Size(200, 90), BackColor = Color.FromArgb(250, 240, 210), Cursor = Cursors.Hand };
            card.Click += clickEvent;

            UIHelper.ApplyRegion(card, 15);

            PictureBox picQR = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(15, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.may_quét_qr,
                BackColor = Color.White,
            };
            picQR.Click += clickEvent;

            Label lbl = new Label { Text = text, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(85, 20), AutoSize = true };
            lbl.Click += clickEvent;

            card.Controls.Add(picQR);
            card.Controls.Add(lbl);
            return card;
        }
        private void AddGroupTitle(FlowLayoutPanel container, string text)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = container.Width - 40,
                Height = 30,
                Margin = new Padding(0, 20, 0, 5)
            };
            container.Controls.Add(lbl);
        }
        // HÀM HIỂN THỊ GIAO DIỆN ĐỔI MẬT KHẨU VỚI CAPTCHA VÀ KIỂM TRA QUY ĐỊNH MẬT KHẨU MỚI
        private void ShowChangePassword(object sender, EventArgs e)
        {
            Form f = new Form()
            {
                Text = "ĐỔI MẬT KHẨU",
                Size = new Size(380, 520),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackgroundImage = Properties.Resources.background_dung,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            string currentCaptcha = "";

            Label lblOld = new Label()
            {
                Text = "Mật khẩu hiện tại:",
                Location = new Point(30, 20),
                Width = 150,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            TextBox txtOld = new TextBox() { Location = new Point(30, 45), Width = 300, PasswordChar = '●' };

            Label lblNew = new Label()
            {
                Text = "Mật khẩu mới:",
                Location = new Point(30, 85),
                Width = 150,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            TextBox txtNew = new TextBox() { Location = new Point(30, 110), Width = 300, PasswordChar = '●' };

            Label lblPolicy = new Label()
            {
                Text = "⚠️ Quy định mật khẩu mới:\n" +
                       "• Độ dài từ 6 ký tự trở lên\n" +
                       "• Có chữ HOA, chữ thường và số\n" +
                       "• Có ít nhất 1 ký tự đặc biệt (!@#$^*...)",
                Location = new Point(30, 145),
                Size = new Size(300, 70),
                ForeColor = Color.DarkBlue,
                BackColor = Color.FromArgb(180, 255, 255, 255),
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 8, FontStyle.Italic)
            };

            Label lblCap = new Label()
            {
                Text = "Mã xác thực:",
                Location = new Point(30, 240),
                Width = 150,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            PictureBox picCap = new PictureBox()
            {
                Location = new Point(30, 265),
                Size = new Size(120, 40),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                BackColor = Color.White
            };

            TextBox txtCapInput = new TextBox() { Location = new Point(160, 275), Size = new Size(170, 25), Font = new Font("Segoe UI", 10) };

            Action RefreshCaptcha = () => { currentCaptcha = UIHelper.GenerateCaptchaImage(picCap); };
            picCap.Click += (s, ev) => RefreshCaptcha();
            RefreshCaptcha();

            Button btnUpdate = new Button()
            {
                Text = "XÁC NHẬN ĐỔI MẬT KHẨU",
                Location = new Point(30, 340),
                Width = 300,
                Height = 45,
                BackColor = Color.FromArgb(20, 40, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnUpdate.Click += (se, ev) => {
                string newPass = txtNew.Text;


                if (txtCapInput.Text != currentCaptcha)
                {
                    MessageBox.Show("Mã xác thực không chính xác!", "Lỗi");
                    RefreshCaptcha();
                    return;
                }


                bool isValid = newPass.Length >= 6 &&
                               newPass.Any(char.IsUpper) &&
                               newPass.Any(char.IsLower) &&
                               newPass.Any(char.IsDigit) &&
                               newPass.Any(ch => !char.IsLetterOrDigit(ch));

                if (!isValid)
                {
                    MessageBox.Show("Mật khẩu mới không đúng quy định!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtOld.Text == currentUser.Password)
                {

                    currentUser.Password = newPass;

                    MessageBox.Show("Cập nhật mật khẩu mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.Close();
                }
                else
                {
                    MessageBox.Show("Mật khẩu hiện tại không chính xác!", "Lỗi");
                    RefreshCaptcha();
                }
            };

            f.Controls.AddRange(new Control[] { lblOld, txtOld, lblNew, txtNew, lblPolicy, lblCap, picCap, txtCapInput, btnUpdate });
            f.ShowDialog();
        }
        // CCCDForm
        private void ShowCCCD(object sender, EventArgs e)
        {
            CCCD form = new CCCD(currentUser);
            form.ShowDialog();
        }
    }
}
