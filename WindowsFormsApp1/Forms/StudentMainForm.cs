using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class StudentMainForm : Form
    {
        private Student currentStudent;
        private DatabaseHelper db;

        public StudentMainForm(Student student)
        {
            InitializeComponent();
            currentStudent = student;
            db = DatabaseHelper.Instance;
            InitializeForm();
            
            // Xử lý khi đóng form bằng nút X
            this.FormClosing += StudentMainForm_FormClosing;
        }
        
        private void StudentMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đóng bằng nút X, quay về LoginForm
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Tìm LoginForm cũ và hiển thị lại
                foreach (Form form in Application.OpenForms)
                {
                    if (form is LoginForm)
                    {
                        form.Show();
                        break;
                    }
                }
            }
        }

        private void InitializeForm()
        {
            lblWelcome.Text = $"Xin chào, {currentStudent.TenSV} - {currentStudent.MaSV}";
            LoadStudentInfo();
            LoadRegisteredCourses();
            LoadAvailableCourses();
        }

        private void LoadStudentInfo()
        {
            // Load thông tin chương trình đào tạo từ database
            string tenCTDT = db.GetProgramName(currentStudent.MaCTDT);
            
            txtStudentInfo.Text = $"Mã sinh viên: {currentStudent.MaSV}\r\n" +
                                  $"Họ tên: {currentStudent.TenSV}\r\n" +
                                  $"Chương trình: {tenCTDT}\r\n" +
                                  $"Ngày sinh: {currentStudent.NgaySinh:dd/MM/yyyy}\r\n" +
                                  $"Giới tính: {currentStudent.GioiTinh}\r\n" +
                                  $"Địa chỉ: {currentStudent.DiaChi}\r\n" +
                                  $"Email: {currentStudent.Email}\r\n" +
                                  $"SĐT: {currentStudent.SDT}";
        }

        private void LoadRegisteredCourses()
        {
            dgvRegisteredCourses.Rows.Clear();
            var registeredSections = db.GetRegisteredSections(currentStudent.MaSV);
            
            foreach (var section in registeredSections)
            {
                dgvRegisteredCourses.Rows.Add(
                    section.MaLHP,
                    section.TenHocPhan,
                    section.TenLop,
                    section.LichHoc,
                    "", // LoaiMonHoc không có trong schema mới
                    $"{section.SoLuongDangKy}/{section.SiSo}"
                );
            }
        }

        private void LoadAvailableCourses()
        {
            dgvAvailableCourses.Rows.Clear();
            int currentSemester = db.GetCurrentSemester();
            var courses = db.GetCoursesBySemester(currentSemester);
            
            foreach (var course in courses)
            {
                var sections = db.GetAvailableCourseSections(course.MaMH);
                if (sections.Any())
                {
                    foreach (var section in sections)
                    {
                        dgvAvailableCourses.Rows.Add(
                            section.MaLHP,
                            course.MaMH,
                            course.TenHocPhan,
                            course.SoTC,
                            course.TenHocPhanTienQuyet ?? "Không có",
                            section.TenLop,
                            section.LichHoc,
                            section.TenGiangVien,
                            $"{section.SoLuongDangKy}/{section.SiSo}"
                        );
                    }
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (dgvAvailableCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp học phần cần đăng ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLHP = dgvAvailableCourses.SelectedRows[0].Cells[0].Value.ToString();
            
            if (db.RegisterCourseSection(currentStudent.MaSV, maLHP))
            {
                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegisteredCourses();
                LoadAvailableCourses();
            }
            else
            {
                MessageBox.Show("Đăng ký không thành công! Lớp đã đầy hoặc bạn đã đăng ký lớp này rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvRegisteredCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp học phần cần hủy đăng ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLHP = dgvRegisteredCourses.SelectedRows[0].Cells[0].Value.ToString();
            
            if (db.CancelRegistration(currentStudent.MaSV, maLHP))
            {
                MessageBox.Show("Hủy đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegisteredCourses();
                LoadAvailableCourses();
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            var registeredSections = db.GetRegisteredSections(currentStudent.MaSV);
            if (!registeredSections.Any())
            {
                MessageBox.Show("Bạn chưa đăng ký lớp học phần nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string schedule = "=== LỊCH HỌC CỦA BẠN ===\r\n\r\n";
            foreach (var section in registeredSections)
            {
                schedule += $"{section.TenHocPhan} - Lớp {section.TenLop}\r\n";
                schedule += $"Lịch học: {section.LichHoc}\r\n";
                schedule += $"Giảng viên: {section.TenGiangVien}\r\n\r\n";
            }

            MessageBox.Show(schedule, "Lịch học", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Tìm LoginForm cũ và hiển thị lại
            foreach (Form form in Application.OpenForms)
            {
                if (form is LoginForm)
                {
                    form.Show();
                    break;
                }
            }
            this.Close();
        }
    }
}


