namespace WindowsFormsApp1.Forms
{
    partial class StudentMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TextBox txtStudentInfo;
        private System.Windows.Forms.DataGridView dgvRegisteredCourses;
        private System.Windows.Forms.DataGridView dgvAvailableCourses;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabRegistered;
        private System.Windows.Forms.TabPage tabAvailable;
        private System.Windows.Forms.Label lblRegisteredTitle;
        private System.Windows.Forms.Label lblAvailableTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.txtStudentInfo = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabRegistered = new System.Windows.Forms.TabPage();
            this.lblRegisteredTitle = new System.Windows.Forms.Label();
            this.dgvRegisteredCourses = new System.Windows.Forms.DataGridView();
            this.tabAvailable = new System.Windows.Forms.TabPage();
            this.lblAvailableTitle = new System.Windows.Forms.Label();
            this.dgvAvailableCourses = new System.Windows.Forms.DataGridView();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabRegistered.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisteredCourses)).BeginInit();
            this.tabAvailable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableCourses)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(120, 29);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Xin chào";
            // 
            // txtStudentInfo
            // 
            this.txtStudentInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStudentInfo.Location = new System.Drawing.Point(20, 60);
            this.txtStudentInfo.Multiline = true;
            this.txtStudentInfo.Name = "txtStudentInfo";
            this.txtStudentInfo.ReadOnly = true;
            this.txtStudentInfo.Size = new System.Drawing.Size(350, 120);
            this.txtStudentInfo.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabRegistered);
            this.tabControl.Controls.Add(this.tabAvailable);
            this.tabControl.Location = new System.Drawing.Point(20, 200);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1200, 450);
            this.tabControl.TabIndex = 2;
            // 
            // tabRegistered
            // 
            this.tabRegistered.Controls.Add(this.lblRegisteredTitle);
            this.tabRegistered.Controls.Add(this.dgvRegisteredCourses);
            this.tabRegistered.Location = new System.Drawing.Point(4, 25);
            this.tabRegistered.Name = "tabRegistered";
            this.tabRegistered.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegistered.Size = new System.Drawing.Size(1192, 421);
            this.tabRegistered.TabIndex = 0;
            this.tabRegistered.Text = "Lớp đã đăng ký";
            this.tabRegistered.UseVisualStyleBackColor = true;
            // 
            // lblRegisteredTitle
            // 
            this.lblRegisteredTitle.AutoSize = true;
            this.lblRegisteredTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegisteredTitle.Location = new System.Drawing.Point(10, 10);
            this.lblRegisteredTitle.Name = "lblRegisteredTitle";
            this.lblRegisteredTitle.Size = new System.Drawing.Size(210, 25);
            this.lblRegisteredTitle.TabIndex = 0;
            this.lblRegisteredTitle.Text = "Các lớp đã đăng ký:";
            // 
            // dgvRegisteredCourses
            // 
            this.dgvRegisteredCourses.AllowUserToAddRows = false;
            this.dgvRegisteredCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRegisteredCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegisteredCourses.Location = new System.Drawing.Point(10, 50);
            this.dgvRegisteredCourses.Name = "dgvRegisteredCourses";
            this.dgvRegisteredCourses.ReadOnly = true;
            this.dgvRegisteredCourses.RowHeadersWidth = 51;
            this.dgvRegisteredCourses.RowTemplate.Height = 24;
            this.dgvRegisteredCourses.Size = new System.Drawing.Size(1172, 360);
            this.dgvRegisteredCourses.TabIndex = 1;
            this.dgvRegisteredCourses.Columns.Add("MaLHP", "Mã lớp HP");
            this.dgvRegisteredCourses.Columns.Add("TenHP", "Tên học phần");
            this.dgvRegisteredCourses.Columns.Add("TenLop", "Lớp");
            this.dgvRegisteredCourses.Columns.Add("LichHoc", "Lịch học");
            this.dgvRegisteredCourses.Columns.Add("LoaiMon", "Loại môn");
            this.dgvRegisteredCourses.Columns.Add("SiSo", "Sĩ số");
            // 
            // tabAvailable
            // 
            this.tabAvailable.Controls.Add(this.lblAvailableTitle);
            this.tabAvailable.Controls.Add(this.dgvAvailableCourses);
            this.tabAvailable.Location = new System.Drawing.Point(4, 25);
            this.tabAvailable.Name = "tabAvailable";
            this.tabAvailable.Padding = new System.Windows.Forms.Padding(3);
            this.tabAvailable.Size = new System.Drawing.Size(1192, 421);
            this.tabAvailable.TabIndex = 1;
            this.tabAvailable.Text = "Lớp có thể đăng ký";
            this.tabAvailable.UseVisualStyleBackColor = true;
            // 
            // lblAvailableTitle
            // 
            this.lblAvailableTitle.AutoSize = true;
            this.lblAvailableTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableTitle.Location = new System.Drawing.Point(10, 10);
            this.lblAvailableTitle.Name = "lblAvailableTitle";
            this.lblAvailableTitle.Size = new System.Drawing.Size(230, 25);
            this.lblAvailableTitle.TabIndex = 0;
            this.lblAvailableTitle.Text = "Các lớp có thể đăng ký:";
            // 
            // dgvAvailableCourses
            // 
            this.dgvAvailableCourses.AllowUserToAddRows = false;
            this.dgvAvailableCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAvailableCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableCourses.Location = new System.Drawing.Point(10, 50);
            this.dgvAvailableCourses.Name = "dgvAvailableCourses";
            this.dgvAvailableCourses.ReadOnly = true;
            this.dgvAvailableCourses.RowHeadersWidth = 51;
            this.dgvAvailableCourses.RowTemplate.Height = 24;
            this.dgvAvailableCourses.Size = new System.Drawing.Size(1172, 360);
            this.dgvAvailableCourses.TabIndex = 1;
            this.dgvAvailableCourses.Columns.Add("MaLHP", "Mã lớp HP");
            this.dgvAvailableCourses.Columns.Add("MaHP", "Mã HP");
            this.dgvAvailableCourses.Columns.Add("TenHP", "Tên học phần");
            this.dgvAvailableCourses.Columns.Add("SoTC", "Tín chỉ");
            this.dgvAvailableCourses.Columns.Add("MonTienQuyet", "Môn tiên quyết");
            this.dgvAvailableCourses.Columns.Add("TenLop", "Lớp");
            this.dgvAvailableCourses.Columns.Add("LichHoc", "Lịch học");
            this.dgvAvailableCourses.Columns.Add("GiangVien", "Giảng viên");
            this.dgvAvailableCourses.Columns.Add("SiSo", "Sĩ số");
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(400, 680);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(150, 40);
            this.btnRegister.TabIndex = 3;
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(570, 680);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 40);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Hủy đăng ký";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSchedule
            // 
            this.btnSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchedule.Location = new System.Drawing.Point(740, 680);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(150, 40);
            this.btnSchedule.TabIndex = 5;
            this.btnSchedule.Text = "Xem lịch học";
            this.btnSchedule.UseVisualStyleBackColor = false;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Gray;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(910, 680);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(140, 40);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // StudentMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSchedule);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.txtStudentInfo);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "StudentMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sinh viên - Đăng ký học phần";
            this.tabControl.ResumeLayout(false);
            this.tabRegistered.ResumeLayout(false);
            this.tabRegistered.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisteredCourses)).EndInit();
            this.tabAvailable.ResumeLayout(false);
            this.tabAvailable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableCourses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

