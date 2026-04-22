using DO_AN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN
{
    internal static class Program
    {
        public static AVL Tree = new AVL();

        [STAThread]
        static void Main()
        {
            string excelPath = @"C:\Users\pc\OneDrive\Desktop\Tài liệu\Quốc Huy\DO_AN\DO_AN\bin\Debug\data.xlsx";
            DataLoader.LoadFromExcel(Tree, excelPath);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool keepRunning = true;
            while (keepRunning)
            {
                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        Citizen user = login.AuthenticatedUser;

                        using (DashboardForm dashboard = new DashboardForm(user))
                        {
                            if (dashboard.ShowDialog() != DialogResult.OK)
                            {
                                keepRunning = false;
                            }
                        }
                    }
                    else
                    {
                        keepRunning = false;
                    }
                }
            }
        }
    }
}

