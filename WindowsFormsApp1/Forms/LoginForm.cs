using System;
using System.Windows.Forms;
using WindowsFormsApp1.Database;

namespace WindowsFormsApp1.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            // Xử lý khi đóng form - chỉ thoát app nếu không còn form nào khác
            this.FormClosing += LoginForm_FormClosing;
        }
        
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đang có form khác mở (StudentMainForm hoặc InstructorMainForm), 
            // thì không cho phép đóng LoginForm (chỉ ẩn đi)
            if (Application.OpenForms.Count > 1)
            {
                e.Cancel = true;
                this.Hide();
            }
            // Nếu chỉ còn LoginForm, cho phép đóng và thoát app
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var db = DatabaseHelper.Instance;

            // Try login as student
            var student = db.LoginStudent(username, password);
            if (student != null)
            {
                var studentForm = new StudentMainForm(student);
                this.Hide();
                studentForm.Show(); // Dùng Show() thay vì ShowDialog() để có thể quay lại
                return;
            }

            // Try login as instructor
            var instructor = db.LoginInstructor(username, password);
            if (instructor != null)
            {
                var instructorForm = new InstructorMainForm(instructor);
                this.Hide();
                instructorForm.Show(); // Dùng Show() thay vì ShowDialog() để có thể quay lại
                return;
            }

            MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            // Test connection khi click vào Info
            var db = DatabaseHelper.Instance;
            string message;
            bool success = db.TestConnection(out message);
            
            string info = "=== THÔNG TIN HỆ THỐNG ===\n\n";
            info += message + "\n\n";
            info += "=== HƯỚNG DẪN ĐĂNG NHẬP ===\n";
            info += "Sinh viên: Nhập MaSV và mật khẩu từ database\n";
            info += "Giảng viên: Nhập MaGV và mật khẩu từ database\n";
            info += "Mật khẩu được lưu trong cột Password\n\n";
            info += "Phiên bản: 1.0";
            
            MessageBoxIcon icon = success ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
            MessageBox.Show(info, success ? "Kết nối thành công" : "Cảnh báo kết nối", MessageBoxButtons.OK, icon);
        }
    }
}



