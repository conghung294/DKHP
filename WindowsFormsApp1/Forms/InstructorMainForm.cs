using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public partial class InstructorMainForm : Form
    {
        private Instructor currentInstructor;
        private DatabaseHelper db;
        
        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label lblHeaderTitle;
        private Button btnHome;
        private Button btnMyCourses;
        private Label lblUserInfo;

        public InstructorMainForm(Instructor instructor)
        {
            InitializeComponent();
            currentInstructor = instructor;
            db = DatabaseHelper.Instance;
            
            // Xử lý khi đóng form bằng nút X
            this.FormClosing += InstructorMainForm_FormClosing;
            
            // Tạo layout mới
            CreateModernLayout();
            InitializeForm();
            ApplyTheme();
        }
        
        private void CreateModernLayout()
        {
            // Form settings - Cho phép resize form
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.BackColor = ThemeHelper.BackgroundLight;
            
            // Header Bar
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = ThemeHelper.HeaderBlue;
            headerPanel.Padding = new Padding(0);
            
            lblHeaderTitle = new Label();
            lblHeaderTitle.Text = "HỆ THỐNG ĐĂNG KÝ HỌC TÍN CHỈ";
            lblHeaderTitle.Font = ThemeHelper.HeaderFont;
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Location = new Point(20, 0);
            lblHeaderTitle.Height = 60;
            lblHeaderTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblHeaderTitle.Padding = new Padding(0);
            
            headerPanel.Controls.Add(lblHeaderTitle);
            
            // Sidebar
            sidebarPanel = new Panel();
            sidebarPanel.Width = 260;
            sidebarPanel.BackColor = ThemeHelper.SidebarBackground;
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.AutoScroll = false;
            sidebarPanel.BorderStyle = BorderStyle.None;
            sidebarPanel.Padding = new Padding(15, 20, 15, 0);
            
            // Sidebar content - User info panel
            Panel userInfoPanel = new Panel();
            userInfoPanel.BackColor = Color.Transparent;
            userInfoPanel.Location = new Point(0, 20);
            userInfoPanel.Size = new Size(230, 100);
            userInfoPanel.Padding = new Padding(5);
            
            lblUserInfo = new Label();
            lblUserInfo.Text = $"Xin chào,\r\n{currentInstructor.TenGV}\r\n{currentInstructor.MaGV}";
            lblUserInfo.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblUserInfo.ForeColor = ThemeHelper.PrimaryBlueDark;
            lblUserInfo.AutoSize = false;
            lblUserInfo.Size = new Size(220, 95);
            lblUserInfo.Location = new Point(5, 5);
            lblUserInfo.TextAlign = ContentAlignment.TopLeft;
            lblUserInfo.Padding = new Padding(0);
            
            userInfoPanel.Controls.Add(lblUserInfo);
            
            btnHome = new Button();
            btnHome.Text = "🏠 Trang chủ";
            btnHome.Size = new Size(230, 45);
            btnHome.Location = new Point(0, 130);
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.Padding = new Padding(15, 0, 0, 0);
            btnHome.Margin = new Padding(0, 10, 0, 0);
            ThemeHelper.ApplyButtonStyle(btnHome, ThemeHelper.SidebarBackground, ThemeHelper.TextDark);
            
            btnMyCourses = new Button();
            btnMyCourses.Text = "📚 Lớp của tôi";
            btnMyCourses.Size = new Size(230, 45);
            btnMyCourses.Location = new Point(0, 185);
            btnMyCourses.TextAlign = ContentAlignment.MiddleLeft;
            btnMyCourses.Padding = new Padding(15, 0, 0, 0);
            btnMyCourses.Margin = new Padding(0, 10, 0, 0);
            ThemeHelper.ApplyButtonStyle(btnMyCourses, ThemeHelper.SidebarActive, Color.White);
            
            // Logout button in sidebar
            Panel logoutPanel = new Panel();
            logoutPanel.Dock = DockStyle.Bottom;
            logoutPanel.Height = 60;
            logoutPanel.BackColor = ThemeHelper.SidebarBackground;
            
            Button btnSidebarLogout = new Button();
            btnSidebarLogout.Text = "Đăng xuất";
            btnSidebarLogout.Size = new Size(210, 40);
            btnSidebarLogout.Location = new Point(10, 10);
            ThemeHelper.ApplyButtonStyle(btnSidebarLogout, Color.FromArgb(50, 50, 50), Color.White);
            btnSidebarLogout.Click += btnLogout_Click;
            
            logoutPanel.Controls.Add(btnSidebarLogout);
            
            sidebarPanel.Controls.AddRange(new Control[] {
                userInfoPanel, btnHome, btnMyCourses
            });
            sidebarPanel.Controls.Add(logoutPanel);
            logoutPanel.BringToFront();
            
            // Content Panel
            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = ThemeHelper.BackgroundWhite;
            contentPanel.Padding = new Padding(25, 25, 25, 25);
            contentPanel.BorderStyle = BorderStyle.None;
            
            // Di chuyển controls vào contentPanel
            lblWelcome.Parent = contentPanel;
            txtInstructorInfo.Parent = contentPanel;
            dgvMyCourses.Parent = contentPanel;
            btnViewStudents.Parent = contentPanel;
            btnEnterGrades.Parent = contentPanel;
            btnSchedule.Parent = contentPanel;
            btnLogout.Parent = contentPanel;
            
            // Điều chỉnh vị trí (đã có padding)
            lblWelcome.Location = new Point(0, 0);
            txtInstructorInfo.Location = new Point(0, 40);
            txtInstructorInfo.Size = new Size(400, 120);
            
            // DataGridView - đảm bảo hiển thị rõ ràng
            dgvMyCourses.Location = new Point(0, 170);
            dgvMyCourses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMyCourses.Size = new Size(contentPanel.Width, contentPanel.Height - 250);
            dgvMyCourses.BringToFront();
            dgvMyCourses.Visible = true;
            
            // Buttons ở dưới cùng
            btnViewStudents.Location = new Point(0, contentPanel.Height - 60);
            btnEnterGrades.Location = new Point(180, contentPanel.Height - 60);
            btnSchedule.Location = new Point(360, contentPanel.Height - 60);
            btnViewStudents.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEnterGrades.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSchedule.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.Visible = false; // Ẩn button logout cũ
            
            // Thêm panels vào form - thứ tự quan trọng!
            this.Controls.Add(contentPanel); // Thêm contentPanel trước
            this.Controls.Add(sidebarPanel);
            this.Controls.Add(headerPanel);
            
            // Đảm bảo thứ tự z-order đúng
            contentPanel.SendToBack(); // ContentPanel ở sau
            sidebarPanel.BringToFront(); // Sidebar ở giữa
            headerPanel.BringToFront(); // Header ở trên cùng
            
            // Xử lý resize để update kích thước DataGridView
            this.Resize += InstructorMainForm_Resize;
        }
        
        private void InstructorMainForm_Resize(object sender, EventArgs e)
        {
            if (contentPanel != null && dgvMyCourses != null)
            {
                // Update DataGridView size khi form resize
                dgvMyCourses.Size = new Size(contentPanel.Width, contentPanel.Height - 250);
                
                // Update button positions
                if (btnViewStudents != null && contentPanel.Height > 60)
                {
                    btnViewStudents.Location = new Point(0, contentPanel.Height - 60);
                    btnEnterGrades.Location = new Point(180, contentPanel.Height - 60);
                    btnSchedule.Location = new Point(360, contentPanel.Height - 60);
                }
            }
        }
        
        private void ApplyTheme()
        {
            // Style cho labels
            lblWelcome.Font = ThemeHelper.SubHeaderFont;
            lblWelcome.ForeColor = ThemeHelper.TextDark;
            
            // Style cho textbox
            txtInstructorInfo.BackColor = ThemeHelper.BackgroundWhite;
            txtInstructorInfo.ForeColor = ThemeHelper.TextDark;
            txtInstructorInfo.Font = ThemeHelper.NormalFont;
            txtInstructorInfo.BorderStyle = BorderStyle.FixedSingle;
            
            // Style cho DataGridView
            ThemeHelper.ApplyDataGridViewStyle(dgvMyCourses);
            
            // Style cho buttons
            ThemeHelper.ApplyButtonStyle(btnViewStudents, ThemeHelper.PrimaryBlue, Color.White);
            ThemeHelper.ApplyButtonStyle(btnEnterGrades, ThemeHelper.PrimaryBlue, Color.White);
            ThemeHelper.ApplyButtonStyle(btnSchedule, ThemeHelper.PrimaryBlue, Color.White);
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


