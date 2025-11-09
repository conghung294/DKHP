using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ApplyTheme();
            // Xử lý khi đóng form - chỉ thoát app nếu không còn form nào khác
            this.FormClosing += LoginForm_FormClosing;
        }
        
        private void ApplyTheme()
        {
            // Style cho form
            this.BackColor = ThemeHelper.BackgroundLight;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            // Style cho title
            lblTitle.Font = ThemeHelper.TitleFont;
            lblTitle.ForeColor = ThemeHelper.PrimaryBlueDark;
            
            // Style cho labels
            lblUsername.Font = ThemeHelper.LabelFont;
            lblUsername.ForeColor = ThemeHelper.TextDark;
            lblPassword.Font = ThemeHelper.LabelFont;
            lblPassword.ForeColor = ThemeHelper.TextDark;
            
            // Style cho textboxes
            txtUsername.BackColor = ThemeHelper.BackgroundWhite;
            txtUsername.ForeColor = ThemeHelper.TextDark;
            txtUsername.Font = ThemeHelper.NormalFont;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            
            txtPassword.BackColor = ThemeHelper.BackgroundWhite;
            txtPassword.ForeColor = ThemeHelper.TextDark;
            txtPassword.Font = ThemeHelper.NormalFont;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            
            // Style cho button
            ThemeHelper.ApplyButtonStyle(btnLogin, ThemeHelper.PrimaryBlue, Color.White, 8);
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

            // Thử login Admin trước
            if (db.LoginAdmin(username, password, out string adminName))
            {
                var adminForm = new AdminMainForm(adminName);
                this.Hide();
                adminForm.Show();
                return;
            }

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
    }
}



