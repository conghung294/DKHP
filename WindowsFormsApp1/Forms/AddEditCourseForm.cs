using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AddEditCourseForm : Form
    {
        private TextBox txtMaMH, txtTenHP, txtSoTC, txtMaTQ, txtMaHK;
        private Button btnOK, btnCancel;
        public Course Result { get; private set; }

        public AddEditCourseForm(Course existing = null)
        {
            this.Text = existing == null ? "Thêm môn học" : "Sửa môn học";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.ClientSize = new Size(520, 300);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 6, Padding = new Padding(16) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            txtMaMH = new TextBox(); txtTenHP = new TextBox(); txtSoTC = new TextBox(); txtMaTQ = new TextBox(); txtMaHK = new TextBox();
            AddRow(tlp, "Mã môn học", txtMaMH, 0);
            AddRow(tlp, "Tên học phần", txtTenHP, 1);
            AddRow(tlp, "Số tín chỉ", txtSoTC, 2);
            AddRow(tlp, "Mã tiên quyết", txtMaTQ, 3);
            AddRow(tlp, "Mã học kỳ", txtMaHK, 4);

            var panelButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            btnOK = new Button { Text = "Lưu", Width = 100, Height = 32 };
            btnCancel = new Button { Text = "Hủy", Width = 100, Height = 32 };
            ThemeHelper.ApplyButtonStyle(btnOK, ThemeHelper.SuccessGreen, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.DangerRed, Color.White, 6);
            panelButtons.Controls.Add(btnOK); panelButtons.Controls.Add(btnCancel);
            tlp.Controls.Add(panelButtons, 0, 5); tlp.SetColumnSpan(panelButtons, 2);
            this.Controls.Add(tlp);

            if (existing != null)
            {
                txtMaMH.Text = existing.MaMH; txtMaMH.Enabled = false;
                txtTenHP.Text = existing.TenHocPhan;
                txtSoTC.Text = existing.SoTC.ToString();
                txtMaTQ.Text = existing.MaHocPhanTienQuyet;
                txtMaHK.Text = existing.MaHocKi.ToString();
            }

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaMH.Text) || string.IsNullOrWhiteSpace(txtTenHP.Text) || string.IsNullOrWhiteSpace(txtSoTC.Text) || string.IsNullOrWhiteSpace(txtMaHK.Text))
                { MessageBox.Show("Mã, Tên, Số TC, Mã HK là bắt buộc"); return; }
                int soTC, maHK; if (!int.TryParse(txtSoTC.Text, out soTC) || !int.TryParse(txtMaHK.Text, out maHK)) { MessageBox.Show("Số TC/Mã HK phải là số"); return; }
                Result = new Course { MaMH = txtMaMH.Text.Trim(), TenHocPhan = txtTenHP.Text.Trim(), SoTC = soTC, MaHocPhanTienQuyet = string.IsNullOrWhiteSpace(txtMaTQ.Text) ? null : txtMaTQ.Text.Trim(), MaHocKi = maHK };
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






















