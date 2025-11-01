using System;

namespace WindowsFormsApp1.Models
{
    public class Instructor
    {
        public string MaGV { get; set; }
        public string TenGV { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string HocVi { get; set; }
        public string MaKV { get; set; }
        
        // Thuộc tính tính toán (không có trong DB)
        public string Password { get; set; } // Tạm thời dùng cho authentication
        
        // Thuộc tính tính toán
        public string TenKhoa { get; set; }
    }
}



