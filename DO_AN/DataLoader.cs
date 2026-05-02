using DO_AN;
using OfficeOpenXml; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace DO_AN
{
    public class DataLoader
    {
        public static Dictionary<string, string> provinceMap = new Dictionary<string, string>()
        {
            {"001","Hà Nội"},{"002","Hà Giang"},{"004","Cao Bằng"},{"006","Bắc Kạn"},
            {"008","Tuyên Quang"},{"010","Lào Cai"},{"011","Điện Biên"},{"012","Lai Châu"},
            {"014","Sơn La"},{"015","Yên Bái"},{"017","Hòa Bình"},{"019","Thái Nguyên"},
            {"020","Lạng Sơn"},{"022","Quảng Ninh"},{"024","Bắc Giang"},{"025","Phú Thọ"},
            {"026","Vĩnh Phúc"},{"027","Bắc Ninh"},{"030","Hải Dương"},{"031","Hải Phòng"},
            {"033","Hưng Yên"},{"034","Thái Bình"},{"035","Hà Nam"},{"036","Nam Định"},
            {"037","Ninh Bình"},{"038","Thanh Hóa"},{"040","Nghệ An"},{"042","Hà Tĩnh"},
            {"044","Quảng Bình"},{"045","Quảng Trị"},{"046","Thừa Thiên Huế"},{"048","Đà Nẵng"},
            {"049","Quảng Nam"},{"051","Quảng Ngãi"},{"052","Bình Định"},{"054","Phú Yên"},
            {"056","Khánh Hòa"},{"058","Ninh Thuận"},{"060","Bình Thuận"},{"062","Kon Tum"},
            {"064","Gia Lai"},{"066","Đắk Lắk"},{"067","Đắk Nông"},{"068","Lâm Đồng"},
            {"070","Bình Phước"},{"072","Tây Ninh"},{"074","Bình Dương"},{"075","Đồng Nai"},
            {"077","Bà Rịa - Vũng Tàu"},{"079","TP.HCM"},{"080","Long An"},{"082","Tiền Giang"},
            {"083","Bến Tre"},{"084","Trà Vinh"},{"086","Vĩnh Long"},{"087","Đồng Tháp"},
            {"089","An Giang"},{"091","Kiên Giang"},{"092","Cần Thơ"},{"093","Hậu Giang"},
            {"094","Sóc Trăng"},{"095","Bạc Liêu"},{"096","Cà Mau"}
        };

        public static void LoadFromExcel(AVL tree, string filePath)
        {
            CreateFixedAccounts(tree); 

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Lỗi: Không tìm thấy file dữ liệu tại " + filePath);
                return;
            }

            ExcelPackage.License.SetNonCommercialPersonal("DO An");

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                List<Citizen> tempSample = new List<Citizen>();

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            Citizen c = new Citizen();

                            c.CitizenID = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(c.CitizenID)) continue;

                            c.FullName = worksheet.Cells[row, 2].Value?.ToString();

                            string dobStr = worksheet.Cells[row, 3].Value?.ToString();
                            if (DateTime.TryParse(dobStr, out DateTime dob))
                                c.DateOfBirth = dob;

                            c.Gender = worksheet.Cells[row, 4].Value?.ToString(); 
                            c.Address = worksheet.Cells[row, 5].Value?.ToString(); 

                            c.Nationality = worksheet.Cells[row, 6].Value?.ToString();
                            c.PhoneNumber = worksheet.Cells[row, 7].Value?.ToString(); 
                            c.Occupation = worksheet.Cells[row, 8].Value?.ToString();  
                            c.Password = worksheet.Cells[row, 9].Value?.ToString(); 

                            c.FatherID = worksheet.Cells[row, 10].Value?.ToString() ?? "null"; 
                            c.MotherID = worksheet.Cells[row, 11].Value?.ToString() ?? "null"; 
                            c.SpouseID = worksheet.Cells[row, 12].Value?.ToString() ?? "null"; 

                            tree.Insert(c);
                            if (tempSample.Count < 5) tempSample.Add(c);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi tại dòng {row}: {ex.Message}");
                        }
                    }
                }
                Console.WriteLine("Nạp dữ liệu từ Excel thành công!");
                Console.WriteLine("\n=== DANH SÁCH 5 CÔNG DÂN NGẪU NHIÊN VỪA TẠO ===");
                foreach (Citizen citizen in tempSample)
                {
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("ID: " + citizen.CitizenID);
                    Console.WriteLine("Pass: " + citizen.Password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi hệ thống khi đọc Excel: " + ex.Message);
            }
        }       
        private static void CreateFixedAccounts(AVL tree) 
        {
            // Tài khoản Admin luôn phải có
            tree.Insert(new Citizen
            {
                CitizenID = "admin001",
                FullName = "Quản Trị Viên",
                Password = "123",
                Address = "Hệ thống"
            });
        }
        public static string RemoveDiacritics(string text) 
        { 
            if (string.IsNullOrEmpty(text)) return text; 
            string[] vietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ","íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ","ýỳỵỷỹ", "ÝỲỴỶỸ"
            };
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                    text = text.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
            }
            return text;
        }
        public static void SaveToExcel(AVL tree, string filePath) 
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("DO An");
                FileInfo fileInfo = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                        ? package.Workbook.Worksheets[0]
                        : package.Workbook.Worksheets.Add("Citizens");

                    if (worksheet.Dimension != null)
                        worksheet.Cells.Clear();

                    string[] headers = { "CitizenID", "FullName", "DOB", "Gender", "Address", "Phone", "Occupation", "Password", "FatherID", "MotherID", "SpouseID" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true; 
                    }

                    List<Citizen> list = tree.GetAllCitizens();

                    int row = 2;
                    foreach (Citizen c in list)
                    {
                        if (c.CitizenID.ToLower().StartsWith("admin")) continue;

                        worksheet.Cells[row, 1].Value = c.CitizenID;
                        worksheet.Cells[row, 2].Value = c.FullName;
                        worksheet.Cells[row, 3].Value = c.DateOfBirth.ToString("dd/MM/yyyy");
                        worksheet.Cells[row, 4].Value = c.Gender;
                        worksheet.Cells[row, 5].Value = c.Address;
                        worksheet.Cells[row, 6].Value = c.PhoneNumber;
                        worksheet.Cells[row, 7].Value = c.Occupation;
                        worksheet.Cells[row, 8].Value = c.Password;
                        worksheet.Cells[row, 9].Value = c.FatherID;
                        worksheet.Cells[row, 10].Value = c.MotherID;
                        worksheet.Cells[row, 11].Value = c.SpouseID;
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();

                    package.Save();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi lưu dữ liệu vào Excel: " + ex.Message, "Lỗi Hệ Thống");
            }
        }
    }
}