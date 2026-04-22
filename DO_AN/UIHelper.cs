using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN
{
    internal class UIHelper
    {
        // =========================================================
        // 1. NHÓM HIỆU ỨNG VÀ TƯƠNG TÁC 
        // =========================================================

        //Xử lý nhập liệu
        public static void SetPlaceholder(TextBox txt, string placeholder, bool isPassword = false)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;
            if (isPassword)
                txt.PasswordChar = '\0';
            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                    if (isPassword) txt.PasswordChar = '●';
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                    if (isPassword) txt.PasswordChar = '\0';
                }
            };
        }

        //Hiệu ứng tương tác
        public static void AddHoverEffect(Button btn, Color normalColor, Color hoverColor)
        {
            btn.BackColor = normalColor;
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = normalColor;
        }

        // Rung khi đăng nhập sai
        public static async Task ShakeFormAsync(Form form, int intensity = 10, int delay = 15, int times = 8)
        {
            if (form == null) return;

            Point originalLocation = form.Location;
            Random rnd = new Random();

            for (int i = 0; i < times; i++)
            {
                int offsetX = rnd.Next(-intensity, intensity);
                int offsetY = rnd.Next(-intensity, intensity);
                form.Location = new Point(originalLocation.X + offsetX, originalLocation.Y + offsetY);
                await Task.Delay(delay);
            }

            form.Location = originalLocation;
        }

        // =========================================================
        // 2.ĐỒ HỌA VÀ BO GÓC 
        // =========================================================
        // Tạo đường dẫn bo góc
        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
        // Áp dụng bo góc cho control
        public static void ApplyRegion(Control control, int radius)
        {
            void UpdateRegion()
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    control.Region = new Region(path);
                }
            }
            control.HandleCreated += (s, e) => UpdateRegion();
            control.SizeChanged += (s, e) => UpdateRegion();
        }
        // Sử dụng bo góc cho panel
        public static void Panel_Paint_BoGoc_Nhe(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int r = 15;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddArc(0, 0, r, r, 180, 90);
            gp.AddArc(p.Width - r, 0, r, r, 270, 90);
            gp.AddArc(p.Width - r, p.Height - r, r, r, 0, 90);
            gp.AddArc(0, p.Height - r, r, r, 90, 90);
            p.Region = new Region(gp);
        }

        // =========================================================
        // 3.TẠO NỘI DUNG HÌNH ẢNH 
        // =========================================================
        // Tạo CAPTCHA
        public static string GenerateCaptchaImage(PictureBox pic)
        {
            Random rnd = new Random();

            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            string captcha = "";

            for (int i = 0; i < 4; i++)
                captcha += chars[rnd.Next(chars.Length)];

            Bitmap bmp = new Bitmap(pic.Width, pic.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);

            string[] fonts = { "Times New Roman", "Arial", "Tahoma", "Calibri", "Verdana", "Georgia", "Comic Sans MS" };

            for (int i = 0; i < captcha.Length; i++)
            {
                string fontName = fonts[rnd.Next(fonts.Length)];

                Font font = new Font(fontName, rnd.Next(18, 24), FontStyle.Bold);

                Brush brush = new SolidBrush(Color.FromArgb(
                    rnd.Next(20, 150),
                    rnd.Next(20, 150),
                    rnd.Next(20, 150)));

                int x = 5 + i * 20;
                int y = rnd.Next(0, 10);

                g.DrawString(captcha[i].ToString(), font, brush, x, y);
            }

            // vẽ đường nhiễu
            for (int i = 0; i < 5; i++)
            {
                Pen pen = new Pen(Color.Gray);

                g.DrawLine(
                    pen,
                    rnd.Next(pic.Width),
                    rnd.Next(pic.Height),
                    rnd.Next(pic.Width),
                    rnd.Next(pic.Height));
            }

            // vẽ chấm nhiễu
            for (int i = 0; i < 30; i++)
            {
                bmp.SetPixel(
                    rnd.Next(pic.Width),
                    rnd.Next(pic.Height),
                    Color.Gray);
            }

            pic.Image = bmp;

            return captcha;
        }

        // =========================================================
        // 4. ĐIỀU KHIỂN GIAO DIỆN 
        // =========================================================

        public static void AddMenuButton(string text, EventHandler click, Control parent, bool isExit)
        {
            Button btn = new Button
            {
                Text = text,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = isExit ? DockStyle.Bottom : DockStyle.Top
            };
            btn.FlatAppearance.BorderSize = 0;

            if (isExit)
            {
                btn.BackColor = Color.FromArgb(192, 57, 43);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 62, 80);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 40, 50);
            }

            btn.Click += click;
            parent.Controls.Add(btn);

            if (!isExit) btn.BringToFront();
        }
        public static Panel CreateServiceCard(string title, Image avt, EventHandler clickEvent)
        {
            Panel card = new Panel { Size = new Size(125, 135), BackColor = Color.White, Margin = new Padding(15), Cursor = Cursors.Hand, Padding = new Padding(10) };
            card.Click += clickEvent;

            // Hiệu ứng Bo góc & Hover
            card.Paint += (s, e) =>
            {
                GraphicsPath gp = GetRoundedPath(card.ClientRectangle, 15);
                card.Region = new Region(gp);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1)) e.Graphics.DrawPath(pen, gp);
            };

            PictureBox pic = new PictureBox { Image = avt, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Enabled = false };
            Label lbl = new Label { Text = title, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Bottom, Height = 45, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), Enabled = false };

            card.Controls.Add(pic);
            card.Controls.Add(lbl);

            card.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(245, 245, 245); };
            card.MouseLeave += (s, e) => { card.BackColor = Color.White; };

            return card;
        }
    }
}
