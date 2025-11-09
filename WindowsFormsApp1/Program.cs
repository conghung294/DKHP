using System;
using System.Windows.Forms;
using WindowsFormsApp1.Database;
using WindowsFormsApp1.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new LoginForm());
        }

        // Optional: Test connection khi khởi động ứng dụng
        private static void TestConnectionOnStartup()
        {
            var db = DatabaseHelper.Instance;
            string message;
            bool success = db.TestConnection(out message);
            
            if (!success)
            {
                var result = MessageBox.Show(
                    message + "\n\nBạn có muốn tiếp tục không? (Có thể sẽ gặp lỗi)", 
                    "Cảnh báo kết nối Database", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);
                    
                if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
        }
    }
}
