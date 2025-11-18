using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AddEditSectionForm : Form
    {
        private TextBox txtMaLHP, txtTenLop, txtMaHP, txtMaGV, txtSiSo, txtLichHoc;
        private Button btnOK, btnCancel;
        public CourseSection Result { get; private set; }

        public AddEditSectionForm(CourseSection existing = null)
        {
            this.Text = existing == null ? "Thêm lớp học phần" : "Sửa lớp học phần";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.ClientSize = new Size(520, 340);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 7, Padding = new Padding(16) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 6; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            txtMaLHP = new TextBox(); txtTenLop = new TextBox(); txtMaHP = new TextBox(); txtMaGV = new TextBox(); txtSiSo = new TextBox(); txtLichHoc = new TextBox();
            AddRow(tlp, "Mã LHP", txtMaLHP, 0);
            AddRow(tlp, "Tên lớp", txtTenLop, 1);
            AddRow(tlp, "Mã học phần", txtMaHP, 2);
            AddRow(tlp, "Mã giảng viên", txtMaGV, 3);
            AddRow(tlp, "Sĩ số", txtSiSo, 4);
            AddRow(tlp, "Lịch học", txtLichHoc, 5);

            var panelButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            btnOK = new Button { Text = "Lưu", Width = 100, Height = 32 };
            btnCancel = new Button { Text = "Hủy", Width = 100, Height = 32 };
            ThemeHelper.ApplyButtonStyle(btnOK, ThemeHelper.SuccessGreen, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.DangerRed, Color.White, 6);
            panelButtons.Controls.Add(btnOK); panelButtons.Controls.Add(btnCancel);
            tlp.Controls.Add(panelButtons, 0, 6); tlp.SetColumnSpan(panelButtons, 2);
            this.Controls.Add(tlp);

            if (existing != null)
            {
                txtMaLHP.Text = existing.MaLHP; txtMaLHP.Enabled = false;
                txtTenLop.Text = existing.TenLop;
                txtMaHP.Text = existing.MaHP;
                txtMaGV.Text = existing.MaGV;
                txtSiSo.Text = existing.SiSo.ToString();
                txtLichHoc.Text = existing.LichHoc;
            }

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaLHP.Text) || string.IsNullOrWhiteSpace(txtTenLop.Text) || string.IsNullOrWhiteSpace(txtMaHP.Text) || string.IsNullOrWhiteSpace(txtMaGV.Text) || string.IsNullOrWhiteSpace(txtSiSo.Text))
                { MessageBox.Show("Các trường Mã LHP/Tên lớp/Mã HP/Mã GV/Sĩ số bắt buộc"); return; }
                int siSo; if (!int.TryParse(txtSiSo.Text, out siSo)) { MessageBox.Show("Sĩ số phải là số"); return; }
                Result = new CourseSection { MaLHP = txtMaLHP.Text.Trim(), TenLop = txtTenLop.Text.Trim(), MaHP = txtMaHP.Text.Trim(), MaGV = txtMaGV.Text.Trim(), SiSo = siSo, LichHoc = string.IsNullOrWhiteSpace(txtLichHoc.Text) ? null : txtLichHoc.Text.Trim() };
                this.DialogResult = DialogResult.OK;
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void AddRow(TableLayoutPanel tlp, string label, Control input, int row)
        {
            var lbl = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Font = ThemeHelper.LabelFont, ForeColor = ThemeHelper.TextDark };
            ThemeHelper.ApplyTextBoxStyle(input as TextBox);
            tlp.Controls.Add(lbl, 0, row); tlp.Controls.Add(input, 1, row); input.Dock = DockStyle.Fill;
        }
    }
}































