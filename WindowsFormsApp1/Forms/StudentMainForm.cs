using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
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
            btnSchedule.Parent = contentPanel;
            btnLogout.Parent = contentPanel;
            
            // Điều chỉnh vị trí controls trong contentPanel (padding đã được set ở Panel level)
            // Vị trí tính từ padding của Panel (25px)
            lblWelcome.Location = new Point(0, 0);
            txtStudentInfo.Location = new Point(0, 35);
            txtStudentInfo.Size = new Size(400, 100);
            
            // TabControl - đảm bảo hiển thị rõ ràng và không bị che
            // Location tính từ contentPanel (đã có padding), không cần thêm padding
            tabControl.Location = new Point(0, 140);
            tabControl.Size = new Size(contentPanel.ClientSize.Width, contentPanel.ClientSize.Height - 200);
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
            btnSchedule.Location = new Point(320, contentPanel.Height - 60);
            btnRegister.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSchedule.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.Visible = false; // Ẩn button logout cũ, dùng button trong sidebar
            
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
            
            // TabControl phải ở trên cùng để không bị che
            tabControl.BringToFront();
            
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
                // TabControl đã dùng Anchor nên sẽ tự động resize, nhưng cần đảm bảo Height
                int tabControlHeight = Math.Max(200, contentPanel.ClientSize.Height - 200); // Để chỗ cho buttons
                if (tabControl.Height != tabControlHeight)
                {
                    tabControl.Size = new Size(contentPanel.ClientSize.Width, tabControlHeight);
                }
                
                // Update button positions (buttons đã dùng Anchor nên sẽ tự động update)
                if (btnRegister != null && contentPanel.ClientSize.Height > 60)
                {
                    btnRegister.Location = new Point(0, contentPanel.ClientSize.Height - 60);
                    btnCancel.Location = new Point(160, contentPanel.ClientSize.Height - 60);
                    btnSchedule.Location = new Point(320, contentPanel.ClientSize.Height - 60);
                }
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
                    // QUAN TRỌNG: Đảm bảo DataGridViews vẫn ở trong TabPages
                    EnsureDataGridViewsInTabPages();
                    
                    // Kiểm tra và reload dữ liệu nếu bị mất
                    EnsureDataGridViewsHaveData();
                    
                    // Update kích thước
                    UpdateDataGridViewSizes();
                    
                    // Đảm bảo DataGridViews visible và enabled
                    EnsureDataGridViewsVisible();
                    
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
        
        private void EnsureDataGridViewsHaveData()
        {
            // Kiểm tra và reload dữ liệu nếu DataGridView bị mất dữ liệu
            // Chỉ reload nếu không có dữ liệu và không đang trong quá trình load
            if (dgvRegisteredCourses != null && dgvRegisteredCourses.Rows.Count == 0)
            {
                // Có thể dữ liệu chưa được load hoặc bị mất, reload lại
                LoadRegisteredCourses();
            }
            
            if (dgvAvailableCourses != null && dgvAvailableCourses.Rows.Count == 0)
            {
                // Có thể dữ liệu chưa được load hoặc bị mất, reload lại
                LoadAvailableCourses();
            }
        }
        
        private void EnsureDataGridViewsInTabPages()
        {
            // Đảm bảo DataGridViews vẫn ở trong TabPages
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
            if (!tabAvailable.Controls.Contains(lblAvailableTitle))
            {
                tabAvailable.Controls.Add(lblAvailableTitle);
            }
        }
        
        private void EnsureDataGridViewsVisible()
        {
            // Đảm bảo DataGridViews visible và enabled
            if (dgvRegisteredCourses != null)
            {
                dgvRegisteredCourses.Visible = true;
                dgvRegisteredCourses.Enabled = true;
                dgvRegisteredCourses.Show();
            }
            if (dgvAvailableCourses != null)
            {
                dgvAvailableCourses.Visible = true;
                dgvAvailableCourses.Enabled = true;
                dgvAvailableCourses.Show();
            }
            
            // Đảm bảo TabPages visible
            if (tabRegistered != null)
            {
                tabRegistered.Visible = true;
                tabRegistered.Enabled = true;
            }
            if (tabAvailable != null)
            {
                tabAvailable.Visible = true;
                tabAvailable.Enabled = true;
            }
            
            // Đảm bảo labels visible
            if (lblRegisteredTitle != null)
            {
                lblRegisteredTitle.Visible = true;
            }
            if (lblAvailableTitle != null)
            {
                lblAvailableTitle.Visible = true;
            }
        }
        
        private void TabControl_Resize(object sender, EventArgs e)
        {
            // Update DataGridView sizes khi TabControl resize
            UpdateDataGridViewSizes();
        }
        
        private void UpdateDataGridViewSizes()
        {
            // Đảm bảo DataGridViews ở trong TabPages trước
            EnsureDataGridViewsInTabPages();
            
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
                
                // Tính toán kích thước dựa trên ClientSize của TabPage (đã trừ tab header)
                int availableWidth = Math.Max(100, tabRegistered.ClientSize.Width - 20);
                int availableHeight = Math.Max(100, tabRegistered.ClientSize.Height - 55);
                dgvRegisteredCourses.Size = new Size(availableWidth, availableHeight);
                
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
            ThemeHelper.ApplyButtonStyle(btnSchedule, ThemeHelper.PrimaryBlue, Color.White);
            
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


