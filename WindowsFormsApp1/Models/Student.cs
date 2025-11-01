using System;

namespace WindowsFormsApp1.Models
{
    public class Student
    {
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string MaCTDT { get; set; }
        
        // Thuộc tính tính toán (không có trong DB)
        public string Password { get; set; } // Tạm thời dùng cho authentication
        
        // Thuộc tính tính toán
        public string TenCTDT { get; set; }
    }
}


