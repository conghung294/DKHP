using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApp1.UI
{
    /// <summary>
    /// Helper class để quản lý theme, màu sắc và style cho toàn bộ ứng dụng
    /// </summary>
    public static class ThemeHelper
    {
        // Màu sắc chủ đạo
        public static Color PrimaryBlue = Color.FromArgb(33, 150, 243); // #2196F3
        public static Color PrimaryBlueDark = Color.FromArgb(25, 118, 210); // #1976D2
        public static Color PrimaryBlueDarker = Color.FromArgb(13, 71, 161); // #0D47A1
        public static Color BackgroundLight = Color.FromArgb(245, 245, 245); // #F5F5F5
        public static Color BackgroundWhite = Color.White;
        public static Color TextDark = Color.FromArgb(33, 33, 33); // #212121
        public static Color TextGray = Color.FromArgb(97, 97, 97); // #616161
        public static Color BorderLight = Color.FromArgb(224, 224, 224); // #E0E0E0
        public static Color SuccessGreen = Color.FromArgb(76, 175, 80); // #4CAF50
        public static Color DangerRed = Color.FromArgb(244, 67, 54); // #F44336
        public static Color WarningOrange = Color.FromArgb(255, 152, 0); // #FF9800
        public static Color SidebarBackground = Color.FromArgb(240, 242, 245); // #F0F2F5
        public static Color SidebarActive = Color.FromArgb(33, 150, 243); // #2196F3
        public static Color SidebarHover = Color.FromArgb(238, 238, 238); // #EEEEEE
        public static Color HeaderBlue = Color.FromArgb(25, 118, 210); // #1976D2

        // Fonts
        public static Font TitleFont = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
        public static Font HeaderFont = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
        public static Font SubHeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        public static Font NormalFont = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        public static Font LabelFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static Font ButtonFont = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
        public static Font SmallFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>
        /// Apply style cho button với rounded corners (đơn giản hóa)
        /// </summary>
        public static void ApplyButtonStyle(Button button, Color backColor, Color foreColor, int cornerRadius = 5)
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Font = ButtonFont;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = LightenColor(backColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = DarkenColor(backColor, 0.2f);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }
        
        /// <summary>
        /// Làm sáng màu
        /// </summary>
        private static Color LightenColor(Color color, float factor)
        {
            return Color.FromArgb(
                color.A,
                Math.Min(255, (int)(color.R + (255 - color.R) * factor)),
                Math.Min(255, (int)(color.G + (255 - color.G) * factor)),
                Math.Min(255, (int)(color.B + (255 - color.B) * factor))
            );
        }
        
        /// <summary>
        /// Làm tối màu
        /// </summary>
        private static Color DarkenColor(Color color, float factor)
        {
            return Color.FromArgb(
                color.A,
                Math.Max(0, (int)(color.R * (1 - factor))),
                Math.Max(0, (int)(color.G * (1 - factor))),
                Math.Max(0, (int)(color.B * (1 - factor)))
            );
        }

        /// <summary>
        /// Apply style cho TextBox với rounded corners và border
        /// </summary>
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BackColor = BackgroundWhite;
            textBox.ForeColor = TextDark;
            textBox.Font = NormalFont;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Padding = new Padding(10, 8, 10, 8);

            // Vẽ border và background
            textBox.Paint += (sender, e) =>
            {
                TextBox txt = sender as TextBox;
                if (txt == null) return;

                using (GraphicsPath path = CreateRoundedRectangle(txt.ClientRectangle, 4))
                {
                    using (Pen pen = new Pen(BorderLight, 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };
        }

        /// <summary>
        /// Apply style cho DataGridView với header màu xanh
        /// </summary>
        public static void ApplyDataGridViewStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = BackgroundWhite;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.Font = NormalFont;
            dgv.DefaultCellStyle.Font = NormalFont;
            dgv.DefaultCellStyle.ForeColor = TextDark;
            dgv.DefaultCellStyle.BackColor = BackgroundWhite;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(229, 243, 255);
            dgv.DefaultCellStyle.SelectionForeColor = TextDark;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 40;

            // Style cho header
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Style cho rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.GridColor = BorderLight;
        }

        /// <summary>
        /// Tạo rounded rectangle path
        /// </summary>
        public static GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Tạo Panel với rounded corners
        /// </summary>
        public static Panel CreateRoundedPanel(Color backColor, int cornerRadius = 8)
        {
            Panel panel = new Panel();
            panel.BackColor = backColor;
            panel.Paint += (sender, e) =>
            {
                Panel pnl = sender as Panel;
                if (pnl == null) return;

                using (GraphicsPath path = CreateRoundedRectangle(pnl.ClientRectangle, cornerRadius))
                {
                    using (SolidBrush brush = new SolidBrush(pnl.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (Pen pen = new Pen(BorderLight, 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            return panel;
        }

        /// <summary>
        /// Tạo label với style chuẩn
        /// </summary>
        public static Label CreateLabel(string text, Font font, Color foreColor, ContentAlignment alignment = ContentAlignment.MiddleLeft)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = font;
            label.ForeColor = foreColor;
            label.AutoSize = false;
            label.TextAlign = alignment;
            return label;
        }

        /// <summary>
        /// Tạo header bar panel
        /// </summary>
        public static Panel CreateHeaderBar(string title, int width, int height = 60)
        {
            Panel header = new Panel();
            header.Size = new Size(width, height);
            header.BackColor = HeaderBlue;
            header.Dock = DockStyle.Top;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = false;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Padding = new Padding(20, 0, 0, 0);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Size = new Size(width - 150, height);

            header.Controls.Add(lblTitle);
            return header;
        }

        /// <summary>
        /// Tạo sidebar panel
        /// </summary>
        public static Panel CreateSidebar(int width = 250)
        {
            Panel sidebar = new Panel();
            sidebar.Width = width;
            sidebar.BackColor = SidebarBackground;
            sidebar.Dock = DockStyle.Left;
            sidebar.BorderStyle = BorderStyle.None;

            return sidebar;
        }
    }
}

