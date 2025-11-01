using System;
using System.Windows.Forms;
using WindowsFormsApp1.Database;

namespace WindowsFormsApp1.Forms
{
    public partial class TestConnectionForm : Form
    {
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblTitle;

        public TestConnectionForm()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            btnTest.Text = "Đang kiểm tra...";
            txtResult.Clear();
            
            Application.DoEvents(); // Refresh UI

            var db = DatabaseHelper.Instance;
            string message;
            bool success = db.TestConnection(out message);

            txtResult.Text = message;
            
            if (success)
            {
                txtResult.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                txtResult.ForeColor = System.Drawing.Color.Red;
            }

            btnTest.Enabled = true;
            btnTest.Text = "Kiểm tra lại";
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(320, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Kiểm tra Kết nối Database";
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(20, 70);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(200, 45);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Kiểm tra Kết nối";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(20, 130);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(900, 400);
            this.txtResult.TabIndex = 2;
            // 
            // TestConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 560);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TestConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Database Connection";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

