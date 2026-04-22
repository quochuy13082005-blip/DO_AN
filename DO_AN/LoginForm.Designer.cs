namespace DO_AN
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlInputID;
        private System.Windows.Forms.Panel pnlInputPass;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.TextBox txtCitizenID;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button BtnRegister;
        private System.Windows.Forms.ProgressBar loadingBar;
        private System.Windows.Forms.PictureBox picShowPass;
        private System.Windows.Forms.Button BtnForgotPassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.logo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlInputID = new System.Windows.Forms.Panel();
            this.txtCitizenID = new System.Windows.Forms.TextBox();
            this.pnlInputPass = new System.Windows.Forms.Panel();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.picShowPass = new System.Windows.Forms.PictureBox();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.BtnRegister = new System.Windows.Forms.Button();
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            this.BtnForgotPassword = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.pnlInputID.SuspendLayout();
            this.pnlInputPass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.Image = global::DO_AN.Properties.Resources.logo_rm;
            this.logo.Location = new System.Drawing.Point(113, -1);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(190, 158);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logo.TabIndex = 10;
            this.logo.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblTitle.Location = new System.Drawing.Point(12, 145);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(380, 40);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "ĐỊNH DANH ĐIỆN TỬ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlInputID
            // 
            this.pnlInputID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlInputID.Controls.Add(this.txtCitizenID);
            this.pnlInputID.Location = new System.Drawing.Point(50, 206);
            this.pnlInputID.Name = "pnlInputID";
            this.pnlInputID.Size = new System.Drawing.Size(320, 45);
            this.pnlInputID.TabIndex = 2;
            // 
            // txtCitizenID
            // 
            this.txtCitizenID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtCitizenID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCitizenID.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCitizenID.Location = new System.Drawing.Point(10, 12);
            this.txtCitizenID.Name = "txtCitizenID";
            this.txtCitizenID.Size = new System.Drawing.Size(300, 27);
            this.txtCitizenID.TabIndex = 0;
            // 
            // pnlInputPass
            // 
            this.pnlInputPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlInputPass.Controls.Add(this.txtPass);
            this.pnlInputPass.Controls.Add(this.picShowPass);
            this.pnlInputPass.Location = new System.Drawing.Point(50, 273);
            this.pnlInputPass.Name = "pnlInputPass";
            this.pnlInputPass.Size = new System.Drawing.Size(320, 45);
            this.pnlInputPass.TabIndex = 3;
            // 
            // txtPass
            // 
            this.txtPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPass.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPass.Location = new System.Drawing.Point(15, 12);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '●';
            this.txtPass.Size = new System.Drawing.Size(250, 25);
            this.txtPass.TabIndex = 0;
            // 
            // picShowPass
            // 
            this.picShowPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picShowPass.Image = global::DO_AN.Properties.Resources.eye_close;
            this.picShowPass.Location = new System.Drawing.Point(280, 10);
            this.picShowPass.Name = "picShowPass";
            this.picShowPass.Size = new System.Drawing.Size(25, 25);
            this.picShowPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShowPass.TabIndex = 1;
            this.picShowPass.TabStop = false;
            // 
            // picCaptcha
            // 
            this.picCaptcha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCaptcha.Location = new System.Drawing.Point(50, 340);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(131, 55);
            this.picCaptcha.TabIndex = 4;
            this.picCaptcha.TabStop = false;
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCaptcha.Location = new System.Drawing.Point(187, 351);
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(179, 34);
            this.txtCaptcha.TabIndex = 5;
            // 
            // BtnLogin
            // 
            this.BtnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.BtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.BtnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.BtnLogin.Location = new System.Drawing.Point(50, 417);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(320, 50);
            this.BtnLogin.TabIndex = 16;
            this.BtnLogin.Text = "ĐĂNG NHẬP";
            this.BtnLogin.UseVisualStyleBackColor = false;
            // 
            // BtnRegister
            // 
            this.BtnRegister.FlatAppearance.BorderSize = 0;
            this.BtnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRegister.Location = new System.Drawing.Point(50, 473);
            this.BtnRegister.Name = "BtnRegister";
            this.BtnRegister.Size = new System.Drawing.Size(320, 30);
            this.BtnRegister.TabIndex = 17;
            this.BtnRegister.Text = "Chưa có tài khoản? Đăng ký ngay";
            // 
            // loadingBar
            // 
            this.loadingBar.Location = new System.Drawing.Point(50, 401);
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(320, 10);
            this.loadingBar.TabIndex = 8;
            this.loadingBar.Visible = false;
            // 
            // BtnForgotPassword
            // 
            this.BtnForgotPassword.BackColor = System.Drawing.Color.Transparent;
            this.BtnForgotPassword.FlatAppearance.BorderSize = 0;
            this.BtnForgotPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnForgotPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.BtnForgotPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.BtnForgotPassword.Location = new System.Drawing.Point(230, 316);
            this.BtnForgotPassword.Name = "BtnForgotPassword";
            this.BtnForgotPassword.Size = new System.Drawing.Size(140, 30);
            this.BtnForgotPassword.TabIndex = 19;
            this.BtnForgotPassword.Text = "Quên mật khẩu?";
            this.BtnForgotPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnForgotPassword.UseVisualStyleBackColor = false;
            // 
            // LoginForm
            // 
            this.BackgroundImage = global::DO_AN.Properties.Resources.background_dung;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(420, 520);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.pnlInputID);
            this.Controls.Add(this.pnlInputPass);
            this.Controls.Add(this.BtnForgotPassword);
            this.Controls.Add(this.picCaptcha);
            this.Controls.Add(this.txtCaptcha);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.BtnRegister);
            this.Controls.Add(this.loadingBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.pnlInputID.ResumeLayout(false);
            this.pnlInputID.PerformLayout();
            this.pnlInputPass.ResumeLayout(false);
            this.pnlInputPass.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
