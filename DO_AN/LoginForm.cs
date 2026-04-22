using DO_AN;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN
{
    public partial class LoginForm : Form
    {
        private string _captchaText;
        public Citizen AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Icon = Properties.Resources.logo;

            //Bo góc cho form và các control
            UIHelper.ApplyRegion(pnlInputID, 15);
            UIHelper.ApplyRegion(pnlInputPass, 15);
            UIHelper.ApplyRegion(BtnLogin, 10);

            //Cấu hình placeholder
            UIHelper.SetPlaceholder(txtCitizenID, "Số định danh");
            UIHelper.SetPlaceholder(txtPass, "Mật khẩu", true);
            UIHelper.SetPlaceholder(txtCaptcha, "Mã xác thực");

            // Sự kiện
            picShowPass.Click += TogglePasswordVisibility;
            picCaptcha.Click += (s, e) => GenerateCaptcha();
            BtnLogin.Click += BtnLogin_Click;
            BtnRegister.Click += BtnRegister_Click;
            this.Shown += (s, e) => GenerateCaptcha();

            // Hiệu ứng tương tác
            UIHelper.AddHoverEffect(BtnLogin, Color.FromArgb(13, 71, 161), Color.FromArgb(25, 118, 210));
            loadingBar.Visible = false;
        }
        // Xử lý sự kiện đăng nhập
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            Citizen user = Program.Tree.Search(txtCitizenID.Text.Trim());

            if (user == null || user.Password != txtPass.Text)
            {
                await UIHelper.ShakeFormAsync(this);
                MessageBox.Show("Thông tin đăng nhập không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GenerateCaptcha();
                return;
            }

            BtnLogin.Enabled = false;
            loadingBar.Visible = true;
            for (int i = 0; i <= 100; i += 10)
            {
                loadingBar.Value = i;
                await Task.Delay(50);
            }

            this.AuthenticatedUser = user;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        // Xử lý sự kiện đăng ký
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm().ShowDialog();
        }
        // Xử lý sự kiện quên mật khẩu
        private void BtnForgotPassword_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị viên: 0798.***.***", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Xử lý sự kiện hiển thị mật khẩu
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            if (txtPass.ForeColor == Color.Gray) return;
            bool isCurrentlyHidden = (txtPass.PasswordChar == '●' || txtPass.PasswordChar == '*');

            if (isCurrentlyHidden)
            {
                txtPass.PasswordChar = '\0';
                picShowPass.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtPass.PasswordChar = '●';
                picShowPass.Image = Properties.Resources.eye_close;
            }
        }
        // Tạo mã Captcha mới
        private void GenerateCaptcha()
        {
            _captchaText = UIHelper.GenerateCaptchaImage(picCaptcha);
        }
        // Kiểm tra tính hợp lệ của các trường nhập liệu
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCitizenID.Text) || txtCitizenID.Text.Contains("Số định danh"))
                return Error("Nhập Citizen ID!");

            if (string.IsNullOrWhiteSpace(txtPass.Text) || txtPass.Text.Contains("Mật khẩu"))
                return Error("Nhập mật khẩu!");

            if (txtCaptcha.Text != _captchaText)
            {
                GenerateCaptcha();
                return Error("Mã Captcha sai!");
            }
            return true;
        }
        // Hiển thị thông báo lỗi
        private bool Error(string msg)
        {
            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}