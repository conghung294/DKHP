using System;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class EnterGradesForm : Form
    {
        private string maLopHocPhan;
        private System.Collections.Generic.List<Student> students;
        private DatabaseHelper db;
        private System.Windows.Forms.DataGridView dgvGrades;
        private System.Windows.Forms.Button btnSave;

        public EnterGradesForm(string maLHP, System.Collections.Generic.List<Student> students)
        {
            maLopHocPhan = maLHP;
            this.students = students;
            db = DatabaseHelper.Instance;
            InitializeComponent();
            LoadGradesData();
            
            // Attach event handler to calculate final grade when values change
            dgvGrades.CellValueChanged += dgvGrades_CellValueChanged;
        }

        private void dgvGrades_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4) // CC, GK, or Thi columns
            {
                var row = dgvGrades.Rows[e.RowIndex];
                double? diemCC = ParseGrade(row.Cells[2].Value);
                double? diemGK = ParseGrade(row.Cells[3].Value);
                double? diemThi = ParseGrade(row.Cells[4].Value);

                // Calculate final grade
                if (diemCC.HasValue && diemGK.HasValue && diemThi.HasValue)
                {
                    double diemTK = (diemCC.Value * 0.1) + (diemGK.Value * 0.3) + (diemThi.Value * 0.6);
                    row.Cells[5].Value = Math.Round(diemTK, 2);
                }
            }
        }

        private void LoadGradesData()
        {
            dgvGrades.Rows.Clear();
            foreach (var student in students)
            {
                // TODO: Load grades từ database khi có bảng Diem
                // Hiện tại schema không có bảng Diem, tạm thời để null
                dgvGrades.Rows.Add(
                    student.MaSV,
                    student.TenSV,
                    null, // DiemChuyenCan
                    null, // DiemGiuaKy
                    null, // DiemThi
                    null, // DiemTongKet
                    maLopHocPhan
                );
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvGrades.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string maSV = row.Cells[0].Value.ToString();
                    double? diemCC = ParseGrade(row.Cells[2].Value);
                    double? diemGK = ParseGrade(row.Cells[3].Value);
                    double? diemThi = ParseGrade(row.Cells[4].Value);

                    db.SaveGrades(maSV, maLopHocPhan, diemCC, diemGK, diemThi);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private double? ParseGrade(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            if (double.TryParse(value.ToString(), out double result))
                return result;

            return null;
        }

        private void InitializeComponent()
        {
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvGrades.Columns.Add("MaSV", "Mã SV");
            this.dgvGrades.Columns.Add("HoTen", "Họ tên");
            this.dgvGrades.Columns.Add("DiemCC", "Điểm chuyên cần");
            this.dgvGrades.Columns.Add("DiemGK", "Điểm giữa kỳ");
            this.dgvGrades.Columns.Add("DiemThi", "Điểm thi");
            this.dgvGrades.Columns.Add("DiemTK", "Điểm tổng kết");
            this.dgvGrades.Columns.Add("MaLHP", "Mã lớp HP");
            this.dgvGrades.Columns[6].Visible = false; // Hide MLHP column
            this.dgvGrades.Columns[5].ReadOnly = true; // Điểm TK là read-only
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvGrades
            // 
            this.dgvGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrades.Location = new System.Drawing.Point(20, 20);
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.RowHeadersWidth = 51;
            this.dgvGrades.Size = new System.Drawing.Size(1100, 450);
            this.dgvGrades.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(500, 500);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 45);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu điểm";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // EnterGradesForm
            // 
            this.ClientSize = new System.Drawing.Size(1150, 570);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvGrades);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnterGradesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập điểm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

