using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AddEditInstructorForm : Form
    {
        private TextBox txtMaGV, txtTenGV, txtGioiTinh, txtDiaChi, txtEmail, txtSDT, txtHocVi, txtMaKV, txtPassword;
        private Button btnOK, btnCancel;
        public Instructor Result { get; private set; }

        public AddEditInstructorForm(Instructor existing = null)
        {
            this.Text = existing == null ? "Thêm giảng viên" : "Sửa giảng viên";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(520, 400);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 10, Padding = new Padding(16) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 9; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            txtMaGV = new TextBox();
            txtTenGV = new TextBox();
            txtGioiTinh = new TextBox();
            txtDiaChi = new TextBox();
            txtEmail = new TextBox();
            txtSDT = new TextBox();
            txtHocVi = new TextBox();
            txtMaKV = new TextBox();
            txtPassword = new TextBox();

            AddRow(tlp, "Mã GV", txtMaGV, 0);
            AddRow(tlp, "Họ tên", txtTenGV, 1);
            AddRow(tlp, "Giới tính", txtGioiTinh, 2);
            AddRow(tlp, "Địa chỉ", txtDiaChi, 3);
            AddRow(tlp, "Email", txtEmail, 4);
            AddRow(tlp, "SĐT", txtSDT, 5);
            AddRow(tlp, "Học vị", txtHocVi, 6);
            AddRow(tlp, "Mã khoa viện", txtMaKV, 7);
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
                txtMaGV.Text = existing.MaGV; txtMaGV.Enabled = false;
                txtTenGV.Text = existing.TenGV;
                txtGioiTinh.Text = existing.GioiTinh;
                txtDiaChi.Text = existing.DiaChi;
                txtEmail.Text = existing.Email;
                txtSDT.Text = existing.SDT;
                txtHocVi.Text = existing.HocVi;
                txtMaKV.Text = existing.MaKV;
                txtPassword.Text = existing.Password;
            }

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaGV.Text) || string.IsNullOrWhiteSpace(txtTenGV.Text) || string.IsNullOrWhiteSpace(txtMaKV.Text))
                {
                    MessageBox.Show("Mã GV, Họ tên, Mã khoa viện là bắt buộc.");
                    return;
                }
                Result = new Instructor
                {
                    MaGV = txtMaGV.Text.Trim(),
                    TenGV = txtTenGV.Text.Trim(),
                    GioiTinh = txtGioiTinh.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    HocVi = txtHocVi.Text.Trim(),
                    MaKV = txtMaKV.Text.Trim(),
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


