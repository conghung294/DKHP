namespace WindowsFormsApp1.Models
{
    public class CourseSection
    {
        public string MaLHP { get; set; }
        public string TenLop { get; set; }
        public string MaHP { get; set; }
        public string MaGV { get; set; }
        public int SiSo { get; set; }
        public string LichHoc { get; set; }
        
        // Thuộc tính tính toán (tính từ DangKi)
        public int SoLuongDangKy { get; set; }
        
        // Thuộc tính tính toán
        public string TenHocPhan { get; set; }
        public string TenGiangVien { get; set; }
        public int SoChoTrong => SiSo - SoLuongDangKy;
        public bool DaDaySinhVien => SoLuongDangKy >= SiSo;
    }
}



