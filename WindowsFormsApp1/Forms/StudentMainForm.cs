using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public partial class StudentMainForm : Form
    {
        private Student currentStudent;
        private DatabaseHelper db;

        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label lblHeaderTitle;
        private Button btnHome;
        private Button btnCourses;
        private Label lblUserInfo;
        
        // Card thông tin sinh viên (UI đẹp hơn thay cho txtStudentInfo)
        private Panel studentInfoCard;
        private TableLayoutPanel tlpStudentInfo;
        private Label lblValueMaSV;
        private Label lblValueHoTen;
        private Label lblValueCTDT;
        private Label lblValueNgaySinh;
        private Label lblValueGioiTinh;
        private Label lblValueDiaChi;
        private Label lblValueEmail;
        private Label lblValueSDT;

        public StudentMainForm(Student student)
        {
            InitializeComponent();
            currentStudent = student;
            db = DatabaseHelper.Instance;
            
            // Xử lý khi đóng form bằng nút X
            this.FormClosing += StudentMainForm_FormClosing;
            
            // Xử lý khi form thay đổi kích thước (resize, maximize, restore)
            this.ResizeEnd += StudentMainForm_ResizeEnd;
            this.Resize += StudentMainForm_Resize;
            
            // Mở form ở chế độ toàn màn hình
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            
            // Tạo layout mới
            CreateModernLayout();
            ApplyTheme();
            InitializeForm(); // Load dữ liệu sau khi layout và theme đã áp dụng
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
            lblHeaderTitle.AutoSize = true; // Tự động điều chỉnh kích thước
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
            lblUserInfo.Text = $"Xin chào,\r\n{currentStudent.TenSV}\r\n{currentStudent.MaSV}";
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
            
            btnCourses = new Button();
            btnCourses.Text = "📚 Đăng ký học phần";
            btnCourses.Size = new Size(230, 45);
            btnCourses.Location = new Point(0, 185);
            btnCourses.TextAlign = ContentAlignment.MiddleLeft;
            btnCourses.Padding = new Padding(15, 0, 0, 0);
            btnCourses.Margin = new Padding(0, 10, 0, 0);
            ThemeHelper.ApplyButtonStyle(btnCourses, ThemeHelper.SidebarActive, Color.White);
            
            // Logout button in sidebar (đặt ở cuối sidebar)
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
                userInfoPanel, btnHome, btnCourses
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
            contentPanel.Layout += ContentPanel_Layout; // Xử lý layout events
            // ContentPanel sẽ được đặt vị trí và kích thước sau khi header và sidebar đã được add
            
            // Di chuyển các controls vào contentPanel
            lblWelcome.Parent = contentPanel;
            txtStudentInfo.Parent = contentPanel;
            tabControl.Parent = contentPanel;
            btnRegister.Parent = contentPanel;
            btnCancel.Parent = contentPanel;
            btnLogout.Parent = contentPanel;
            btnExportPDF.Parent = tabRegistered;
            
            // Điều chỉnh vị trí controls trong contentPanel (padding đã được set ở Panel level)
            // Vị trí tính từ padding của Panel (25px)
            lblWelcome.Location = new Point(0, 0);
            // Ẩn textbox cũ, thay bằng card đẹp
            txtStudentInfo.Visible = false;
            
            // Tạo card thông tin sinh viên
            studentInfoCard = new Panel();
            studentInfoCard.Parent = contentPanel;
            studentInfoCard.Location = new Point(0, 35);
            studentInfoCard.Size = new Size(700, 120);
            studentInfoCard.BackColor = ThemeHelper.BackgroundWhite;
            studentInfoCard.BorderStyle = BorderStyle.FixedSingle;
            studentInfoCard.Padding = new Padding(12);
            studentInfoCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            
            tlpStudentInfo = new TableLayoutPanel();
            tlpStudentInfo.Parent = studentInfoCard;
            tlpStudentInfo.Dock = DockStyle.Fill;
            // Bố cục 2 cột: (Tiêu đề1, Giá trị1, Tiêu đề2, Giá trị2)
            tlpStudentInfo.ColumnCount = 4;
            tlpStudentInfo.RowCount = 4;
            tlpStudentInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlpStudentInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpStudentInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlpStudentInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 4; i++) tlpStudentInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            
            // Hàm thêm cặp tiêu đề-giá trị vào vị trí (row, groupColumn)
            // groupColumn: 0 cho cột trái, 1 cho cột phải
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
                
                tlpStudentInfo.Controls.Add(lblTitle, colTitle, row);
                tlpStudentInfo.Controls.Add(valueLabel, colValue, row);
            }
            
            // Cột trái
            AddPair(0, 0, "Mã sinh viên:", out lblValueMaSV);
            AddPair(1, 0, "Họ tên:", out lblValueHoTen);
            AddPair(2, 0, "Chương trình:", out lblValueCTDT);
            AddPair(3, 0, "Ngày sinh:", out lblValueNgaySinh);
            // Cột phải
            AddPair(0, 1, "Giới tính:", out lblValueGioiTinh);
            AddPair(1, 1, "Địa chỉ:", out lblValueDiaChi);
            AddPair(2, 1, "Email:", out lblValueEmail);
            AddPair(3, 1, "SĐT:", out lblValueSDT);
            
            // TabControl - đảm bảo hiển thị rõ ràng và không bị che
            // Location tính từ contentPanel (đã có padding), không cần thêm padding
            tabControl.Location = new Point(0, 190);
            // Chừa chỗ cho vùng nút ở cuối
            int buttonBarHeight = 60; // chiều cao khu vực nút
            int bottomMargin = 20;    // lề dưới
            int availableHeight = Math.Max(200, contentPanel.ClientSize.Height - tabControl.Location.Y - (buttonBarHeight + bottomMargin));
            tabControl.Size = new Size(contentPanel.ClientSize.Width, availableHeight);
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Visible = true;
            tabControl.Enabled = true;
            tabControl.Dock = DockStyle.None; // Không dùng Dock, dùng Anchor
            
            // QUAN TRỌNG: Đảm bảo TabPages hiển thị đúng và tabs rõ ràng
            if (tabControl.TabPages.Count >= 1)
            {
                tabControl.TabPages[0].Text = "Lớp đã đăng ký";
                tabControl.TabPages[0].UseVisualStyleBackColor = false;
            }
            if (tabControl.TabPages.Count >= 2)
            {
                tabControl.TabPages[1].Text = "Lớp có thể đăng ký";
                tabControl.TabPages[1].UseVisualStyleBackColor = false;
            }
            
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.Multiline = false;
            tabControl.DrawMode = TabDrawMode.Normal;
            tabControl.ShowToolTips = false;
            tabControl.SizeMode = TabSizeMode.Normal;
            
            // Đảm bảo TabControl có chiều cao đủ để hiển thị tabs (tối thiểu 30px cho tab header)
            if (tabControl.Height < 30)
            {
                tabControl.Height = Math.Max(30, tabControl.Height);
            }
            
            // Điều chỉnh DataGridView trong TabPages - ĐỢI TabControl layout xong
            // Sẽ được gọi trong TabControl_Resize hoặc sau khi form shown
            
            // Đảm bảo TabPages có nền trắng
            tabRegistered.BackColor = ThemeHelper.BackgroundWhite;
            tabAvailable.BackColor = ThemeHelper.BackgroundWhite;
            
            // Buttons ở dưới cùng
            btnRegister.Location = new Point(0, contentPanel.Height - 60);
            btnCancel.Location = new Point(160, contentPanel.Height - 60);
            btnRegister.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.Visible = false; // Ẩn button logout cũ, dùng button trong sidebar
            btnRegister.Visible = true;
            btnCancel.Visible = true;
            
            // Đảm bảo TabControl và DataGridViews visible và hiển thị đúng
            tabControl.Visible = true;
            tabControl.Enabled = true;
            tabControl.TabStop = true;
            tabControl.Show(); // Force show
            
            // Đảm bảo TabPages visible
            tabRegistered.Visible = true;
            tabRegistered.Enabled = true;
            tabAvailable.Visible = true;
            tabAvailable.Enabled = true;
            
            // Đảm bảo DataGridViews visible và hiển thị
            dgvRegisteredCourses.Visible = true;
            dgvRegisteredCourses.Enabled = true;
            dgvAvailableCourses.Visible = true;
            dgvAvailableCourses.Enabled = true;
            
            // Đảm bảo labels trong TabPages visible
            lblRegisteredTitle.Visible = true;
            lblAvailableTitle.Visible = true;
            
            // Xử lý resize TabPages để update DataGridView
            tabControl.Resize += TabControl_Resize;
            
            // Thêm các panels vào form - thứ tự QUAN TRỌNG!
            // Header phải add đầu tiên (Dock = Top)
            this.Controls.Add(headerPanel);
            
            // Sidebar add thứ hai (Dock = Left, sẽ tự động ở dưới header)
            this.Controls.Add(sidebarPanel);
            
            // ContentPanel add cuối cùng và đặt vị trí/kích thước để không bị che
            this.Controls.Add(contentPanel);
            
            // Đặt vị trí và kích thước contentPanel để không bị sidebar và header che
            // ContentPanel phải bắt đầu từ bên phải sidebar và dưới header
            UpdateContentPanelLayout();
            
            // Đảm bảo thứ tự z-order đúng trong form
            contentPanel.SendToBack(); // ContentPanel ở sau (dưới sidebar và header)
            sidebarPanel.BringToFront(); // Sidebar ở giữa
            headerPanel.BringToFront(); // Header ở trên cùng
            
            // Đảm bảo TabControl ở trên cùng trong contentPanel
            // Thứ tự trong contentPanel: Welcome và TextBox ở dưới, TabControl ở trên
            lblWelcome.SendToBack();
            txtStudentInfo.SendToBack();
            
            // Đảm bảo TabControl không che khu vực nút
            tabControl.BringToFront();
            btnRegister.BringToFront();
            btnCancel.BringToFront();
            
            // QUAN TRỌNG: Đảm bảo DataGridViews vẫn ở trong TabPages
            // Kiểm tra xem DataGridViews có đang trong TabPages không
            if (!tabRegistered.Controls.Contains(dgvRegisteredCourses))
            {
                tabRegistered.Controls.Add(dgvRegisteredCourses);
            }
            if (!tabAvailable.Controls.Contains(dgvAvailableCourses))
            {
                tabAvailable.Controls.Add(dgvAvailableCourses);
            }
            
            // Đảm bảo labels trong TabPages
            if (!tabRegistered.Controls.Contains(lblRegisteredTitle))
            {
                tabRegistered.Controls.Add(lblRegisteredTitle);
            }
            if (!tabRegistered.Controls.Contains(btnExportPDF))
            {
                tabRegistered.Controls.Add(btnExportPDF);
            }
            btnExportPDF.Click += btnExportPDF_Click;
            if (!tabAvailable.Controls.Contains(lblAvailableTitle))
            {
                tabAvailable.Controls.Add(lblAvailableTitle);
            }
            
            // Đảm bảo TabPages và DataGridViews có z-order đúng
            if (tabRegistered != null)
            {
                tabRegistered.BringToFront();
                if (dgvRegisteredCourses != null)
                {
                    dgvRegisteredCourses.BringToFront();
                    dgvRegisteredCourses.Visible = true;
                }
                if (lblRegisteredTitle != null)
                {
                    lblRegisteredTitle.BringToFront();
                    lblRegisteredTitle.Visible = true;
                }
            }
            if (tabAvailable != null)
            {
                tabAvailable.BringToFront();
                if (dgvAvailableCourses != null)
                {
                    dgvAvailableCourses.BringToFront();
                    dgvAvailableCourses.Visible = true;
                }
                if (lblAvailableTitle != null)
                {
                    lblAvailableTitle.BringToFront();
                    lblAvailableTitle.Visible = true;
                }
            }
            
            // Xử lý khi tab được chọn để update DataGridView sizes
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            
            // Xử lý resize TabControl
            tabControl.Resize += TabControl_Resize;
        }
        
        // Cố gắng tìm logo ở nhiều vị trí thường gặp
        private string ResolveLogoPath()
        {
            // Danh sách tên/đuôi có thể
            var candidateNames = new[] { "neu.png"};
            
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
            return string.Empty;
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
        
        private void StudentMainForm_Resize(object sender, EventArgs e)
        {
            // Cập nhật layout của contentPanel khi form resize
            UpdateContentPanelLayout();
            
            if (contentPanel != null && tabControl != null)
            {
                // Update TabControl size khi form resize (dùng ClientSize để tính đúng)
                // Chừa vùng nút cuối màn hình
                int buttonBarHeight = 60;
                int bottomMargin = 20;
                int tabTop = tabControl.Location.Y;
                int tabControlHeight = Math.Max(200, contentPanel.ClientSize.Height - tabTop - (buttonBarHeight + bottomMargin));
                tabControl.Size = new Size(contentPanel.ClientSize.Width, tabControlHeight);
                
                // Update button positions (buttons đã dùng Anchor nên sẽ tự động update)
                if (btnRegister != null && contentPanel.ClientSize.Height > 60)
                {
                    btnRegister.Location = new Point(0, contentPanel.ClientSize.Height - 60);
                    btnCancel.Location = new Point(160, contentPanel.ClientSize.Height - 60);
                }
                
                // Đảm bảo nút không bị che
                btnRegister?.BringToFront();
                btnCancel?.BringToFront();
            }
        }
        
        private void StudentMainForm_ResizeEnd(object sender, EventArgs e)
        {
            // Sau khi form resize xong (bao gồm maximize/restore)
            if (tabControl != null && contentPanel != null)
            {
                // Update DataGridView sizes sau khi TabControl đã hoàn tất resize
                this.BeginInvoke(new Action(() =>
                {
                    // Update kích thước
                    UpdateDataGridViewSizes();
                    
                    
                    // Force refresh để đảm bảo hiển thị đúng
                    if (tabControl != null)
                    {
                        tabControl.BringToFront();
                        tabControl.Invalidate();
                        tabControl.Update();
                        tabControl.Refresh();
                    }
                    
                    // Refresh TabPages và DataGridViews
                    if (tabRegistered != null)
                    {
                        tabRegistered.BringToFront();
                        tabRegistered.Invalidate();
                        tabRegistered.Update();
                        if (dgvRegisteredCourses != null)
                        {
                            dgvRegisteredCourses.BringToFront();
                            dgvRegisteredCourses.Invalidate();
                            dgvRegisteredCourses.Update();
                            dgvRegisteredCourses.Refresh();
                        }
                    }
                    if (tabAvailable != null)
                    {
                        tabAvailable.BringToFront();
                        tabAvailable.Invalidate();
                        tabAvailable.Update();
                        if (dgvAvailableCourses != null)
                        {
                            dgvAvailableCourses.BringToFront();
                            dgvAvailableCourses.Invalidate();
                            dgvAvailableCourses.Update();
                            dgvAvailableCourses.Refresh();
                        }
                    }
                }));
            }
        }
        

        private void TabControl_Resize(object sender, EventArgs e)
        {
            // Update DataGridView sizes khi TabControl resize
            UpdateDataGridViewSizes();
        }
        
        private void UpdateDataGridViewSizes()
        {
            // Điều chỉnh DataGridView trong Tab Registered
            if (tabRegistered != null && dgvRegisteredCourses != null)
            {
                // Đảm bảo dgvRegisteredCourses trong tabRegistered
                if (!tabRegistered.Controls.Contains(dgvRegisteredCourses))
                {
                    tabRegistered.Controls.Add(dgvRegisteredCourses);
                }
                
                lblRegisteredTitle.Location = new Point(10, 10);
                lblRegisteredTitle.BringToFront();
                
                dgvRegisteredCourses.Location = new Point(10, 45);
                dgvRegisteredCourses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                
                // Tính toán kích thước dựa trên ClientSize của TabPage (đã trừ tab header và nút)
                int availableWidth = Math.Max(100, tabRegistered.ClientSize.Width - 20);
                int buttonAreaHeight = 50; // Chừa chỗ cho nút
                int availableHeight = Math.Max(100, tabRegistered.ClientSize.Height - 55 - buttonAreaHeight);
                dgvRegisteredCourses.Size = new Size(availableWidth, availableHeight);
                
                // Đặt nút xuất PDF
                if (btnExportPDF != null)
                {
                    btnExportPDF.Location = new Point(10, dgvRegisteredCourses.Bottom + 10);
                    btnExportPDF.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                    btnExportPDF.BringToFront();
                }
                
                dgvRegisteredCourses.Visible = true;
                dgvRegisteredCourses.Enabled = true;
                dgvRegisteredCourses.Show();
                dgvRegisteredCourses.BringToFront();
                dgvRegisteredCourses.Refresh();
                dgvRegisteredCourses.Update();
            }
            
            // Điều chỉnh DataGridView trong Tab Available
            if (tabAvailable != null && dgvAvailableCourses != null)
            {
                // Đảm bảo dgvAvailableCourses trong tabAvailable
                if (!tabAvailable.Controls.Contains(dgvAvailableCourses))
                {
                    tabAvailable.Controls.Add(dgvAvailableCourses);
                }
                
                lblAvailableTitle.Location = new Point(10, 10);
                lblAvailableTitle.BringToFront();
                
                dgvAvailableCourses.Location = new Point(10, 45);
                dgvAvailableCourses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                
                // Tính toán kích thước dựa trên ClientSize của TabPage (đã trừ tab header)
                int availableWidth = Math.Max(100, tabAvailable.ClientSize.Width - 20);
                int availableHeight = Math.Max(100, tabAvailable.ClientSize.Height - 55);
                dgvAvailableCourses.Size = new Size(availableWidth, availableHeight);
                
                dgvAvailableCourses.Visible = true;
                dgvAvailableCourses.Enabled = true;
                dgvAvailableCourses.Show();
                dgvAvailableCourses.BringToFront();
                dgvAvailableCourses.Refresh();
                dgvAvailableCourses.Update();
            }
        }
        
        private void ContentPanel_Layout(object sender, LayoutEventArgs e)
        {
            // Đảm bảo TabControl luôn hiển thị đúng khi contentPanel layout
            if (tabControl != null && tabControl.Parent == contentPanel)
            {
                tabControl.BringToFront();
                // Update DataGridView sizes khi layout
                UpdateDataGridViewSizes();
            }
        }
        
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update DataGridView sizes khi chuyển tab
            UpdateDataGridViewSizes();
        }
        
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
            // Đảm bảo TabControl hiển thị đúng sau khi form được show
            if (tabControl != null)
            {
                // Đảm bảo TabControl ở trên cùng và không bị che
                contentPanel.Invalidate();
                contentPanel.Update();
                
                tabControl.BringToFront();
                tabControl.Invalidate();
                tabControl.Update();
                
                // Đảm bảo TabPages visible
                if (tabControl.TabPages.Count > 0)
                {
                    tabControl.TabPages[0].Visible = true;
                    if (tabControl.TabPages.Count > 1)
                    {
                        tabControl.TabPages[1].Visible = true;
                    }
                }
                
                // Cập nhật kích thước DataGridViews sau khi TabControl đã layout
                UpdateDataGridViewSizes();
                
                // Force refresh toàn bộ
                this.Invalidate();
                this.Update();
                this.Refresh();
                
                // Refresh TabControl và TabPages
                tabControl.Refresh();
                tabControl.Update();
                
                if (tabRegistered != null)
                {
                    tabRegistered.Refresh();
                    tabRegistered.Update();
                }
                if (tabAvailable != null)
                {
                    tabAvailable.Refresh();
                    tabAvailable.Update();
                }
                
                // Force refresh DataGridViews
                if (dgvRegisteredCourses != null)
                {
                    dgvRegisteredCourses.Refresh();
                    dgvRegisteredCourses.Update();
                    dgvRegisteredCourses.Invalidate();
                }
                if (dgvAvailableCourses != null)
                {
                    dgvAvailableCourses.Refresh();
                    dgvAvailableCourses.Update();
                    dgvAvailableCourses.Invalidate();
                }
            }
        }
        
        private void ApplyTheme()
        {
            // Style cho labels
            lblWelcome.Font = ThemeHelper.SubHeaderFont;
            lblWelcome.ForeColor = ThemeHelper.TextDark;
            
            // Style cho card thông tin
            if (studentInfoCard != null)
            {
                studentInfoCard.BackColor = ThemeHelper.BackgroundWhite;
            }
            
            // Style cho textbox
            txtStudentInfo.BackColor = ThemeHelper.BackgroundWhite;
            txtStudentInfo.ForeColor = ThemeHelper.TextDark;
            txtStudentInfo.Font = ThemeHelper.NormalFont;
            txtStudentInfo.BorderStyle = BorderStyle.FixedSingle;
            
            // Style cho DataGridViews
            ThemeHelper.ApplyDataGridViewStyle(dgvRegisteredCourses);
            ThemeHelper.ApplyDataGridViewStyle(dgvAvailableCourses);
            
            // Style cho buttons
            ThemeHelper.ApplyButtonStyle(btnRegister, ThemeHelper.SuccessGreen, Color.White);
            ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.DangerRed, Color.White);
            ThemeHelper.ApplyButtonStyle(btnExportPDF, Color.FromArgb(0, 123, 255), Color.White);
            
            // Style cho TabControl
            tabControl.Font = ThemeHelper.NormalFont;
            tabControl.Appearance = TabAppearance.Normal; // Dùng Normal để tabs hiển thị rõ hơn
            
            // Style cho tab pages - đảm bảo visible và có nền
            tabRegistered.BackColor = ThemeHelper.BackgroundWhite;
            tabRegistered.UseVisualStyleBackColor = false;
            tabAvailable.BackColor = ThemeHelper.BackgroundWhite;
            tabAvailable.UseVisualStyleBackColor = false;
            
            lblRegisteredTitle.Font = ThemeHelper.SubHeaderFont;
            lblRegisteredTitle.ForeColor = ThemeHelper.TextDark;
            lblAvailableTitle.Font = ThemeHelper.SubHeaderFont;
            lblAvailableTitle.ForeColor = ThemeHelper.TextDark;
            
            // Đảm bảo TabControl và DataGridViews hiển thị đúng
            tabControl.Show();
            tabControl.Refresh();
            dgvRegisteredCourses.Show();
            dgvAvailableCourses.Show();
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
            
            // Load dữ liệu vào DataGridViews
            LoadRegisteredCourses();
            LoadAvailableCourses();
            
            // Đảm bảo DataGridViews visible và refresh sau khi load dữ liệu
            if (dgvRegisteredCourses != null)
            {
                dgvRegisteredCourses.Visible = true;
                dgvRegisteredCourses.Refresh();
                dgvRegisteredCourses.Update();
            }
            if (dgvAvailableCourses != null)
            {
                dgvAvailableCourses.Visible = true;
                dgvAvailableCourses.Refresh();
                dgvAvailableCourses.Update();
            }
            
            // Cập nhật kích thước DataGridViews sau khi có dữ liệu
            UpdateDataGridViewSizes();
        }

        private void LoadStudentInfo()
        {
            // Load thông tin chương trình đào tạo từ database
            string tenCTDT = db.GetProgramName(currentStudent.MaCTDT);
            
            // Đổ dữ liệu vào card đẹp
            if (lblValueMaSV != null)
            {
                lblValueMaSV.Text = currentStudent.MaSV;
                lblValueHoTen.Text = currentStudent.TenSV;
                lblValueCTDT.Text = tenCTDT;
                lblValueNgaySinh.Text = currentStudent.NgaySinh.ToString("dd/MM/yyyy");
                lblValueGioiTinh.Text = currentStudent.GioiTinh;
                lblValueDiaChi.Text = currentStudent.DiaChi;
                lblValueEmail.Text = currentStudent.Email;
                lblValueSDT.Text = currentStudent.SDT;
            }
            else
            {
                // Fallback cho trường hợp card chưa khởi tạo
                txtStudentInfo.Text = $"Mã sinh viên: {currentStudent.MaSV}\r\n" +
                                      $"Họ tên: {currentStudent.TenSV}\r\n" +
                                      $"Chương trình: {tenCTDT}\r\n" +
                                      $"Ngày sinh: {currentStudent.NgaySinh:dd/MM/yyyy}\r\n" +
                                      $"Giới tính: {currentStudent.GioiTinh}\r\n" +
                                      $"Địa chỉ: {currentStudent.DiaChi}\r\n" +
                                      $"Email: {currentStudent.Email}\r\n" +
                                      $"SĐT: {currentStudent.SDT}";
            }
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
                    section.HinhThuc ?? "Kế hoạch", // Hình thức đăng ký: "Kế hoạch" hoặc "Học vượt"
                    $"{section.SoLuongDangKy}/{section.SiSo}"
                );
            }
        }

        private void LoadAvailableCourses()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var registeredSections = db.GetRegisteredSections(currentStudent.MaSV);
                
                if (registeredSections.Count == 0)
                {
                    MessageBox.Show("Bạn chưa đăng ký học phần nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"DanhSachHocPhan_{currentStudent.MaSV}_{DateTime.Now:yyyyMMdd}.pdf";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportRegisteredCoursesToPdf(registeredSections, saveDialog.FileName);
                    MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportRegisteredCoursesToPdf(List<CourseSection> sections, string filePath)
        {
            // Tạo file HTML với bảng danh sách học phần
            string htmlContent = GenerateHtmlReport(sections);
            string htmlPath = Path.ChangeExtension(filePath, ".html");
            File.WriteAllText(htmlPath, htmlContent, System.Text.Encoding.UTF8);
        }

        private string GenerateHtmlReport(List<CourseSection> sections)
        {
            string tenCTDT = db.GetProgramName(currentStudent.MaCTDT);
            int tongTC = sections.Sum(s => s.SoTC);
            
            string html = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Danh sách học phần đã đăng ký</title>
    <style>
        body {{ font-family: 'Times New Roman', serif; margin: 20px; }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .header h1 {{ margin: 0; font-size: 18px; font-weight: bold; }}
        .header h2 {{ margin: 5px 0; font-size: 16px; }}
        .info {{ margin-bottom: 20px; }}
        .info table {{ width: 100%; border-collapse: collapse; }}
        .info td {{ padding: 5px; }}
        .info td:first-child {{ font-weight: bold; width: 150px; }}
        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
        th, td {{ border: 1px solid #000; padding: 8px; text-align: left; }}
        th {{ background-color: #f0f0f0; font-weight: bold; }}
        .footer {{ margin-top: 30px; text-align: right; }}
        .total {{ font-weight: bold; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>TRƯỜNG ĐẠI HỌC KINH TẾ QUỐC DÂN</h1>
        <h2>DANH SÁCH HỌC PHẦN ĐÃ ĐĂNG KÝ</h2>
    </div>
    
    <div class='info'>
        <table>
            <tr>
                <td>Mã sinh viên:</td>
                <td>{currentStudent.MaSV}</td>
                <td>Họ và tên:</td>
                <td>{currentStudent.TenSV}</td>
            </tr>
            <tr>
                <td>Chương trình đào tạo:</td>
                <td>{tenCTDT}</td>
                <td>Ngày xuất:</td>
                <td>{DateTime.Now:dd/MM/yyyy HH:mm}</td>
            </tr>
        </table>
    </div>
    
    <table>
        <thead>
            <tr>
                <th>STT</th>
                <th>Mã lớp HP</th>
                <th>Tên học phần</th>
                <th>Lớp</th>
                <th>Số TC</th>
                <th>Lịch học</th>
                <th>Giảng viên</th>
                <th>Hình thức</th>
            </tr>
        </thead>
        <tbody>";
            
            int stt = 1;
            foreach (var section in sections)
            {
                html += $@"
            <tr>
                <td>{stt++}</td>
                <td>{section.MaLHP}</td>
                <td>{section.TenHocPhan}</td>
                <td>{section.TenLop}</td>
                <td>{section.SoTC}</td>
                <td>{section.LichHoc ?? ""}</td>
                <td>{section.TenGiangVien}</td>
                <td>{section.HinhThuc ?? "Kế hoạch"}</td>
            </tr>";
            }
            
            html += $@"
            <tr class='total'>
                <td colspan='4' style='text-align: right;'><strong>Tổng số tín chỉ:</strong></td>
                <td><strong>{tongTC}</strong></td>
                <td colspan='3'></td>
            </tr>
        </tbody>
    </table>
    
    <div class='footer'>
        <p>Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}</p>
        <p style='margin-top: 50px;'><strong>Người lập</strong></p>
        <p style='margin-top: 30px;'><strong>{currentStudent.TenSV}</strong></p>
    </div>
</body>
</html>";
            
            return html;
        }


        private void CreatePdfUsingPrintDocument(List<CourseSection> sections, string pdfPath)
        {
            // Sử dụng PrintDocument để tạo PDF
            // Cần Microsoft Print to PDF được cài đặt trên Windows
            
            using (PrintDocument printDoc = new PrintDocument())
            {
                printDoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                printDoc.PrinterSettings.PrintToFile = true;
                printDoc.PrinterSettings.PrintFileName = pdfPath;
                
                printDoc.PrintPage += (sender, e) =>
                {
                    DrawReportPage(sections, e);
                };
                
                printDoc.Print();
            }
        }

        private void DrawReportPage(List<CourseSection> sections, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Vẽ nội dung báo cáo lên trang in
            float yPos = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float rightMargin = e.MarginBounds.Right;
            float bottomMargin = e.MarginBounds.Bottom;
            
            Font titleFont = new Font("Times New Roman", 16, FontStyle.Bold);
            Font headerFont = new Font("Times New Roman", 14, FontStyle.Bold);
            Font normalFont = new Font("Times New Roman", 10);
            Font tableFont = new Font("Times New Roman", 9);
            
            // Tiêu đề
            string title = "TRƯỜNG ĐẠI HỌC KINH TẾ QUỐC DÂN";
            string subtitle = "DANH SÁCH HỌC PHẦN ĐÃ ĐĂNG KÝ";
            
            SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
            e.Graphics.DrawString(title, titleFont, Brushes.Black, 
                leftMargin + (e.MarginBounds.Width - titleSize.Width) / 2, yPos + topMargin);
            yPos += titleSize.Height + 10;
            
            SizeF subtitleSize = e.Graphics.MeasureString(subtitle, headerFont);
            e.Graphics.DrawString(subtitle, headerFont, Brushes.Black,
                leftMargin + (e.MarginBounds.Width - subtitleSize.Width) / 2, yPos + topMargin);
            yPos += subtitleSize.Height + 20;
            
            // Thông tin sinh viên
            string tenCTDT = db.GetProgramName(currentStudent.MaCTDT);
            string info1 = $"Mã sinh viên: {currentStudent.MaSV}";
            string info2 = $"Họ và tên: {currentStudent.TenSV}";
            string info3 = $"Chương trình: {tenCTDT}";
            string info4 = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
            
            e.Graphics.DrawString(info1, normalFont, Brushes.Black, leftMargin, yPos + topMargin);
            yPos += normalFont.Height + 5;
            e.Graphics.DrawString(info2, normalFont, Brushes.Black, leftMargin, yPos + topMargin);
            yPos += normalFont.Height + 5;
            e.Graphics.DrawString(info3, normalFont, Brushes.Black, leftMargin, yPos + topMargin);
            yPos += normalFont.Height + 5;
            e.Graphics.DrawString(info4, normalFont, Brushes.Black, leftMargin, yPos + topMargin);
            yPos += normalFont.Height + 15;
            
            // Vẽ bảng
            float tableWidth = e.MarginBounds.Width;
            float colWidth = tableWidth / 8;
            float rowHeight = tableFont.Height + 4;
            
            // Header
            string[] headers = { "STT", "Mã lớp HP", "Tên học phần", "Lớp", "Số TC", "Lịch học", "Giảng viên", "Hình thức" };
            float xPos = leftMargin;
            for (int i = 0; i < headers.Length; i++)
            {
                RectangleF cellRect = new RectangleF(xPos, yPos + topMargin, colWidth, rowHeight);
                e.Graphics.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                e.Graphics.DrawString(headers[i], tableFont, Brushes.Black, cellRect, 
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                xPos += colWidth;
            }
            yPos += rowHeight;
            
            // Dữ liệu
            int stt = 1;
            int tongTC = 0;
            foreach (var section in sections)
            {
                if (yPos + topMargin + rowHeight > bottomMargin)
                {
                    e.HasMorePages = true;
                    return;
                }
                
                xPos = leftMargin;
                string[] rowData = {
                    stt++.ToString(),
                    section.MaLHP,
                    section.TenHocPhan,
                    section.TenLop,
                    section.SoTC.ToString(),
                    section.LichHoc ?? "",
                    section.TenGiangVien,
                    section.HinhThuc ?? "Kế hoạch"
                };
                
                tongTC += section.SoTC;
                
                for (int i = 0; i < rowData.Length; i++)
                {
                    RectangleF cellRect = new RectangleF(xPos, yPos + topMargin, colWidth, rowHeight);
                    e.Graphics.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    
                    StringFormat format = new StringFormat();
                    if (i == 0 || i == 4) // STT và Số TC căn giữa
                        format.Alignment = StringAlignment.Center;
                    else
                        format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    
                    e.Graphics.DrawString(rowData[i], tableFont, Brushes.Black, cellRect, format);
                    xPos += colWidth;
                }
                yPos += rowHeight;
            }
            
            // Tổng số tín chỉ
            yPos += 5;
            xPos = leftMargin;
            RectangleF totalRect = new RectangleF(xPos, yPos + topMargin, colWidth * 4, rowHeight);
            e.Graphics.DrawRectangle(Pens.Black, totalRect.X, totalRect.Y, totalRect.Width, totalRect.Height);
            e.Graphics.DrawString("Tổng số tín chỉ:", tableFont, Brushes.Black, totalRect,
                new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
            
            xPos += colWidth * 4;
            RectangleF totalValueRect = new RectangleF(xPos, yPos + topMargin, colWidth, rowHeight);
            e.Graphics.DrawRectangle(Pens.Black, totalValueRect.X, totalValueRect.Y, totalValueRect.Width, totalValueRect.Height);
            e.Graphics.DrawString(tongTC.ToString(), tableFont, Brushes.Black, totalValueRect,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            
            // Footer
            yPos += rowHeight + 30;
            string footer1 = $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
            e.Graphics.DrawString(footer1, normalFont, Brushes.Black, rightMargin - 200, yPos + topMargin);
            
            yPos += normalFont.Height + 20;
            e.Graphics.DrawString("Người lập", normalFont, Brushes.Black, rightMargin - 200, yPos + topMargin);
            
            yPos += normalFont.Height + 30;
            e.Graphics.DrawString(currentStudent.TenSV, normalFont, Brushes.Black, rightMargin - 200, yPos + topMargin);
            
            e.HasMorePages = false;
        }
    }
}


