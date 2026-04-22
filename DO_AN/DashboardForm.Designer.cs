namespace DO_AN
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblUser;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ===== panelMenu =====
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Width = 200;

            // ===== panelContent =====
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.BackColor = System.Drawing.Color.White;

            // ===== lblUser =====
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUser.Location = new System.Drawing.Point(210, 10);

            // ===== Form =====
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);

            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashboardForm_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}