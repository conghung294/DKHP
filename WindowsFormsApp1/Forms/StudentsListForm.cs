using System;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class StudentsListForm : Form
    {
        private string maLopHocPhan;
        private System.Windows.Forms.DataGridView dgvStudents;

        public StudentsListForm(string maLHP, System.Collections.Generic.List<Student> students)
        {
            maLopHocPhan = maLHP;
            InitializeComponent();
            LoadStudents(students);
        }

        private void LoadStudents(System.Collections.Generic.List<Student> students)
        {
            dgvStudents.Rows.Clear();
            foreach (var student in students)
            {
                dgvStudents.Rows.Add(
                    student.MaSV,
                    student.TenSV,
                    student.NgaySinh.ToString("dd/MM/yyyy"),
                    student.GioiTinh,
                    student.Email,
                    student.SDT
                );
            }

            lblCount.Text = $"Tổng số sinh viên: {students.Count}";
        }

        private void InitializeComponent()
        {
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();
            this.dgvStudents.Columns.Add("MaSV", "Mã SV");
            this.dgvStudents.Columns.Add("HoTen", "Họ tên");
            this.dgvStudents.Columns.Add("NgaySinh", "Ngày sinh");
            this.dgvStudents.Columns.Add("GioiTinh", "Giới tính");
            this.dgvStudents.Columns.Add("Email", "Email");
            this.dgvStudents.Columns.Add("SDT", "SĐT");
            // 
            // dgvStudents
            // 
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Location = new System.Drawing.Point(20, 60);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersWidth = 51;
            this.dgvStudents.Size = new System.Drawing.Size(900, 400);
            this.dgvStudents.TabIndex = 0;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.Location = new System.Drawing.Point(20, 20);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(160, 25);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "Tổng số sinh viên:";
            // 
            // StudentsListForm
            // 
            this.ClientSize = new System.Drawing.Size(950, 500);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.dgvStudents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StudentsListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách sinh viên";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCount;
    }
}

