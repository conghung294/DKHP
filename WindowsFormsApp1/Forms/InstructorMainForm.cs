using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class InstructorMainForm : Form
    {
        private Instructor currentInstructor;
        private DatabaseHelper db;

        public InstructorMainForm(Instructor instructor)
        {
            InitializeComponent();
            currentInstructor = instructor;
            db = DatabaseHelper.Instance;
            InitializeForm();
            
            // Xử lý khi đóng form bằng nút X
            this.FormClosing += InstructorMainForm_FormClosing;
        }
        
        private void InstructorMainForm_FormClosing(object sender, FormClosingEventArgs e)
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
            lblWelcome.Text = $"Xin chào, {currentInstructor.TenGV} - {currentInstructor.MaGV}";
            LoadInstructorInfo();
            LoadMyCourses();
        }

        private void LoadInstructorInfo()
        {
            // Load thông tin khoa từ database
            string tenKhoa = db.GetDepartmentName(currentInstructor.MaKV);
            
            txtInstructorInfo.Text = $"Mã giảng viên: {currentInstructor.MaGV}\r\n" +
                                     $"Họ tên: {currentInstructor.TenGV}\r\n" +
                                     $"Khoa: {tenKhoa}\r\n" +
                                     $"Học vị: {currentInstructor.HocVi}\r\n" +
                                     $"Giới tính: {currentInstructor.GioiTinh}\r\n" +
                                     $"Địa chỉ: {currentInstructor.DiaChi}\r\n" +
                                     $"Email: {currentInstructor.Email}\r\n" +
                                     $"SĐT: {currentInstructor.SDT}";
        }

        private void LoadMyCourses()
        {
            dgvMyCourses.Rows.Clear();
            var sections = db.GetInstructorSections(currentInstructor.MaGV);

            foreach (var section in sections)
            {
                dgvMyCourses.Rows.Add(
                    section.MaLHP,
                    section.TenHocPhan,
                    section.TenLop,
                    section.LichHoc,
                    "", // LoaiMonHoc không có trong schema mới
                    $"{section.SoLuongDangKy}/{section.SiSo}"
                );
            }
        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            if (dgvMyCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp học phần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLHP = dgvMyCourses.SelectedRows[0].Cells[0].Value.ToString();
            var students = db.GetStudentsInSection(maLHP);

            if (!students.Any())
            {
                MessageBox.Show("Lớp này chưa có sinh viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form studentsForm = new StudentsListForm(maLHP, students);
            studentsForm.ShowDialog();
        }

        private void btnEnterGrades_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng nhập điểm tạm thời chưa khả dụng vì database chưa có bảng Diem.\n\nCần tạo bảng Diem trong database trước.", 
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // TODO: Uncomment khi có bảng Diem
            /*
            if (dgvMyCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp học phần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLHP = dgvMyCourses.SelectedRows[0].Cells[0].Value.ToString();
            var students = db.GetStudentsInSection(maLHP);

            if (!students.Any())
            {
                MessageBox.Show("Lớp này chưa có sinh viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form gradesForm = new EnterGradesForm(maLHP, students);
            if (gradesForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Nhập điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            var sections = db.GetInstructorSections(currentInstructor.MaGV);
            if (!sections.Any())
            {
                MessageBox.Show("Bạn chưa có lịch dạy nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string schedule = "=== LỊCH DẠY CỦA BẠN ===\r\n\r\n";
            foreach (var section in sections)
            {
                schedule += $"{section.TenHocPhan} - Lớp {section.TenLop}\r\n";
                schedule += $"Lịch học: {section.LichHoc}\r\n\r\n";
            }

            MessageBox.Show(schedule, "Lịch dạy", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


