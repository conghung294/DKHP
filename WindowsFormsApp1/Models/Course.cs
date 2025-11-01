namespace WindowsFormsApp1.Models
{
    public class Course
    {
        public string MaMH { get; set; }
        public string TenHocPhan { get; set; }
        public int SoTC { get; set; }
        public string MaHocPhanTienQuyet { get; set; }
        public int MaHocKi { get; set; }
        
        // Thuộc tính tính toán
        public string TenHocPhanTienQuyet { get; set; }
        public string TenHocKi { get; set; }
        public string NamHoc { get; set; }
    }
}



