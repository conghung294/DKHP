using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AdminMainForm : Form
    {
        private Panel headerPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Label lblTitle;
        private Button btnLogout;
        private TabControl tabControl;
        private TabPage tabStudents;
        private TabPage tabInstructors;
        private DataGridView dgvStudents;
        private DataGridView dgvInstructors;
        private ToolStrip studentsToolbar;
        private ToolStrip instructorsToolbar;
        private Button btnNavStudents;
        private Button btnNavInstructors;
        private PictureBox picLogo;

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
        }

        private void CreateLayout()
        {
            headerPanel = new Panel { Height = 56, Dock = DockStyle.Top, BackColor = ThemeHelper.HeaderBlue };
            lblTitle = new Label { Text = "Quản trị hệ thống - " + adminName, AutoSize = true, ForeColor = Color.White, Location = new Point(16, 16), Font = ThemeHelper.HeaderFont };
            btnLogout = new Button { Text = "Đăng xuất", Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(110, 32) };
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnLogout);

            sidebarPanel = new Panel { Width = 220, Dock = DockStyle.Left, BackColor = ThemeHelper.SidebarBackground };

            // Sidebar content: logo + nav buttons
            picLogo = new PictureBox { Size = new Size(64, 64), Location = new Point(16, 16), SizeMode = PictureBoxSizeMode.Zoom };
            TryLoadLogo(picLogo);
            var lblApp = new Label { Text = "Admin", AutoSize = true, Font = ThemeHelper.SubHeaderFont, ForeColor = ThemeHelper.TextDark, Location = new Point(96, 36) };

            btnNavStudents = new Button { Text = "Quản lý sinh viên", Location = new Point(12, 100), Size = new Size(196, 40) };
            btnNavInstructors = new Button { Text = "Quản lý giảng viên", Location = new Point(12, 148), Size = new Size(196, 40) };

            sidebarPanel.Controls.Add(picLogo);
            sidebarPanel.Controls.Add(lblApp);
            sidebarPanel.Controls.Add(btnNavStudents);
            sidebarPanel.Controls.Add(btnNavInstructors);

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = ThemeHelper.BackgroundLight };

            tabControl = new TabControl { Dock = DockStyle.Fill };
            tabStudents = new TabPage("Quản lý sinh viên");
            tabInstructors = new TabPage("Quản lý giảng viên");

            // Students tab
            studentsToolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };
            studentsToolbar.Items.Add(new ToolStripButton("Thêm") { Tag = "add" });
            studentsToolbar.Items.Add(new ToolStripButton("Sửa") { Tag = "edit" });
            studentsToolbar.Items.Add(new ToolStripButton("Xóa") { Tag = "delete" });
            studentsToolbar.Items.Add(new ToolStripSeparator());
            studentsToolbar.Items.Add(new ToolStripButton("Tải lại") { Tag = "refresh" });
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

            tabControl.TabPages.Add(tabStudents);
            tabControl.TabPages.Add(tabInstructors);

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
            ThemeHelper.ApplyButtonStyle(btnNavStudents, ThemeHelper.SidebarActive, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnNavInstructors, ThemeHelper.PrimaryBlue, Color.White, 6);
        }

        private void BindEvents()
        {
            // Căn phải nút Đăng xuất trong header như màn sinh viên
            headerPanel.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(headerPanel.Width - btnLogout.Width - 16, 12);
            };
            // Gọi một lần để đặt vị trí ban đầu
            headerPanel.PerformLayout();
            btnLogout.Location = new Point(headerPanel.Width - btnLogout.Width - 16, 12);

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
        }

        private void LoadStudents()
        {
            var data = DatabaseHelper.Instance.GetAllStudents();
            ConfigureStudentsGrid();
            dgvStudents.DataSource = data;
        }

        private void LoadInstructors()
        {
            var data = DatabaseHelper.Instance.GetAllInstructors();
            ConfigureInstructorsGrid();
            dgvInstructors.DataSource = data;
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
    }
}


