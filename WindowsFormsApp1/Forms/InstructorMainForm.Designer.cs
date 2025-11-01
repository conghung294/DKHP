namespace WindowsFormsApp1.Forms
{
    partial class InstructorMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TextBox txtInstructorInfo;
        private System.Windows.Forms.DataGridView dgvMyCourses;
        private System.Windows.Forms.Button btnViewStudents;
        private System.Windows.Forms.Button btnEnterGrades;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblMyCourses;

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
            this.txtInstructorInfo = new System.Windows.Forms.TextBox();
            this.lblMyCourses = new System.Windows.Forms.Label();
            this.dgvMyCourses = new System.Windows.Forms.DataGridView();
            this.btnViewStudents = new System.Windows.Forms.Button();
            this.btnEnterGrades = new System.Windows.Forms.Button();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCourses)).BeginInit();
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
            // txtInstructorInfo
            // 
            this.txtInstructorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInstructorInfo.Location = new System.Drawing.Point(20, 60);
            this.txtInstructorInfo.Multiline = true;
            this.txtInstructorInfo.Name = "txtInstructorInfo";
            this.txtInstructorInfo.ReadOnly = true;
            this.txtInstructorInfo.Size = new System.Drawing.Size(350, 120);
            this.txtInstructorInfo.TabIndex = 1;
            // 
            // lblMyCourses
            // 
            this.lblMyCourses.AutoSize = true;
            this.lblMyCourses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMyCourses.Location = new System.Drawing.Point(20, 200);
            this.lblMyCourses.Name = "lblMyCourses";
            this.lblMyCourses.Size = new System.Drawing.Size(230, 25);
            this.lblMyCourses.TabIndex = 2;
            this.lblMyCourses.Text = "Các lớp đang giảng dạy:";
            // 
            // dgvMyCourses
            // 
            this.dgvMyCourses.AllowUserToAddRows = false;
            this.dgvMyCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMyCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyCourses.Location = new System.Drawing.Point(20, 240);
            this.dgvMyCourses.Name = "dgvMyCourses";
            this.dgvMyCourses.ReadOnly = true;
            this.dgvMyCourses.RowHeadersWidth = 51;
            this.dgvMyCourses.RowTemplate.Height = 24;
            this.dgvMyCourses.Size = new System.Drawing.Size(1200, 400);
            this.dgvMyCourses.TabIndex = 3;
            this.dgvMyCourses.Columns.Add("MaLHP", "Mã lớp HP");
            this.dgvMyCourses.Columns.Add("TenHP", "Tên học phần");
            this.dgvMyCourses.Columns.Add("TenLop", "Lớp");
            this.dgvMyCourses.Columns.Add("LichHoc", "Lịch học");
            this.dgvMyCourses.Columns.Add("LoaiMon", "Loại môn");
            this.dgvMyCourses.Columns.Add("SiSo", "Sĩ số");
            // 
            // btnViewStudents
            // 
            this.btnViewStudents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnViewStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewStudents.ForeColor = System.Drawing.Color.White;
            this.btnViewStudents.Location = new System.Drawing.Point(400, 680);
            this.btnViewStudents.Name = "btnViewStudents";
            this.btnViewStudents.Size = new System.Drawing.Size(200, 45);
            this.btnViewStudents.TabIndex = 4;
            this.btnViewStudents.Text = "Xem danh sách sinh viên";
            this.btnViewStudents.UseVisualStyleBackColor = false;
            this.btnViewStudents.Click += new System.EventHandler(this.btnViewStudents_Click);
            // 
            // btnEnterGrades
            // 
            this.btnEnterGrades.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(136)))), ((int)(((byte)(0)))));
            this.btnEnterGrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterGrades.ForeColor = System.Drawing.Color.White;
            this.btnEnterGrades.Location = new System.Drawing.Point(620, 680);
            this.btnEnterGrades.Name = "btnEnterGrades";
            this.btnEnterGrades.Size = new System.Drawing.Size(200, 45);
            this.btnEnterGrades.TabIndex = 5;
            this.btnEnterGrades.Text = "Nhập điểm";
            this.btnEnterGrades.UseVisualStyleBackColor = false;
            this.btnEnterGrades.Click += new System.EventHandler(this.btnEnterGrades_Click);
            // 
            // btnSchedule
            // 
            this.btnSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.btnSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchedule.Location = new System.Drawing.Point(840, 680);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(150, 45);
            this.btnSchedule.TabIndex = 6;
            this.btnSchedule.Text = "Lịch dạy";
            this.btnSchedule.UseVisualStyleBackColor = false;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Gray;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(1010, 680);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(150, 45);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // InstructorMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSchedule);
            this.Controls.Add(this.btnEnterGrades);
            this.Controls.Add(this.btnViewStudents);
            this.Controls.Add(this.dgvMyCourses);
            this.Controls.Add(this.lblMyCourses);
            this.Controls.Add(this.txtInstructorInfo);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InstructorMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giảng viên - Quản lý lớp học";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCourses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

