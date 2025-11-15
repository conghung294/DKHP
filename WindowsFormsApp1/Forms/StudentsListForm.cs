using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
    public partial class StudentsListForm : Form
    {
        private string maLopHocPhan;
        private List<Student> studentsList;
        private DatabaseHelper db;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Button btnExportPDF;

        public StudentsListForm(string maLHP, List<Student> students)
        {
            maLopHocPhan = maLHP;
            studentsList = students;
            db = DatabaseHelper.Instance;
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
            this.btnExportPDF = new System.Windows.Forms.Button();
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
            this.dgvStudents.Size = new System.Drawing.Size(900, 380);
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
            // btnExportPDF
            // 
            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnExportPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(20, 450);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(150, 35);
            this.btnExportPDF.TabIndex = 2;
            this.btnExportPDF.Text = "📄 Xuất PDF";
            this.btnExportPDF.UseVisualStyleBackColor = false;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // StudentsListForm
            // 
            this.ClientSize = new System.Drawing.Size(950, 500);
            this.Controls.Add(this.btnExportPDF);
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

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (studentsList == null || studentsList.Count == 0)
                {
                    MessageBox.Show("Không có sinh viên nào để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var sectionInfo = db.GetSectionInfo(maLopHocPhan);
                if (sectionInfo == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin lớp học phần!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"DanhSachSinhVien_{maLopHocPhan}_{DateTime.Now:yyyyMMdd}.pdf";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportStudentsToPdf(studentsList, sectionInfo, saveDialog.FileName);
                    MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportStudentsToPdf(List<Student> students, CourseSection sectionInfo, string filePath)
        {
            // Tạo file HTML với bảng danh sách sinh viên
            string htmlContent = GenerateStudentsHtmlReport(students, sectionInfo);
            string htmlPath = Path.ChangeExtension(filePath, ".html");
            File.WriteAllText(htmlPath, htmlContent, System.Text.Encoding.UTF8);
        }

        private string GenerateStudentsHtmlReport(List<Student> students, CourseSection sectionInfo)
        {
            string html = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Danh sách sinh viên lớp học phần</title>
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
        <h2>DANH SÁCH SINH VIÊN LỚP HỌC PHẦN</h2>
    </div>
    
    <div class='info'>
        <table>
            <tr>
                <td>Mã lớp học phần:</td>
                <td>{sectionInfo.MaLHP}</td>
                <td>Tên học phần:</td>
                <td>{sectionInfo.TenHocPhan}</td>
            </tr>
            <tr>
                <td>Lớp:</td>
                <td>{sectionInfo.TenLop}</td>
                <td>Giảng viên:</td>
                <td>{sectionInfo.TenGiangVien}</td>
            </tr>
            <tr>
                <td>Lịch học:</td>
                <td>{sectionInfo.LichHoc ?? ""}</td>
                <td>Ngày xuất:</td>
                <td>{DateTime.Now:dd/MM/yyyy HH:mm}</td>
            </tr>
            <tr>
                <td>Sĩ số:</td>
                <td>{students.Count}/{sectionInfo.SiSo}</td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
    
    <table>
        <thead>
            <tr>
                <th>STT</th>
                <th>Mã sinh viên</th>
                <th>Họ và tên</th>
                <th>Ngày sinh</th>
                <th>Giới tính</th>
                <th>Email</th>
                <th>SĐT</th>
            </tr>
        </thead>
        <tbody>";
            
            int stt = 1;
            foreach (var student in students)
            {
                html += $@"
            <tr>
                <td>{stt++}</td>
                <td>{student.MaSV}</td>
                <td>{student.TenSV}</td>
                <td>{student.NgaySinh:dd/MM/yyyy}</td>
                <td>{student.GioiTinh ?? ""}</td>
                <td>{student.Email ?? ""}</td>
                <td>{student.SDT ?? ""}</td>
            </tr>";
            }
            
            html += $@"
            <tr class='total'>
                <td colspan='2' style='text-align: right;'><strong>Tổng số sinh viên:</strong></td>
                <td><strong>{students.Count}</strong></td>
                <td colspan='4'></td>
            </tr>
        </tbody>
    </table>
    
    <div class='footer'>
        <p>Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}</p>
        <p style='margin-top: 50px;'><strong>Giảng viên</strong></p>
        <p style='margin-top: 30px;'><strong>{sectionInfo.TenGiangVien}</strong></p>
    </div>
</body>
</html>";
            
            return html;
        }
    }
}

