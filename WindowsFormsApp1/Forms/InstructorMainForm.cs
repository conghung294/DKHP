using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            
            // Mở form ở chế độ toàn màn hình khi đăng nhập
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            
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
            
            // Thêm logo thay cho dòng "Xin chào"
            try
            {
                var logoPath = ResolveLogoPath();
                if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                {
                    PictureBox picLogo = new PictureBox();
                    picLogo.Size = new Size(64, 64);
                    picLogo.Location = new Point(5, 5);
                    picLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    using (var fs = new FileStream(logoPath, FileMode.Open, FileAccess.Read))
                    {
                        picLogo.Image = Image.FromStream(fs);
                    }
                    userInfoPanel.Controls.Add(picLogo);
                    // Ẩn label "Xin chào"
                    lblUserInfo.Visible = false;
                }
            }
            catch { /* Bỏ qua, fallback về label */ }
            
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
            
            // Content Panel (chứa các controls cũ)
            // QUAN TRỌNG: Không dùng Dock = Fill để tránh bị sidebar và header che
            // Thay vào đó, dùng Dock với điều chỉnh vị trí
            contentPanel = new Panel();
            contentPanel.BackColor = ThemeHelper.BackgroundWhite;
            contentPanel.Padding = new Padding(25, 25, 25, 25);
            contentPanel.BorderStyle = BorderStyle.None;
            // ContentPanel sẽ được đặt vị trí và kích thước sau khi header và sidebar đã được add
            
            // Di chuyển controls vào contentPanel
            lblWelcome.Parent = contentPanel;
            txtInstructorInfo.Parent = contentPanel;
            dgvMyCourses.Parent = contentPanel;
            btnViewStudents.Parent = contentPanel;
            btnLogout.Parent = contentPanel;
            
            // Điều chỉnh vị trí controls trong contentPanel (padding đã được set ở Panel level)
            lblWelcome.Location = new Point(0, 0);
            // Ẩn textbox cũ, thay bằng card đẹp
            txtInstructorInfo.Visible = false;
            
            // Tạo card thông tin giảng viên (UI đẹp hơn thay cho txtInstructorInfo)
            CreateInstructorInfoCard();
            
            // Tạo TabControl giống StudentMainForm
            CreateTabControl();
            
            // Buttons ở dưới cùng
            btnViewStudents.Location = new Point(0, contentPanel.Height - 60);
            btnViewStudents.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.Visible = false; // Ẩn button logout cũ, dùng button trong sidebar
            btnViewStudents.Visible = true;
            
            // Thêm các panels vào form - thứ tự QUAN TRỌNG!
            // Header phải add đầu tiên (Dock = Top)
            this.Controls.Add(headerPanel);
            
            // Sidebar add thứ hai (Dock = Left, sẽ tự động ở dưới header)
            this.Controls.Add(sidebarPanel);
            
            // ContentPanel add cuối cùng và đặt vị trí/kích thước để không bị che
            this.Controls.Add(contentPanel);
            
            // Đặt vị trí và kích thước contentPanel để không bị sidebar và header che
            UpdateContentPanelLayout();
            
            // Đảm bảo thứ tự z-order đúng trong form
            contentPanel.SendToBack(); // ContentPanel ở sau (dưới sidebar và header)
            sidebarPanel.BringToFront(); // Sidebar ở giữa
            headerPanel.BringToFront(); // Header ở trên cùng
            
            // Xử lý resize để update kích thước DataGridView
            this.Resize += InstructorMainForm_Resize;
        }
        
        private void InstructorMainForm_Resize(object sender, EventArgs e)
        {
            // Cập nhật layout của contentPanel khi form resize
            UpdateContentPanelLayout();
            
            if (contentPanel != null && tabControl != null)
            {
                // Update TabControl size khi form resize
                int buttonBarHeight = 60;
                int bottomMargin = 20;
                int tabTop = tabControl.Location.Y;
                int tabControlHeight = Math.Max(200, contentPanel.ClientSize.Height - tabTop - (buttonBarHeight + bottomMargin));
                tabControl.Size = new Size(contentPanel.ClientSize.Width, tabControlHeight);
                
                // Update button positions
                if (btnViewStudents != null && contentPanel.ClientSize.Height > 60)
                {
                    btnViewStudents.Location = new Point(0, contentPanel.ClientSize.Height - 60);
                }
            }
        }
        
        private void UpdateContentPanelLayout()
        {
            if (contentPanel != null && sidebarPanel != null && headerPanel != null)
            {
                // ContentPanel bắt đầu từ bên phải sidebar và dưới header
                int leftMargin = sidebarPanel.Width;
                int topMargin = headerPanel.Height;
                
                contentPanel.Location = new Point(leftMargin, topMargin);
                contentPanel.Size = new Size(
                    this.ClientSize.Width - leftMargin,
                    this.ClientSize.Height - topMargin
                );
                
                // Dùng Anchor để tự động resize khi form resize
                contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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
            // Cập nhật card thông tin giảng viên
            UpdateInstructorInfoCard();
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
        
        // Card thông tin giảng viên
        private Panel instructorInfoCard;
        private TableLayoutPanel tlpInstructorInfo;
        private Label lblValueMaGV;
        private Label lblValueHoTen;
        private Label lblValueKhoa;
        private Label lblValueHocVi;
        private Label lblValueGioiTinh;
        private Label lblValueDiaChi;
        private Label lblValueEmail;
        private Label lblValueSDT;
        
        private void CreateInstructorInfoCard()
        {
            // Tạo card thông tin giảng viên
            instructorInfoCard = new Panel();
            instructorInfoCard.Parent = contentPanel;
            instructorInfoCard.Location = new Point(0, 35);
            instructorInfoCard.Size = new Size(700, 120);
            instructorInfoCard.BackColor = ThemeHelper.BackgroundWhite;
            instructorInfoCard.BorderStyle = BorderStyle.FixedSingle;
            instructorInfoCard.Padding = new Padding(12);
            instructorInfoCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            
            tlpInstructorInfo = new TableLayoutPanel();
            tlpInstructorInfo.Parent = instructorInfoCard;
            tlpInstructorInfo.Dock = DockStyle.Fill;
            // Bố cục 2 cột: (Tiêu đề1, Giá trị1, Tiêu đề2, Giá trị2)
            tlpInstructorInfo.ColumnCount = 4;
            tlpInstructorInfo.RowCount = 4;
            tlpInstructorInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlpInstructorInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInstructorInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlpInstructorInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 4; i++) tlpInstructorInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            
            // Hàm thêm cặp tiêu đề-giá trị vào vị trí (row, groupColumn)
            void AddPair(int row, int groupColumn, string title, out Label valueLabel)
            {
                int colTitle = groupColumn == 0 ? 0 : 2;
                int colValue = colTitle + 1;
                var lblTitle = new Label();
                lblTitle.Text = title;
                lblTitle.AutoSize = true;
                lblTitle.Margin = new Padding(0, 2, 6, 2);
                lblTitle.TextAlign = ContentAlignment.MiddleLeft;
                lblTitle.Font = ThemeHelper.LabelFont;
                lblTitle.ForeColor = ThemeHelper.TextDark;
                
                valueLabel = new Label();
                valueLabel.AutoSize = true;
                valueLabel.Margin = new Padding(0, 2, 0, 2);
                valueLabel.TextAlign = ContentAlignment.MiddleLeft;
                valueLabel.Font = ThemeHelper.NormalFont;
                valueLabel.ForeColor = ThemeHelper.TextDark;
                
                tlpInstructorInfo.Controls.Add(lblTitle, colTitle, row);
                tlpInstructorInfo.Controls.Add(valueLabel, colValue, row);
            }
            
            // Cột trái
            AddPair(0, 0, "Mã giảng viên:", out lblValueMaGV);
            AddPair(1, 0, "Họ tên:", out lblValueHoTen);
            AddPair(2, 0, "Khoa:", out lblValueKhoa);
            AddPair(3, 0, "Học vị:", out lblValueHocVi);
            // Cột phải
            AddPair(0, 1, "Giới tính:", out lblValueGioiTinh);
            AddPair(1, 1, "Địa chỉ:", out lblValueDiaChi);
            AddPair(2, 1, "Email:", out lblValueEmail);
            AddPair(3, 1, "SĐT:", out lblValueSDT);
            
            // Load dữ liệu vào card
            UpdateInstructorInfoCard();
        }
        
        private void UpdateInstructorInfoCard()
        {
            if (instructorInfoCard == null) return;
            
            string tenKhoa = db.GetDepartmentName(currentInstructor.MaKV);
            
            lblValueMaGV.Text = currentInstructor.MaGV;
            lblValueHoTen.Text = currentInstructor.TenGV;
            lblValueKhoa.Text = tenKhoa;
            lblValueHocVi.Text = currentInstructor.HocVi ?? "";
            lblValueGioiTinh.Text = currentInstructor.GioiTinh ?? "";
            lblValueDiaChi.Text = currentInstructor.DiaChi ?? "";
            lblValueEmail.Text = currentInstructor.Email ?? "";
            lblValueSDT.Text = currentInstructor.SDT ?? "";
        }
        
        private TabControl tabControl;
        private TabPage tabMyCourses;
        private Label lblMyCoursesTitle;
        
        private void CreateTabControl()
        {
            // TabControl - đảm bảo hiển thị rõ ràng và không bị che
            tabControl = new TabControl();
            tabControl.Parent = contentPanel;
            tabControl.Location = new Point(0, 190);
            // Chừa chỗ cho vùng nút ở cuối
            int buttonBarHeight = 60;
            int bottomMargin = 20;
            int availableHeight = Math.Max(200, contentPanel.ClientSize.Height - tabControl.Location.Y - (buttonBarHeight + bottomMargin));
            tabControl.Size = new Size(contentPanel.ClientSize.Width, availableHeight);
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Visible = true;
            tabControl.Enabled = true;
            tabControl.Dock = DockStyle.None;
            
            // Tạo tab "Lớp của tôi"
            tabMyCourses = new TabPage("Lớp của tôi");
            tabMyCourses.UseVisualStyleBackColor = false;
            tabMyCourses.BackColor = ThemeHelper.BackgroundWhite;
            
            // Label tiêu đề trong tab
            lblMyCoursesTitle = new Label();
            lblMyCoursesTitle.Text = "Các lớp đang giảng dạy:";
            lblMyCoursesTitle.Font = ThemeHelper.SubHeaderFont;
            lblMyCoursesTitle.ForeColor = ThemeHelper.TextDark;
            lblMyCoursesTitle.AutoSize = true;
            lblMyCoursesTitle.Location = new Point(10, 10);
            tabMyCourses.Controls.Add(lblMyCoursesTitle);
            
            // DataGridView trong tab
            dgvMyCourses.Location = new Point(10, 50);
            dgvMyCourses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMyCourses.Size = new Size(tabMyCourses.Width - 20, tabMyCourses.Height - 60);
            dgvMyCourses.Visible = true;
            dgvMyCourses.Enabled = true;
            tabMyCourses.Controls.Add(dgvMyCourses);
            
            tabControl.TabPages.Add(tabMyCourses);
            
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.Multiline = false;
            tabControl.SizeMode = TabSizeMode.Normal;
            
            // Xử lý resize TabControl
            tabControl.Resize += TabControl_Resize;
        }
        
        private void TabControl_Resize(object sender, EventArgs e)
        {
            if (tabControl != null && dgvMyCourses != null && tabMyCourses != null)
            {
                // Update DataGridView size khi TabControl resize
                dgvMyCourses.Size = new Size(tabMyCourses.Width - 20, tabMyCourses.Height - 60);
            }
        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            if (dgvMyCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp học phần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLHP = dgvMyCourses.SelectedRows[0].Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(maLHP))
            {
                MessageBox.Show("Không tìm thấy mã lớp học phần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var students = db.GetStudentsInSection(maLHP);

            if (!students.Any())
            {
                MessageBox.Show("Lớp này chưa có sinh viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form studentsForm = new StudentsListForm(maLHP, students);
            studentsForm.ShowDialog();
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

        private void InstructorMainForm_Load(object sender, EventArgs e)
        {

        }

        private void txtInstructorInfo_TextChanged(object sender, EventArgs e)
        {

        }
        
        // Cố gắng tìm logo ở nhiều vị trí thường gặp
        private string ResolveLogoPath()
        {
            // Danh sách tên/đuôi có thể
            var candidateNames = new[] { "neu.png", "logo.png", "neu.jpg", "logo.jpg", "neu.ico", "logo.ico" };
            
            // 1) bin/Debug|Release/Resources
            var startup = Application.StartupPath;
            foreach (var name in candidateNames)
            {
                var p = Path.Combine(startup, "Resources", name);
                if (File.Exists(p)) return p;
            }
            
            // 2) Thư mục dự án (2 cấp lên từ bin): projectRoot/Resources
            try
            {
                var projectRoot = Path.GetFullPath(Path.Combine(startup, "..", ".."));
                foreach (var name in candidateNames)
                {
                    var p = Path.Combine(projectRoot, "Resources", name);
                    if (File.Exists(p)) return p;
                }
            }
            catch { }
            
            // 3) Cùng thư mục chạy
            foreach (var name in candidateNames)
            {
                var p = Path.Combine(startup, name);
                if (File.Exists(p)) return p;
            }
            
            return string.Empty;
        }
    }
}


