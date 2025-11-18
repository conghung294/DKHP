using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.UI;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public class AdminMainForm : Form
    {
        private Panel headerPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Label lblTitle;
        private Button btnLogout;
        private Button btnHelp;
        private TabControl tabControl;
        private TabPage tabStudents;
        private TabPage tabInstructors;
        private TabPage tabCourses;
        private TabPage tabSections;
        private TabPage tabDepartments;
        private DataGridView dgvStudents;
        private DataGridView dgvInstructors;
        private DataGridView dgvCourses;
        private DataGridView dgvSections;
        private DataGridView dgvDepartments;
        private ToolStrip studentsToolbar;
        private ToolStrip instructorsToolbar;
        private ToolStrip coursesToolbar;
        private ToolStrip sectionsToolbar;
        private ToolStrip departmentsToolbar;
        private Button btnNavStudents;
        private Button btnNavInstructors;
        private Button btnNavCourses;
        private Button btnNavSections;
        private Button btnNavDepartments;
        private PictureBox picLogo;

        // Dữ liệu và control tìm kiếm sinh viên
        private List<Student> allStudents;
        private ToolStripTextBox txtStudentSearch;

        private readonly string adminName;

        public AdminMainForm(string adminName = "ADMIN")
        {
            this.adminName = adminName;
            this.Text = "Bảng điều khiển Admin";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateLayout();
            ApplyTheme();
            BindEvents();

            LoadStudents();
            LoadInstructors();
            LoadCourses();
            LoadSections();
            LoadDepartments();
        }

        private void CreateLayout()
        {
            headerPanel = new Panel { Height = 56, Dock = DockStyle.Top, BackColor = ThemeHelper.HeaderBlue };
            lblTitle = new Label { Text = "Quản trị hệ thống - " + adminName, AutoSize = true, ForeColor = Color.White, Location = new Point(16, 16), Font = ThemeHelper.HeaderFont };
            btnLogout = new Button { Text = "Đăng xuất", Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(110, 32) };
            btnHelp = new Button { Text = "Help", Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(80, 32) };
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnLogout);
            headerPanel.Controls.Add(btnHelp);

            sidebarPanel = new Panel { Width = 220, Dock = DockStyle.Left, BackColor = ThemeHelper.SidebarBackground };

            // Sidebar content: logo + nav buttons
            picLogo = new PictureBox { Size = new Size(64, 64), Location = new Point(16, 16), SizeMode = PictureBoxSizeMode.Zoom };
            TryLoadLogo(picLogo);
            var lblApp = new Label { Text = "Admin", AutoSize = true, Font = ThemeHelper.SubHeaderFont, ForeColor = ThemeHelper.TextDark, Location = new Point(96, 36) };

            btnNavStudents = new Button { Text = "Quản lý sinh viên", Location = new Point(12, 100), Size = new Size(196, 38) };
            btnNavInstructors = new Button { Text = "Quản lý giảng viên", Location = new Point(12, 142), Size = new Size(196, 38) };
            btnNavCourses = new Button { Text = "Quản lý môn học", Location = new Point(12, 184), Size = new Size(196, 38) };
            btnNavSections = new Button { Text = "Lớp học phần", Location = new Point(12, 226), Size = new Size(196, 38) };
            btnNavDepartments = new Button { Text = "Khoa", Location = new Point(12, 268), Size = new Size(196, 38) };

            sidebarPanel.Controls.Add(picLogo);
            sidebarPanel.Controls.Add(lblApp);
            sidebarPanel.Controls.Add(btnNavStudents);
            sidebarPanel.Controls.Add(btnNavInstructors);
            sidebarPanel.Controls.Add(btnNavCourses);
            sidebarPanel.Controls.Add(btnNavSections);
            sidebarPanel.Controls.Add(btnNavDepartments);

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = ThemeHelper.BackgroundLight };

            tabControl = new TabControl { Dock = DockStyle.Fill };
            tabStudents = new TabPage("Quản lý sinh viên");
            tabInstructors = new TabPage("Quản lý giảng viên");
            tabCourses = new TabPage("Quản lý môn học");
            tabSections = new TabPage("Lớp học phần");
            tabDepartments = new TabPage("Khoa");

            // Students tab
            studentsToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            studentsToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            studentsToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            studentsToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            studentsToolbar.Items.Add(new ToolStripSeparator());
            studentsToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
            studentsToolbar.Items.Add(new ToolStripSeparator());
            studentsToolbar.Items.Add(new ToolStripLabel("Mã SV:"));
            txtStudentSearch = new ToolStripTextBox { AutoSize = false, Width = 120, ToolTipText = "Nhập Mã SV rồi Enter" };
            studentsToolbar.Items.Add(txtStudentSearch);
            var btnStudentSearch = new ToolStripButton("Tìm") { Tag = "search" };
            studentsToolbar.Items.Add(btnStudentSearch);
            dgvStudents = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tabStudents.Controls.Add(dgvStudents);
            tabStudents.Controls.Add(studentsToolbar);

            // Instructors tab
            instructorsToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            instructorsToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            instructorsToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            instructorsToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            instructorsToolbar.Items.Add(new ToolStripSeparator());
            instructorsToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
            dgvInstructors = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tabInstructors.Controls.Add(dgvInstructors);
            tabInstructors.Controls.Add(instructorsToolbar);

            // Courses tab
            coursesToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            coursesToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            coursesToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            coursesToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            coursesToolbar.Items.Add(new ToolStripSeparator());
            coursesToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
            dgvCourses = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tabCourses.Controls.Add(dgvCourses);
            tabCourses.Controls.Add(coursesToolbar);

            // Sections tab
            sectionsToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            sectionsToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            sectionsToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            sectionsToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            sectionsToolbar.Items.Add(new ToolStripSeparator());
            sectionsToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
            dgvSections = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tabSections.Controls.Add(dgvSections);
            tabSections.Controls.Add(sectionsToolbar);

            // Departments tab
            departmentsToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            departmentsToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            departmentsToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            departmentsToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            departmentsToolbar.Items.Add(new ToolStripSeparator());
            departmentsToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
            dgvDepartments = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tabDepartments.Controls.Add(dgvDepartments);
            tabDepartments.Controls.Add(departmentsToolbar);

            tabControl.TabPages.Add(tabStudents);
            tabControl.TabPages.Add(tabInstructors);
            tabControl.TabPages.Add(tabCourses);
            tabControl.TabPages.Add(tabSections);
            tabControl.TabPages.Add(tabDepartments);

            // Ẩn header tab – chỉ dùng sidebar để điều hướng
            HideTabHeaders();
            contentPanel.Controls.Add(tabControl);

            this.Controls.Add(contentPanel);
            this.Controls.Add(sidebarPanel);
            this.Controls.Add(headerPanel);
        }

        private void ApplyTheme()
        {
            ThemeHelper.ApplyButtonStyle(btnLogout, ThemeHelper.DangerRed, Color.White, 6);
            ThemeHelper.ApplyDataGridViewStyle(dgvStudents);
            ThemeHelper.ApplyDataGridViewStyle(dgvInstructors);
            ThemeHelper.ApplyButtonStyle(btnHelp, ThemeHelper.PrimaryBlue, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnNavStudents, ThemeHelper.SidebarActive, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnNavInstructors, ThemeHelper.PrimaryBlue, Color.White, 6);
            ThemeHelper.ApplyDataGridViewStyle(dgvCourses);
            ThemeHelper.ApplyDataGridViewStyle(dgvSections);
            ThemeHelper.ApplyDataGridViewStyle(dgvDepartments);
            ThemeHelper.ApplyButtonStyle(btnNavCourses, ThemeHelper.PrimaryBlue, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnNavSections, ThemeHelper.PrimaryBlue, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnNavDepartments, ThemeHelper.PrimaryBlue, Color.White, 6);
        }

        private void BindEvents()
        {
            // Căn phải nút Đăng xuất trong header như màn sinh viên
            headerPanel.Resize += (s, e) => LayoutHeaderButtons();
            LayoutHeaderButtons();

            btnLogout.Click += (s, e) =>
            {
                // Quay về LoginForm
                foreach (Form f in Application.OpenForms)
                {
                    if (f is LoginForm)
                    {
                        f.Show();
                        break;
                    }
                }
                this.Close();
            };
            btnHelp.Click += (s, e) => HelpLauncher.ShowHelp(this);

            btnNavStudents.Click += (s, e) =>
            {
                tabControl.SelectedTab = tabStudents;
                SetActiveSidebar(btnNavStudents);
            };

            btnNavInstructors.Click += (s, e) =>
            {
                tabControl.SelectedTab = tabInstructors;
                SetActiveSidebar(btnNavInstructors);
            };

            btnNavCourses.Click += (s, e) =>
            {
                tabControl.SelectedTab = tabCourses;
                SetActiveSidebar(btnNavCourses);
            };
            btnNavSections.Click += (s, e) =>
            {
                tabControl.SelectedTab = tabSections;
                SetActiveSidebar(btnNavSections);
            };
            btnNavDepartments.Click += (s, e) =>
            {
                tabControl.SelectedTab = tabDepartments;
                SetActiveSidebar(btnNavDepartments);
            };

            studentsToolbar.ItemClicked += (s, e) =>
            {
                switch (e.ClickedItem.Tag)
                {
                    case "add":
                        using (var dlg = new AddEditStudentForm())
                        {
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                if (DatabaseHelper.Instance.InsertStudent(dlg.Result)) LoadStudents();
                            }
                        }
                        break;
                    case "edit":
                        if (dgvStudents.CurrentRow != null)
                        {
                            var srow = dgvStudents.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Student;
                            if (srow != null)
                            {
                                using (var dlg = new AddEditStudentForm(srow))
                                {
                                    if (dlg.ShowDialog(this) == DialogResult.OK)
                                    {
                                        if (DatabaseHelper.Instance.UpdateStudent(dlg.Result)) LoadStudents();
                                    }
                                }
                            }
                        }
                        break;
                    case "delete":
                        if (dgvStudents.CurrentRow != null)
                        {
                            var srow = dgvStudents.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Student;
                            if (srow != null && MessageBox.Show("Xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (DatabaseHelper.Instance.DeleteStudent(srow.MaSV)) LoadStudents();
                            }
                        }
                        break;
                    case "refresh":
                        LoadStudents();
                        break;
                    case "search":
                        ApplyStudentSearch();
                        break;
                    default:
                        MessageBox.Show("Chức năng sẽ được bổ sung sau.");
                        break;
                }
            };

            instructorsToolbar.ItemClicked += (s, e) =>
            {
                switch (e.ClickedItem.Tag)
                {
                    case "add":
                        using (var dlg = new AddEditInstructorForm())
                        {
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                if (DatabaseHelper.Instance.InsertInstructor(dlg.Result)) LoadInstructors();
                            }
                        }
                        break;
                    case "edit":
                        if (dgvInstructors.CurrentRow != null)
                        {
                            var grow = dgvInstructors.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Instructor;
                            if (grow != null)
                            {
                                using (var dlg = new AddEditInstructorForm(grow))
                                {
                                    if (dlg.ShowDialog(this) == DialogResult.OK)
                                    {
                                        if (DatabaseHelper.Instance.UpdateInstructor(dlg.Result)) LoadInstructors();
                                    }
                                }
                            }
                        }
                        break;
                    case "delete":
                        if (dgvInstructors.CurrentRow != null)
                        {
                            var grow = dgvInstructors.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Instructor;
                            if (grow != null && MessageBox.Show("Xóa giảng viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (DatabaseHelper.Instance.DeleteInstructor(grow.MaGV)) LoadInstructors();
                            }
                        }
                        break;
                    case "refresh":
                        LoadInstructors();
                        break;
                    default:
                        MessageBox.Show("Chức năng sẽ được bổ sung sau.");
                        break;
                }
            };

            coursesToolbar.ItemClicked += (s, e) =>
            {
                switch (e.ClickedItem.Tag)
                {
                    case "add":
                        using (var dlg = new AddEditCourseForm())
                        {
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                if (DatabaseHelper.Instance.InsertCourse(dlg.Result)) LoadCourses();
                            }
                        }
                        break;
                    case "edit":
                        if (dgvCourses.CurrentRow != null)
                        {
                            var crow = dgvCourses.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Course;
                            if (crow != null)
                            {
                                using (var dlg = new AddEditCourseForm(crow))
                                {
                                    if (dlg.ShowDialog(this) == DialogResult.OK)
                                    {
                                        if (DatabaseHelper.Instance.UpdateCourse(dlg.Result)) LoadCourses();
                                    }
                                }
                            }
                        }
                        break;
                    case "delete":
                        if (dgvCourses.CurrentRow != null)
                        {
                            var crow = dgvCourses.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.Course;
                            if (crow != null && MessageBox.Show("Xóa môn học này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (DatabaseHelper.Instance.DeleteCourse(crow.MaMH)) LoadCourses();
                            }
                        }
                        break;
                    case "refresh":
                        LoadCourses();
                        break;
                }
            };

            sectionsToolbar.ItemClicked += (s, e) =>
            {
                switch (e.ClickedItem.Tag)
                {
                    case "add":
                        using (var dlg = new AddEditSectionForm())
                        {
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                if (DatabaseHelper.Instance.InsertSection(dlg.Result)) LoadSections();
                            }
                        }
                        break;
                    case "edit":
                        if (dgvSections.CurrentRow != null)
                        {
                            var lrow = dgvSections.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.CourseSection;
                            if (lrow != null)
                            {
                                using (var dlg = new AddEditSectionForm(lrow))
                                {
                                    if (dlg.ShowDialog(this) == DialogResult.OK)
                                    {
                                        if (DatabaseHelper.Instance.UpdateSection(dlg.Result)) LoadSections();
                                    }
                                }
                            }
                        }
                        break;
                    case "delete":
                        if (dgvSections.CurrentRow != null)
                        {
                            var lrow = dgvSections.CurrentRow.DataBoundItem as WindowsFormsApp1.Models.CourseSection;
                            if (lrow != null && MessageBox.Show("Xóa lớp học phần này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (DatabaseHelper.Instance.DeleteSection(lrow.MaLHP)) LoadSections();
                            }
                        }
                        break;
                    case "refresh":
                        LoadSections();
                        break;
                }
            };

            departmentsToolbar.ItemClicked += (s, e) =>
            {
                switch (e.ClickedItem.Tag)
                {
                    case "add":
                        using (var dlg = new AddEditDepartmentForm())
                        {
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                if (DatabaseHelper.Instance.InsertDepartment(dlg.ResultMa, dlg.ResultTen)) LoadDepartments();
                            }
                        }
                        break;
                    case "edit":
                        if (dgvDepartments.CurrentRow != null)
                        {
                            var ma = dgvDepartments.CurrentRow.Cells["MaKhoa"].Value?.ToString();
                            var ten = dgvDepartments.CurrentRow.Cells["TenKhoa"].Value?.ToString();
                            using (var dlg = new AddEditDepartmentForm(ma, ten))
                            {
                                if (dlg.ShowDialog(this) == DialogResult.OK)
                                {
                                    if (DatabaseHelper.Instance.UpdateDepartment(dlg.ResultMa, dlg.ResultTen)) LoadDepartments();
                                }
                            }
                        }
                        break;
                    case "delete":
                        if (dgvDepartments.CurrentRow != null)
                        {
                            var ma = dgvDepartments.CurrentRow.Cells["MaKhoa"].Value?.ToString();
                            if (!string.IsNullOrEmpty(ma) && MessageBox.Show("Xóa khoa này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (DatabaseHelper.Instance.DeleteDepartment(ma)) LoadDepartments();
                            }
                        }
                        break;
                    case "refresh":
                        LoadDepartments();
                        break;
                }
            };
        }

        private void LoadStudents()
        {
            var data = DatabaseHelper.Instance.GetAllStudents();
            allStudents = data;
            ConfigureStudentsGrid();
            dgvStudents.DataSource = allStudents;
        }

        private void LoadInstructors()
        {
            var data = DatabaseHelper.Instance.GetAllInstructors();
            ConfigureInstructorsGrid();
            dgvInstructors.DataSource = data;
        }

        private void LoadCourses()
        {
            var data = DatabaseHelper.Instance.GetAllCourses();
            ConfigureCoursesGrid();
            dgvCourses.DataSource = data;
        }

        private void LoadSections()
        {
            var data = DatabaseHelper.Instance.GetAllSections();
            ConfigureSectionsGrid();
            dgvSections.DataSource = data;
        }

        private void LoadDepartments()
        {
            var data = DatabaseHelper.Instance.GetAllDepartments();
            ConfigureDepartmentsGrid();
            dgvDepartments.DataSource = data;
        }

        private void SetActiveSidebar(Button active)
        {
            // Đổi màu active cho sidebar
            if (active == btnNavStudents)
            {
                btnNavStudents.BackColor = ThemeHelper.SidebarActive;
                btnNavInstructors.BackColor = ThemeHelper.PrimaryBlue;
            }
            else
            {
                btnNavStudents.BackColor = ThemeHelper.PrimaryBlue;
                btnNavInstructors.BackColor = ThemeHelper.SidebarActive;
            }
        }

        private void TryLoadLogo(PictureBox pictureBox)
        {
            try
            {
                string[] names = { "neu.png", "logo.png", "neu.jpg", "logo.jpg", "neu.ico", "logo.ico" };
                string basePath = Application.StartupPath;
                foreach (var n in names)
                {
                    string p1 = System.IO.Path.Combine(basePath, n);
                    string p2 = System.IO.Path.Combine(basePath, "Resources", n);
                    if (System.IO.File.Exists(p1)) { pictureBox.Image = Image.FromFile(p1); return; }
                    if (System.IO.File.Exists(p2)) { pictureBox.Image = Image.FromFile(p2); return; }
                }
            }
            catch { }
        }

        private void HideTabHeaders()
        {
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.Padding = new Point(0, 0);
        }

        private void ConfigureStudentsGrid()
        {
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.Columns.Clear();
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaSV", DataPropertyName = "MaSV" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TenSV", DataPropertyName = "TenSV" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "NgaySinh", DataPropertyName = "NgaySinh" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "GioiTinh", DataPropertyName = "GioiTinh" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SDT", DataPropertyName = "SDT" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = "Email" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DiaChi", DataPropertyName = "DiaChi" });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaCTDT", DataPropertyName = "MaCTDT" });
        }

        private void ConfigureInstructorsGrid()
        {
            dgvInstructors.AutoGenerateColumns = false;
            dgvInstructors.Columns.Clear();
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaGV", DataPropertyName = "MaGV" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TenGV", DataPropertyName = "TenGV" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "GioiTinh", DataPropertyName = "GioiTinh" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DiaChi", DataPropertyName = "DiaChi" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = "Email" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SDT", DataPropertyName = "SDT" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "HocVi", DataPropertyName = "HocVi" });
            dgvInstructors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaKV", DataPropertyName = "MaKV" });
        }

        private void ConfigureCoursesGrid()
        {
            dgvCourses.AutoGenerateColumns = false;
            dgvCourses.Columns.Clear();
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaMH", DataPropertyName = "MaMH" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TenHocPhan", DataPropertyName = "TenHocPhan" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SoTC", DataPropertyName = "SoTC" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaHocPhanTienQuyet", DataPropertyName = "MaHocPhanTienQuyet" });
            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaHocKi", DataPropertyName = "MaHocKi" });
        }

        private void ConfigureSectionsGrid()
        {
            dgvSections.AutoGenerateColumns = false;
            dgvSections.Columns.Clear();
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaLHP", DataPropertyName = "MaLHP" });
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TenLop", DataPropertyName = "TenLop" });
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaHP", DataPropertyName = "MaHP" });
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaGV", DataPropertyName = "MaGV" });
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SiSo", DataPropertyName = "SiSo" });
            dgvSections.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "LichHoc", DataPropertyName = "LichHoc" });
        }

        private void ConfigureDepartmentsGrid()
        {
            dgvDepartments.AutoGenerateColumns = false;
            dgvDepartments.Columns.Clear();
            dgvDepartments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "MaKhoa", DataPropertyName = "MaKhoa" });
            dgvDepartments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TenKhoa", DataPropertyName = "TenKhoa" });
        }

        private void ApplyStudentSearch()
        {
            if (allStudents == null)
            {
                LoadStudents();
            }

            var keyword = txtStudentSearch?.Text.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(keyword))
            {
                dgvStudents.DataSource = allStudents;
                return;
            }

            // Tìm theo Mã SV (chứa chuỗi nhập vào)
            var filtered = new List<Student>();
            foreach (var s in allStudents)
            {
                if (!string.IsNullOrEmpty(s.MaSV) &&
                    s.MaSV.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    filtered.Add(s);
                }
            }

            dgvStudents.DataSource = filtered;
        }

        private void LayoutHeaderButtons()
        {
            if (headerPanel == null || btnLogout == null || btnHelp == null) return;

            int padding = 16;
            btnLogout.Location = new Point(headerPanel.Width - btnLogout.Width - padding, 12);
            btnHelp.Location = new Point(btnLogout.Left - btnHelp.Width - 10, 12);
        }
    }
}


