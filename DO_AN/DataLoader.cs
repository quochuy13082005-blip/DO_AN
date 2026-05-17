using DO_AN;
using OfficeOpenXml; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DO_AN
{
    public class MyDictionary<TKey, TValue>
    {
        private class Entry
        {
            public TKey Key;
            public TValue Value;
            public Entry Next;
        }
        private Entry[] _buckets;
        private int _size;
        public MyDictionary(int size = 100)
        {
            _size = size;
            _buckets = new Entry[_size];
        }
        private int GetHash(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % _size;
        }
        public void Add(TKey key, TValue value)
        {
            int index = GetHash(key);
            Entry newEntry = new Entry { Key = key, Value = value, Next = _buckets[index] };
            _buckets[index] = newEntry;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetHash(key);
            Entry current = _buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    value = current.Value;
                    return true;
                }
                current = current.Next;
            }
            value = default;
            return false;
        }
    }
    public class DataLoader
    {
        public static MyDictionary<string, string> provinceMap = new MyDictionary<string, string>();
        static DataLoader()
        {
            provinceMap.Add("001", "Hà Nội");provinceMap.Add("002", "Hà Giang");provinceMap.Add("004", "Cao Bằng");
            provinceMap.Add("006", "Bắc Kạn");provinceMap.Add("008", "Tuyên Quang");provinceMap.Add("010", "Lào Cai");
            provinceMap.Add("011", "Điện Biên");provinceMap.Add("012", "Lai Châu");provinceMap.Add("014", "Sơn La");
            provinceMap.Add("015", "Yên Bái");provinceMap.Add("017", "Hòa Bình");provinceMap.Add("019", "Thái Nguyên");
            provinceMap.Add("020", "Lạng Sơn");provinceMap.Add("022", "Quảng Ninh");provinceMap.Add("024", "Bắc Giang");
            provinceMap.Add("025", "Phú Thọ");provinceMap.Add("026", "Vĩnh Phúc");provinceMap.Add("027", "Bắc Ninh");
            provinceMap.Add("030", "Hải Dương");provinceMap.Add("031", "Hải Phòng");provinceMap.Add("033", "Hưng Yên");
            provinceMap.Add("034", "Thái Bình");provinceMap.Add("035", "Hà Nam");provinceMap.Add("036", "Nam Định");
            provinceMap.Add("037", "Ninh Bình");provinceMap.Add("038", "Thanh Hóa");provinceMap.Add("040", "Nghệ An");
            provinceMap.Add("042", "Hà Tĩnh");provinceMap.Add("044", "Quảng Bình");provinceMap.Add("045", "Quảng Trị");
            provinceMap.Add("046", "Thừa Thiên Huế");provinceMap.Add("048", "Đà Nẵng");provinceMap.Add("049", "Quảng Nam");
            provinceMap.Add("051", "Quảng Ngãi");provinceMap.Add("052", "Bình Định");provinceMap.Add("054", "Phú Yên");
            provinceMap.Add("056", "Khánh Hòa");provinceMap.Add("058", "Ninh Thuận");provinceMap.Add("060", "Bình Thuận");
            provinceMap.Add("062", "Kon Tum");provinceMap.Add("064", "Gia Lai");provinceMap.Add("066", "Đắk Lắk");
            provinceMap.Add("067", "Đắk Nông");provinceMap.Add("068", "Lâm Đồng");provinceMap.Add("070", "Bình Phước");
            provinceMap.Add("072", "Tây Ninh");provinceMap.Add("074", "Bình Dương");provinceMap.Add("075", "Đồng Nai");
            provinceMap.Add("077", "Bà Rịa - Vũng Tàu");provinceMap.Add("079", "TP.HCM");provinceMap.Add("080", "Long An");
            provinceMap.Add("082", "Tiền Giang");provinceMap.Add("083", "Bến Tre");provinceMap.Add("084", "Trà Vinh");
            provinceMap.Add("086", "Vĩnh Long");provinceMap.Add("087", "Đồng Tháp");provinceMap.Add("089", "An Giang");
            provinceMap.Add("091", "Kiên Giang");provinceMap.Add("092", "Cần Thơ");provinceMap.Add("093", "Hậu Giang");
            provinceMap.Add("094", "Sóc Trăng");provinceMap.Add("095", "Bạc Liêu");provinceMap.Add("096", "Cà Mau");
        }

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
                Debug.WriteLine("Nạp dữ liệu từ Excel thành công!");
                Debug.WriteLine("\n=== DANH SÁCH 5 CÔNG DÂN NGẪU NHIÊN ===");
                foreach (Citizen citizen in tempSample)
                {
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("ID: " + citizen.CitizenID);
                    Console.WriteLine("Pass: " + citizen.Password);   
                }
                Console.WriteLine("--------------------------------------------------");
                Debug.WriteLine("=== TÀI KHOẢN ADMIN ===");
                Console.WriteLine("ID: admin001 ; PASS: 123");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi hệ thống khi đọc Excel: " + ex.Message);
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

                    MyList<Citizen> list = tree.GetAllCitizens();

                    int row = 2;
                    for (int i = 0; i < list.Count; i++)
                    {
                        Citizen c = list[i];
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