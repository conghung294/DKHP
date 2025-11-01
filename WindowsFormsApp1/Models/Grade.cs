namespace WindowsFormsApp1.Models
{
    public class Grade
    {
        public string MaSV { get; set; }
        public string MaLHP { get; set; }
        public double? DiemChuyenCan { get; set; }
        public double? DiemGiuaKy { get; set; }
        public double? DiemThi { get; set; }
        public double? DiemTongKet { get; set; }
        
        // Thuộc tính tính toán
        public string TenSV { get; set; }
        public string TenLop { get; set; }
        public string TenHocPhan { get; set; }
    }
}



