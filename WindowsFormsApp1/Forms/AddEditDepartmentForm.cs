using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1.Forms
{
    public class AddEditDepartmentForm : Form
    {
        private TextBox txtMa, txtTen;
        private Button btnOK, btnCancel;
        public string ResultMa { get; private set; }
        public string ResultTen { get; private set; }

        public AddEditDepartmentForm(string ma = null, string ten = null)
        {
            this.Text = string.IsNullOrEmpty(ma) ? "Thêm khoa" : "Sửa khoa";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.ClientSize = new Size(460, 200);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(16) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            txtMa = new TextBox(); txtTen = new TextBox();
            AddRow(tlp, "Mã khoa", txtMa, 0);
            AddRow(tlp, "Tên khoa", txtTen, 1);

            var panelButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            btnOK = new Button { Text = "Lưu", Width = 100, Height = 32 };
            btnCancel = new Button { Text = "Hủy", Width = 100, Height = 32 };
            ThemeHelper.ApplyButtonStyle(btnOK, ThemeHelper.SuccessGreen, Color.White, 6);
            ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.DangerRed, Color.White, 6);
            panelButtons.Controls.Add(btnOK); panelButtons.Controls.Add(btnCancel);
            tlp.Controls.Add(panelButtons, 0, 2); tlp.SetColumnSpan(panelButtons, 2);
            this.Controls.Add(tlp);

            if (!string.IsNullOrEmpty(ma))
            {
                txtMa.Text = ma; txtMa.Enabled = false;
                txtTen.Text = ten;
            }

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Mã/Tên khoa bắt buộc"); return; }
                ResultMa = txtMa.Text.Trim();
                ResultTen = txtTen.Text.Trim();
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































