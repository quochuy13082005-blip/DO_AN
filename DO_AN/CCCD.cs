using DO_AN;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DO_AN
{
    public partial class CCCD : Form
    {

        private dynamic _user; // Sử dụng dynamic để linh hoạt với các kiểu dữ liệu khác nhau (Citizen, User, v.v.)

        // =========================================================
        // 1. HÀM KHỞI TẠO 
        // =========================================================

        public CCCD(Citizen user)
        {
            InitializeComponent();
            this._user = user;

            SetupFormLayout();
            RegisterEvents();
            LoadData();
            DisplayIdentityInfo();
        }
        // =========================================================
        // 2. THIẾT LẬP GIAO DIỆN
        // =========================================================
        private void SetupFormLayout()
        {
            this.Size = new Size(700, 650);
            this.Icon = Properties.Resources.logo;

            if (pnlCard != null)
            {
                pnlCard.Width = 650;
                pnlCard.Height = 300;
                pnlCard.Left = (this.ClientSize.Width - pnlCard.Width) / 2;
            }
        }

        // =========================================================
        // 3. HIỂN THỊ DỮ LIỆU 
        // =========================================================

        private void DisplayIdentityInfo()
        {
            if (_user == null) return;

            pnlCard.Controls.Clear();

            pnlCard.BackgroundImage = Properties.Resources.background;
            pnlCard.BackgroundImageLayout = ImageLayout.Stretch;

            Label lblHeaderCard = new Label
            {
                Text = "CĂN CƯỚC ĐIỆN TỬ",
                Font = new Font("Times New Roman", 19F, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = false,
                Width = pnlCard.Width,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 5),
                BackColor = Color.Transparent
            };
            pnlCard.Controls.Add(lblHeaderCard);

            PictureBox picAvatar = new PictureBox
            {
                Size = new Size(110, 140),
                Location = new Point(25, 90),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(230, 230, 230),
                BorderStyle = BorderStyle.FixedSingle,
                Image = Properties.Resources.avata_citizen
            };
            pnlCard.Controls.Add(picAvatar);

            float startX = 145;

            AddInfoLabel("Số / No.: " + (_user.CitizenID ?? ""), new Point((int)startX, 65), true, 11);

            AddInfoLabel("Họ và tên / Full name:", new Point((int)startX, 95), false, 8);
            string name = _user.FullName ?? "";
            AddInfoLabel(name.ToUpper(), new Point((int)startX, 110), true, 11);

            string dob = "";
            try
            {
                if (_user.DateOfBirth is DateTime) dob = _user.DateOfBirth.ToString("dd/MM/yyyy");
                else dob = _user.DateOfBirth.ToString();
            }
            catch { dob = "01/01/2000"; }

            AddInfoLabel("Ngày sinh / Date of birth: " + dob, new Point((int)startX, 140), true, 9);

            AddInfoLabel("Giới tính / Sex: " + (_user.Gender ?? "Nam"), new Point((int)startX, 160), true, 9);
            AddInfoLabel("Quốc tịch / Nationality: Việt Nam", new Point(275, 160), true, 9);

            AddInfoLabel("Quê quán / Place of origin:", new Point((int)startX, 185), false, 8);
            AddInfoLabel(_user.Address ?? "", new Point((int)startX, 200), true, 9);

            AddInfoLabel("Nơi thường trú / Place of residence:", new Point((int)startX, 225), false, 8);
            AddInfoLabel(_user.Address ?? "", new Point((int)startX, 240), true, 9);
        }

        private void LoadData()
        {
            pnlFooter.Controls.Clear();
            pnlFooter.Controls.Add(CreateMenuOption("→ Căn cước điện tử"));
            pnlFooter.Controls.Add(CreateMenuOption("→ Lịch sử cấp thẻ CC/CCCD/CMND"));
        }

        // =========================================================
        // 5. SỰ KIỆN VÀ HÀM HỖ TRỢ 
        // =========================================================

        private void RegisterEvents()
        {
            btnBack.Click += (s, e) => this.Close();

            foreach (Control c in pnlCard.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.BorderStyle = BorderStyle.None;
                    txt.BackColor = Color.FromArgb(240, 240, 240);
                    txt.ReadOnly = true;
                    txt.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }
        private void AddInfoLabel(string text, Point location, bool isBold, int fontSize)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = location,
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.Black
            };
            pnlCard.Controls.Add(lbl);
            lbl.BringToFront();
        }

        private Label CreateMenuOption(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 0, 0),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
        }
    }
}