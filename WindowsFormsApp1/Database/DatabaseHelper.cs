using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Database
{
    public class DatabaseHelper
    {
        private static DatabaseHelper _instance;
        private string connectionString;
        
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseHelper();
                return _instance;
            }
        }

        private DatabaseHelper()
        {
            // Lấy connection string từ App.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback nếu không có trong config
                connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tinchi;Integrated Security=True;";
            }

            // Kiểm tra kết nối
            EnsureDatabaseExists();
        }

        private void EnsureDatabaseExists()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Kết nối thành công
                }
            }
            catch (SqlException ex)
            {
                string errorMsg = $"❌ KHÔNG THỂ KẾT NỐI DATABASE!\n\n";
                errorMsg += $"Lỗi: {ex.Message}\n\n";
                errorMsg += $"Kiểm tra:\n";
                errorMsg += $"1. SQL Server có đang chạy không?\n";
                errorMsg += $"2. Database 'tinchi' đã được tạo chưa?\n";
                errorMsg += $"3. Connection string có đúng không?\n";
                errorMsg += $"4. Windows Authentication có quyền truy cập không?\n\n";
                errorMsg += $"Connection String hiện tại:\n{connectionString}";
                
                MessageBox.Show(errorMsg, "Lỗi Kết nối Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method để test kết nối database (public để có thể gọi từ bên ngoài)
        public bool TestConnection(out string message)
        {
            message = "";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Kiểm tra database có tồn tại không
                    string checkDbSql = "SELECT COUNT(*) FROM sys.databases WHERE name = 'tinchi'";
                    using (var cmd = new SqlCommand(checkDbSql, connection))
                    {
                        // Cần kết nối đến master database để check
                        connection.ChangeDatabase("master");
                        int dbExists = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        if (dbExists == 0)
                        {
                            message = "❌ Database 'tinchi' chưa được tạo!\n\nVui lòng tạo database trong SSMS trước.";
                            return false;
                        }
                    }
                    
                    // Kiểm tra các bảng cơ bản
                    connection.ChangeDatabase("tinchi");
                    string checkTablesSql = @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_TYPE = 'BASE TABLE'";
                    using (var cmd = new SqlCommand(checkTablesSql, connection))
                    {
                        int tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        if (tableCount == 0)
                        {
                            message = "⚠️ Database 'tinchi' đã tồn tại nhưng chưa có bảng nào!\n\nVui lòng chạy script tạo bảng.";
                            return false;
                        }
                        
                        message = $"✅ KẾT NỐI THÀNH CÔNG!\n\n";
                        message += $"Database: tinchi\n";
                        message += $"Số bảng: {tableCount}\n";
                        message += $"Server: {connection.DataSource}\n";
                        message += $"Connection String: {connectionString}";
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                message = $"❌ LỖI KẾT NỐI SQL SERVER!\n\n";
                message += $"Mã lỗi: {ex.Number}\n";
                message += $"Lỗi: {ex.Message}\n\n";
                
                // Xử lý các lỗi phổ biến
                switch (ex.Number)
                {
                    case 2:
                        message += "🔧 Khắc phục:\n";
                        message += "- Kiểm tra SQL Server có đang chạy không\n";
                        message += "- Kiểm tra tên server có đúng không (localhost\\SQLEXPRESS)\n";
                        message += "- Thử đổi về (localdb)\\MSSQLLocalDB nếu dùng LocalDB";
                        break;
                    case 4060:
                        message += "🔧 Khắc phục:\n";
                        message += "- Database 'tinchi' chưa được tạo\n";
                        message += "- Tạo database trong SSMS";
                        break;
                    case 18456:
                        message += "🔧 Khắc phục:\n";
                        message += "- Lỗi đăng nhập (Login failed)\n";
                        message += "- Kiểm tra Windows Authentication\n";
                        message += "- Đảm bảo user hiện tại có quyền truy cập SQL Server";
                        break;
                    default:
                        message += "🔧 Kiểm tra:\n";
                        message += "- SQL Server Service có đang chạy\n";
                        message += "- Connection string có đúng\n";
                        message += "- Database đã được tạo";
                        break;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                message = $"❌ LỖI KHÔNG XÁC ĐỊNH!\n\n{ex.Message}";
                return false;
            }
        }

        // Authentication methods - Sử dụng Password từ database
        public Student LoginStudent(string maSV, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM SinhVien WHERE MaSV = @MaSV AND Password = @Password";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaSV", maSV);
                        command.Parameters.AddWithValue("@Password", password);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Student
                                {
                                    MaSV = reader["MaSV"].ToString(),
                                    TenSV = reader["TenSV"].ToString(),
                                    NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                    GioiTinh = reader["GioiTinh"]?.ToString(),
                                    SDT = reader["SDT"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    DiaChi = reader["DiaChi"]?.ToString(),
                                    MaCTDT = reader["MaCTDT"].ToString(),
                                    Password = reader["Password"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public Instructor LoginInstructor(string maGV, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM GiangVien WHERE MaGV = @MaGV AND Password = @Password";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaGV", maGV);
                        command.Parameters.AddWithValue("@Password", password);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Instructor
                                {
                                    MaGV = reader["MaGV"].ToString(),
                                    TenGV = reader["TenGV"].ToString(),
                                    GioiTinh = reader["GioiTinh"]?.ToString(),
                                    DiaChi = reader["DiaChi"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    SDT = reader["SDT"]?.ToString(),
                                    HocVi = reader["HocVi"]?.ToString(),
                                    MaKV = reader["MaKV"].ToString(),
                                    Password = reader["Password"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        // Admin login: ưu tiên bảng Admin nếu có, fallback admin/admin
        public bool LoginAdmin(string username, string password, out string adminName)
        {
            adminName = "";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Thử truy vấn bảng Admin nếu tồn tại
                    string checkTable = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Admin'";
                    using (var cmdCheck = new SqlCommand(checkTable, connection))
                    {
                        int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (exists > 0)
                        {
                            string sql = "SELECT TOP 1 HoTen FROM Admin WHERE MaAdmin = @U OR TenDangNhap = @U AND Password = @P";
                            using (var cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@U", username);
                                cmd.Parameters.AddWithValue("@P", password);
                                var result = cmd.ExecuteScalar();
                                if (result != null)
                                {
                                    adminName = result.ToString();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            // Fallback tài khoản mặc định nếu không có bảng Admin
            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase) && password == "admin")
            {
                adminName = "Quản trị viên";
                return true;
            }
            return false;
        }

        // Get courses by semester
        public List<Course> GetCoursesBySemester(int maHocKi)
        {
            var courses = new List<Course>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT m.MaMH, m.TenHocPhan, m.SoTC, m.MaHocPhanTienQuyet, m.MaHocKi,
                               mq.TenHocPhan as TenHocPhanTienQuyet, h.TenHocKi, h.NamHoc
                        FROM MonHoc m
                        LEFT JOIN MonHoc mq ON m.MaHocPhanTienQuyet = mq.MaMH
                        JOIN HocKi h ON m.MaHocKi = h.MaHocKi
                        WHERE m.MaHocKi = @MaHK";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaHK", maHocKi);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courses.Add(new Course
                                {
                                    MaMH = reader["MaMH"].ToString(),
                                    TenHocPhan = reader["TenHocPhan"].ToString(),
                                    SoTC = Convert.ToInt32(reader["SoTC"]),
                                    MaHocPhanTienQuyet = reader["MaHocPhanTienQuyet"]?.ToString(),
                                    MaHocKi = Convert.ToInt32(reader["MaHocKi"]),
                                    TenHocPhanTienQuyet = reader["TenHocPhanTienQuyet"]?.ToString(),
                                    TenHocKi = reader["TenHocKi"].ToString(),
                                    NamHoc = reader["NamHoc"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return courses;
        }

        // ===== ADMIN: COURSES =====
        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT MaMH, TenHocPhan, SoTC, MaHocPhanTienQuyet, MaHocKi FROM MonHoc";
                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                MaMH = reader["MaMH"].ToString(),
                                TenHocPhan = reader["TenHocPhan"].ToString(),
                                SoTC = Convert.ToInt32(reader["SoTC"]),
                                MaHocPhanTienQuyet = reader["MaHocPhanTienQuyet"]?.ToString(),
                                MaHocKi = Convert.ToInt32(reader["MaHocKi"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải Môn học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return courses;
        }

        public bool InsertCourse(Course c)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO MonHoc (MaMH, TenHocPhan, SoTC, MaHocPhanTienQuyet, MaHocKi)
                                   VALUES (@MaMH, @TenHocPhan, @SoTC, @MaTQ, @MaHK)";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", c.MaMH);
                        cmd.Parameters.AddWithValue("@TenHocPhan", c.TenHocPhan);
                        cmd.Parameters.AddWithValue("@SoTC", c.SoTC);
                        cmd.Parameters.AddWithValue("@MaTQ", (object)c.MaHocPhanTienQuyet ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaHK", c.MaHocKi);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm môn học: {ex.Message}");
                return false;
            }
        }

        public bool UpdateCourse(Course c)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE MonHoc SET TenHocPhan=@TenHocPhan, SoTC=@SoTC, MaHocPhanTienQuyet=@MaTQ, MaHocKi=@MaHK WHERE MaMH=@MaMH";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", c.MaMH);
                        cmd.Parameters.AddWithValue("@TenHocPhan", c.TenHocPhan);
                        cmd.Parameters.AddWithValue("@SoTC", c.SoTC);
                        cmd.Parameters.AddWithValue("@MaTQ", (object)c.MaHocPhanTienQuyet ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaHK", c.MaHocKi);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật môn học: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCourse(string maMH)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM MonHoc WHERE MaMH=@MaMH";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa môn học: {ex.Message}");
                return false;
            }
        }

        // ===== ADMIN: SECTIONS =====
        public List<CourseSection> GetAllSections()
        {
            var list = new List<CourseSection>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"SELECT l.MaLHP, l.TenLop, l.MaHP, l.MaGV, l.SiSo, l.LichHoc,
                                           m.TenHocPhan, g.TenGV,
                                           (SELECT COUNT(*) FROM DangKi d WHERE d.MaLHP = l.MaLHP) as SoLuongDangKy
                                    FROM LopHocPhan l
                                    JOIN MonHoc m ON l.MaHP = m.MaMH
                                    JOIN GiangVien g ON l.MaGV = g.MaGV";
                    using (var cmd = new SqlCommand(sql, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CourseSection
                            {
                                MaLHP = reader["MaLHP"].ToString(),
                                TenLop = reader["TenLop"].ToString(),
                                MaHP = reader["MaHP"].ToString(),
                                MaGV = reader["MaGV"].ToString(),
                                SiSo = Convert.ToInt32(reader["SiSo"]),
                                LichHoc = reader["LichHoc"]?.ToString(),
                                SoLuongDangKy = Convert.ToInt32(reader["SoLuongDangKy"]),
                                TenHocPhan = reader["TenHocPhan"].ToString(),
                                TenGiangVien = reader["TenGV"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải Lớp học phần: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return list;
        }

        public bool InsertSection(CourseSection s)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO LopHocPhan (MaLHP, TenLop, MaHP, MaGV, SiSo, LichHoc)
                                   VALUES (@MaLHP, @TenLop, @MaHP, @MaGV, @SiSo, @LichHoc)";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaLHP", s.MaLHP);
                        cmd.Parameters.AddWithValue("@TenLop", s.TenLop);
                        cmd.Parameters.AddWithValue("@MaHP", s.MaHP);
                        cmd.Parameters.AddWithValue("@MaGV", s.MaGV);
                        cmd.Parameters.AddWithValue("@SiSo", s.SiSo);
                        cmd.Parameters.AddWithValue("@LichHoc", (object)s.LichHoc ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm lớp học phần: {ex.Message}");
                return false;
            }
        }

        public bool UpdateSection(CourseSection s)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE LopHocPhan SET TenLop=@TenLop, MaHP=@MaHP, MaGV=@MaGV, SiSo=@SiSo, LichHoc=@LichHoc WHERE MaLHP=@MaLHP";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaLHP", s.MaLHP);
                        cmd.Parameters.AddWithValue("@TenLop", s.TenLop);
                        cmd.Parameters.AddWithValue("@MaHP", s.MaHP);
                        cmd.Parameters.AddWithValue("@MaGV", s.MaGV);
                        cmd.Parameters.AddWithValue("@SiSo", s.SiSo);
                        cmd.Parameters.AddWithValue("@LichHoc", (object)s.LichHoc ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật lớp học phần: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSection(string maLHP)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM LopHocPhan WHERE MaLHP=@MaLHP";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa lớp học phần: {ex.Message}");
                return false;
            }
        }

        public bool AssignInstructorToSection(string maLHP, string maGV)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE LopHocPhan SET MaGV=@MaGV WHERE MaLHP=@MaLHP";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaGV", maGV);
                        cmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi phân công giảng viên: {ex.Message}");
                return false;
            }
        }

        // ===== ADMIN: DEPARTMENTS =====
        public List<Department> GetAllDepartments()
        {
            var list = new List<Department>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT MaKhoa, TenKhoa FROM Khoa";
                    using (var cmd = new SqlCommand(sql, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Department
                            {
                                MaKhoa = reader["MaKhoa"].ToString(),
                                TenKhoa = reader["TenKhoa"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải Khoa: {ex.Message}");
            }
            return list;
        }

        public bool InsertDepartment(string maKhoa, string tenKhoa)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES (@Ma, @Ten)";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Ma", maKhoa);
                        cmd.Parameters.AddWithValue("@Ten", tenKhoa);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm Khoa: {ex.Message}");
                return false;
            }
        }

        public bool UpdateDepartment(string maKhoa, string tenKhoa)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Khoa SET TenKhoa=@Ten WHERE MaKhoa=@Ma";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Ma", maKhoa);
                        cmd.Parameters.AddWithValue("@Ten", tenKhoa);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật Khoa: {ex.Message}");
                return false;
            }
        }

        public bool DeleteDepartment(string maKhoa)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Khoa WHERE MaKhoa=@Ma";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Ma", maKhoa);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa Khoa: {ex.Message}");
                return false;
            }
        }

        // Get available course sections (tính SoLuongDangKy từ DangKi)
        public List<CourseSection> GetAvailableCourseSections(string maHP)
        {
            var sections = new List<CourseSection>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT l.MaLHP, l.TenLop, l.MaHP, l.MaGV, l.SiSo, l.LichHoc,
                               m.TenHocPhan, g.TenGV,
                               (SELECT COUNT(*) FROM DangKi d WHERE d.MaLHP = l.MaLHP) as SoLuongDangKy
                        FROM LopHocPhan l
                        JOIN MonHoc m ON l.MaHP = m.MaMH
                        JOIN GiangVien g ON l.MaGV = g.MaGV
                        WHERE l.MaHP = @MaHP
                        AND (SELECT COUNT(*) FROM DangKi d WHERE d.MaLHP = l.MaLHP) < l.SiSo";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaHP", maHP);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sections.Add(new CourseSection
                                {
                                    MaLHP = reader["MaLHP"].ToString(),
                                    TenLop = reader["TenLop"].ToString(),
                                    MaHP = reader["MaHP"].ToString(),
                                    MaGV = reader["MaGV"].ToString(),
                                    SiSo = Convert.ToInt32(reader["SiSo"]),
                                    LichHoc = reader["LichHoc"]?.ToString(),
                                    SoLuongDangKy = Convert.ToInt32(reader["SoLuongDangKy"]),
                                    TenHocPhan = reader["TenHocPhan"].ToString(),
                                    TenGiangVien = reader["TenGV"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sections;
        }

        // Get registered sections for a student
        public List<CourseSection> GetRegisteredSections(string maSV)
        {
            var sections = new List<CourseSection>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT l.MaLHP, l.TenLop, l.MaHP, l.MaGV, l.SiSo, l.LichHoc,
                               m.TenHocPhan, m.SoTC, g.TenGV, d.HinhThuc,
                               (SELECT COUNT(*) FROM DangKi d2 WHERE d2.MaLHP = l.MaLHP) as SoLuongDangKy
                        FROM LopHocPhan l
                        JOIN MonHoc m ON l.MaHP = m.MaMH
                        JOIN GiangVien g ON l.MaGV = g.MaGV
                        JOIN DangKi d ON l.MaLHP = d.MaLHP
                        WHERE d.MaSV = @MaSV";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaSV", maSV);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sections.Add(new CourseSection
                                {
                                    MaLHP = reader["MaLHP"].ToString(),
                                    TenLop = reader["TenLop"].ToString(),
                                    MaHP = reader["MaHP"].ToString(),
                                    MaGV = reader["MaGV"].ToString(),
                                    SiSo = Convert.ToInt32(reader["SiSo"]),
                                    LichHoc = reader["LichHoc"]?.ToString(),
                                    SoLuongDangKy = Convert.ToInt32(reader["SoLuongDangKy"]),
                                    TenHocPhan = reader["TenHocPhan"].ToString(),
                                    TenGiangVien = reader["TenGV"].ToString(),
                                    HinhThuc = reader["HinhThuc"]?.ToString() ?? "Kế hoạch",
                                    SoTC = Convert.ToInt32(reader["SoTC"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sections;
        }

        // Register course section
        public bool RegisterCourseSection(string maSV, string maLHP)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if already registered
                    string checkSql = "SELECT COUNT(*) FROM DangKi WHERE MaSV = @MaSV AND MaLHP = @MaLHP";
                    using (var checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        checkCmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0) return false;
                    }

                    // Check if class is full
                    string fullSql = @"
                        SELECT l.SiSo, (SELECT COUNT(*) FROM DangKi d WHERE d.MaLHP = l.MaLHP) as SoLuongDK
                        FROM LopHocPhan l
                        WHERE l.MaLHP = @MaLHP";
                    using (var fullCmd = new SqlCommand(fullSql, connection))
                    {
                        fullCmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        using (var reader = fullCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int siSo = Convert.ToInt32(reader["SiSo"]);
                                int soLuongDK = Convert.ToInt32(reader["SoLuongDK"]);
                                if (soLuongDK >= siSo) return false;
                            }
                        }
                    }

                    // Insert registration (mặc định HinhThuc = 'Kế hoạch')
                    string insertSql = "INSERT INTO DangKi (MaSV, MaLHP, HinhThuc) VALUES (@MaSV, @MaLHP, N'Kế hoạch')";
                    using (var insertCmd = new SqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@MaSV", maSV);
                        insertCmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        insertCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Cancel registration
        public bool CancelRegistration(string maSV, string maLHP)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete registration
                    string deleteSql = "DELETE FROM DangKi WHERE MaSV = @MaSV AND MaLHP = @MaLHP";
                    using (var deleteCmd = new SqlCommand(deleteSql, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@MaSV", maSV);
                        deleteCmd.Parameters.AddWithValue("@MaLHP", maLHP);
                        int rows = deleteCmd.ExecuteNonQuery();
                        if (rows == 0) return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hủy đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Get instructor's course sections
        public List<CourseSection> GetInstructorSections(string maGV)
        {
            var sections = new List<CourseSection>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT l.MaLHP, l.TenLop, l.MaHP, l.MaGV, l.SiSo, l.LichHoc,
                               m.TenHocPhan, g.TenGV,
                               (SELECT COUNT(*) FROM DangKi d WHERE d.MaLHP = l.MaLHP) as SoLuongDangKy
                        FROM LopHocPhan l
                        JOIN MonHoc m ON l.MaHP = m.MaMH
                        JOIN GiangVien g ON l.MaGV = g.MaGV
                        WHERE l.MaGV = @MaGV";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaGV", maGV);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sections.Add(new CourseSection
                                {
                                    MaLHP = reader["MaLHP"].ToString(),
                                    TenLop = reader["TenLop"].ToString(),
                                    MaHP = reader["MaHP"].ToString(),
                                    MaGV = reader["MaGV"].ToString(),
                                    SiSo = Convert.ToInt32(reader["SiSo"]),
                                    LichHoc = reader["LichHoc"]?.ToString(),
                                    SoLuongDangKy = Convert.ToInt32(reader["SoLuongDangKy"]),
                                    TenHocPhan = reader["TenHocPhan"].ToString(),
                                    TenGiangVien = reader["TenGV"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sections;
        }

        // ===== ADMIN QUERIES =====
        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT MaSV, TenSV, NgaySinh, GioiTinh, SDT, Email, DiaChi, MaCTDT FROM SinhVien";
                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                MaSV = reader["MaSV"].ToString(),
                                TenSV = reader["TenSV"].ToString(),
                                NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                GioiTinh = reader["GioiTinh"]?.ToString(),
                                SDT = reader["SDT"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                DiaChi = reader["DiaChi"]?.ToString(),
                                MaCTDT = reader["MaCTDT"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải Sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return students;
        }

        public List<Instructor> GetAllInstructors()
        {
            var instructors = new List<Instructor>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT MaGV, TenGV, GioiTinh, DiaChi, Email, SDT, HocVi, MaKV FROM GiangVien";
                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            instructors.Add(new Instructor
                            {
                                MaGV = reader["MaGV"].ToString(),
                                TenGV = reader["TenGV"].ToString(),
                                GioiTinh = reader["GioiTinh"]?.ToString(),
                                DiaChi = reader["DiaChi"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                SDT = reader["SDT"]?.ToString(),
                                HocVi = reader["HocVi"]?.ToString(),
                                MaKV = reader["MaKV"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải Giảng viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return instructors;
        }

        // ===== CRUD: SinhVien =====
        public bool InsertStudent(Student s)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, SDT, Email, DiaChi, MaCTDT, Password)
                                   VALUES (@MaSV, @TenSV, @NgaySinh, @GioiTinh, @SDT, @Email, @DiaChi, @MaCTDT, @Password)";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", s.MaSV);
                        cmd.Parameters.AddWithValue("@TenSV", s.TenSV);
                        cmd.Parameters.AddWithValue("@NgaySinh", s.NgaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", (object)s.GioiTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)s.SDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)s.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)s.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaCTDT", s.MaCTDT);
                        cmd.Parameters.AddWithValue("@Password", (object)s.Password ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm sinh viên: {ex.Message}");
                return false;
            }
        }

        public bool UpdateStudent(Student s)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE SinhVien SET TenSV=@TenSV, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, SDT=@SDT, Email=@Email, DiaChi=@DiaChi, MaCTDT=@MaCTDT, Password=@Password WHERE MaSV=@MaSV";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", s.MaSV);
                        cmd.Parameters.AddWithValue("@TenSV", s.TenSV);
                        cmd.Parameters.AddWithValue("@NgaySinh", s.NgaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", (object)s.GioiTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)s.SDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)s.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)s.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaCTDT", s.MaCTDT);
                        cmd.Parameters.AddWithValue("@Password", (object)s.Password ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sinh viên: {ex.Message}");
                return false;
            }
        }

        public bool DeleteStudent(string maSV)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM SinhVien WHERE MaSV=@MaSV";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa sinh viên: {ex.Message}");
                return false;
            }
        }

        // ===== CRUD: GiangVien =====
        public bool InsertInstructor(Instructor g)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO GiangVien (MaGV, TenGV, GioiTinh, DiaChi, Email, SDT, HocVi, MaKV, Password)
                                   VALUES (@MaGV, @TenGV, @GioiTinh, @DiaChi, @Email, @SDT, @HocVi, @MaKV, @Password)";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaGV", g.MaGV);
                        cmd.Parameters.AddWithValue("@TenGV", g.TenGV);
                        cmd.Parameters.AddWithValue("@GioiTinh", (object)g.GioiTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)g.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)g.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)g.SDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HocVi", (object)g.HocVi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaKV", g.MaKV);
                        cmd.Parameters.AddWithValue("@Password", (object)g.Password ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm giảng viên: {ex.Message}");
                return false;
            }
        }

        public bool UpdateInstructor(Instructor g)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE GiangVien SET TenGV=@TenGV, GioiTinh=@GioiTinh, DiaChi=@DiaChi, Email=@Email, SDT=@SDT, HocVi=@HocVi, MaKV=@MaKV, Password=@Password WHERE MaGV=@MaGV";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaGV", g.MaGV);
                        cmd.Parameters.AddWithValue("@TenGV", g.TenGV);
                        cmd.Parameters.AddWithValue("@GioiTinh", (object)g.GioiTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)g.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)g.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)g.SDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HocVi", (object)g.HocVi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaKV", g.MaKV);
                        cmd.Parameters.AddWithValue("@Password", (object)g.Password ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật giảng viên: {ex.Message}");
                return false;
            }
        }

        public bool DeleteInstructor(string maGV)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM GiangVien WHERE MaGV=@MaGV";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaGV", maGV);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa giảng viên: {ex.Message}");
                return false;
            }
        }

        // Get students in a course section
        public List<Student> GetStudentsInSection(string maLHP)
        {
            var students = new List<Student>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT DISTINCT s.*
                        FROM SinhVien s
                        JOIN DangKi d ON s.MaSV = d.MaSV
                        WHERE d.MaLHP = @MaLHP";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaLHP", maLHP);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(new Student
                                {
                                    MaSV = reader["MaSV"].ToString(),
                                    TenSV = reader["TenSV"].ToString(),
                                    NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                    GioiTinh = reader["GioiTinh"]?.ToString(),
                                    SDT = reader["SDT"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    DiaChi = reader["DiaChi"]?.ToString(),
                                    MaCTDT = reader["MaCTDT"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return students;
        }


        // Get program name
        public string GetProgramName(string maCTDT)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT TenCTDT FROM CTDT WHERE MaCTDT = @MaCTDT";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaCTDT", maCTDT);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        // Get section info by MaLHP
        public CourseSection GetSectionInfo(string maLHP)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT l.MaLHP, l.TenLop, l.MaHP, l.MaGV, l.SiSo, l.LichHoc,
                               m.TenHocPhan, g.TenGV
                        FROM LopHocPhan l
                        JOIN MonHoc m ON l.MaHP = m.MaMH
                        JOIN GiangVien g ON l.MaGV = g.MaGV
                        WHERE l.MaLHP = @MaLHP";
                    
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaLHP", maLHP);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CourseSection
                                {
                                    MaLHP = reader["MaLHP"].ToString(),
                                    TenLop = reader["TenLop"].ToString(),
                                    MaHP = reader["MaHP"].ToString(),
                                    MaGV = reader["MaGV"].ToString(),
                                    SiSo = Convert.ToInt32(reader["SiSo"]),
                                    LichHoc = reader["LichHoc"]?.ToString(),
                                    TenHocPhan = reader["TenHocPhan"].ToString(),
                                    TenGiangVien = reader["TenGV"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thông tin lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        // Get department name
        public string GetDepartmentName(string maKV)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT TenKhoa FROM Khoa WHERE MaKhoa = @MaKhoa";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaKhoa", maKV);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        // Get current semester (lấy học kỳ mới nhất)
        public int GetCurrentSemester()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT TOP 1 MaHocKi FROM HocKi ORDER BY MaHocKi DESC";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null)
                            return Convert.ToInt32(result);
                    }
                }
            }
            catch { }
            return 1; // Default
        }
    }
}
