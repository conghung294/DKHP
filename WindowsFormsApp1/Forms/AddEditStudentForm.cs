using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AddEditStudentForm : Form
    {
        private TextBox txtMaSV, txtTenSV, txtNgaySinh, txtGioiTinh, txtSDT, txtEmail, txtDiaChi, txtMaCTDT, txtPassword;
        private Button btnOK, btnCancel;
        public Student Result { get; private set; }

        public AddEditStudentForm(Student existing = null)
        {
            this.Text = existing == null ? "Thêm sinh viên" : "Sửa sinh viên";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(520, 420);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 10, Padding = new Padding(16) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 9; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            txtMaSV = new TextBox();
            txtTenSV = new TextBox();
            txtNgaySinh = new TextBox();
            txtGioiTinh = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            txtDiaChi = new TextBox();
            txtMaCTDT = new TextBox();
            txtPassword = new TextBox();

            AddRow(tlp, "Mã SV", txtMaSV, 0);
            AddRow(tlp, "Họ tên", txtTenSV, 1);
            AddRow(tlp, "Ngày sinh (yyyy-MM-dd)", txtNgaySinh, 2);
            AddRow(tlp, "Giới tính", txtGioiTinh, 3);
            AddRow(tlp, "SĐT", txtSDT, 4);
            AddRow(tlp, "Email", txtEmail, 5);
            AddRow(tlp, "Địa chỉ", txtDiaChi, 6);
            AddRow(tlp, "Mã CTDT", txtMaCTDT, 7);
            AddRow(tlp, "Mật khẩu", txtPassword, 8);

            var panelButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            btnOK = new Button { Text = "Lưu", Width = 100, Height = 32 };
            btnCancel = new Button { Text = "Hủy", Width = 100, Height = 32 };
            ThemeHelper.ApplyButtonStyle(btnOK, ThemeHelper.SuccessGreen, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.DangerRed, Color.White, 6);
            panelButtons.Controls.Add(btnOK);
            panelButtons.Controls.Add(btnCancel);
            tlp.Controls.Add(panelButtons, 0, 9);
            tlp.SetColumnSpan(panelButtons, 2);

            this.Controls.Add(tlp);

            if (existing != null)
            {
                txtMaSV.Text = existing.MaSV; txtMaSV.Enabled = false;
                txtTenSV.Text = existing.TenSV;
                txtNgaySinh.Text = existing.NgaySinh.ToString("yyyy-MM-dd");
                txtGioiTinh.Text = existing.GioiTinh;
                txtSDT.Text = existing.SDT;
                txtEmail.Text = existing.Email;
                txtDiaChi.Text = existing.DiaChi;
                txtMaCTDT.Text = existing.MaCTDT;
                txtPassword.Text = existing.Password;
            }

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text) || string.IsNullOrWhiteSpace(txtMaCTDT.Text))
                {
                    MessageBox.Show("Mã SV, Họ tên, Mã CTDT là bắt buộc.");
                    return;
                }
                DateTime dob;
                if (!DateTime.TryParse(txtNgaySinh.Text, out dob))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ.");
                    return;
                }
                Result = new Student
                {
                    MaSV = txtMaSV.Text.Trim(),
                    TenSV = txtTenSV.Text.Trim(),
                    NgaySinh = dob,
                    GioiTinh = txtGioiTinh.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    MaCTDT = txtMaCTDT.Text.Trim(),
                    Password = txtPassword.Text
                };
                this.DialogResult = DialogResult.OK;
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void AddRow(TableLayoutPanel tlp, string label, Control input, int row)
        {
            var lbl = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Font = ThemeHelper.LabelFont, ForeColor = ThemeHelper.TextDark };
            ThemeHelper.ApplyTextBoxStyle(input as TextBox);
            tlp.Controls.Add(lbl, 0, row);
            tlp.Controls.Add(input, 1, row);
            input.Dock = DockStyle.Fill;
        }
    }
}


