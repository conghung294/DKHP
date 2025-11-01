using System;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class GradesViewForm : Form
    {
        private string maSinhVien;
        private System.Windows.Forms.DataGridView dgvGrades;

        public GradesViewForm(string maSV, System.Collections.Generic.List<Grade> grades)
        {
            maSinhVien = maSV;
            InitializeComponent();
            LoadGrades(grades);
        }

        private void LoadGrades(System.Collections.Generic.List<Grade> grades)
        {
            dgvGrades.Rows.Clear();
            foreach (var grade in grades)
            {
                dgvGrades.Rows.Add(
                    grade.TenHocPhan,
                    grade.TenLop,
                    grade.DiemChuyenCan?.ToString("F2") ?? "",
                    grade.DiemGiuaKy?.ToString("F2") ?? "",
                    grade.DiemThi?.ToString("F2") ?? "",
                    grade.DiemTongKet?.ToString("F2") ?? "",
                    GetGradeLetter(grade.DiemTongKet)
                );
            }
        }

        private string GetGradeLetter(double? diemTongKet)
        {
            if (!diemTongKet.HasValue)
                return "";
            
            double diem = diemTongKet.Value;
            if (diem >= 9.0) return "A+";
            if (diem >= 8.5) return "A";
            if (diem >= 8.0) return "B+";
            if (diem >= 7.0) return "B";
            if (diem >= 6.5) return "C+";
            if (diem >= 5.5) return "C";
            if (diem >= 5.0) return "D";
            return "F";
        }

        private void InitializeComponent()
        {
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.SuspendLayout();
            this.dgvGrades.Columns.Add("TenHP", "Tên học phần");
            this.dgvGrades.Columns.Add("LopHP", "Lớp học phần");
            this.dgvGrades.Columns.Add("DiemCC", "Điểm chuyên cần");
            this.dgvGrades.Columns.Add("DiemGK", "Điểm giữa kỳ");
            this.dgvGrades.Columns.Add("DiemThi", "Điểm thi");
            this.dgvGrades.Columns.Add("DiemTK", "Điểm tổng kết");
            this.dgvGrades.Columns.Add("ChuCai", "Chữ cái");
            // 
            // dgvGrades
            // 
            this.dgvGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrades.Location = new System.Drawing.Point(0, 0);
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.ReadOnly = true;
            this.dgvGrades.RowHeadersWidth = 51;
            this.dgvGrades.Size = new System.Drawing.Size(900, 500);
            this.dgvGrades.TabIndex = 0;
            // 
            // GradesViewForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.dgvGrades);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GradesViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bảng điểm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

