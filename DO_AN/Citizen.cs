using System;
using System.Collections.Generic;

namespace DO_AN
{
    [Serializable]
    public class Citizen
    {
        // =========================================================
        // 1. THÔNG TIN ĐỊNH DANH 
        // =========================================================
        public string CitizenID { get; set; }        // Số CCCD (Khóa chính)
        public string FullName { get; set; }         // Họ và tên
        public DateTime DateOfBirth { get; set; }    // Ngày sinh
        public string Gender { get; set; }           // Giới tính (Nam/Nữ)
        public string Nationality { get; set; }      // Quốc tịch

        // =========================================================
        // 2. THÔNG TIN LIÊN LẠC & TÀI KHOẢN
        // =========================================================
        public string Password { get; set; }         // Mật khẩu
        public string Address { get; set; }          // Địa chỉ/Quê quán
        public string PhoneNumber { get; set; }      // Số điện thoại

        // =========================================================
        // 3. THÔNG TIN XÃ HỘI 
        // =========================================================
        public string FatherID { get; set; }         // ID Cha
        public string MotherID { get; set; }         // ID Mẹ
        public string SpouseID { get; set; }         // ID Vợ/Chồng
        public string Occupation { get; set; }       // Nghề nghiệp

        // =========================================================
        // 4. HÀM KHỞI TẠO
        // =========================================================
        public Citizen()
        {
            CitizenID = "";
            FullName = "";
            Gender = "Nam";
            Nationality = "Việt Nam";

            Password = "";
            PhoneNumber = "";
            Address = "";

            Occupation = "";
            DateOfBirth = DateTime.Now;
            FatherID = "N/A";
            MotherID = "N/A";
            SpouseID = "N/A";

        }
        // =========================================================
        // 5.PHƯƠNG THỨC GHI ĐÈ
        // =========================================================
        public override string ToString()
        {
            return $"{CitizenID} - {FullName}";
        }

    }
}