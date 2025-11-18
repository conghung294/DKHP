using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1.UI
{
  internal static class HelpLauncher
  {
    const string HelpFileName = "help.chm";

    public static void ShowHelp(Form owner)
    {
      try
      {
        var helpPath = ResolveHelpPath();
        if (!File.Exists(helpPath))
        {
          MessageBox.Show(owner, $"Không tìm thấy file hướng dẫn {HelpFileName}.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        Help.ShowHelp(owner, helpPath, HelpNavigator.TableOfContents);
      }
      catch (Exception ex)
      {
        MessageBox.Show(owner, $"Không thể mở tài liệu hướng dẫn.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    static string ResolveHelpPath()
    {
      var startup = AppDomain.CurrentDomain.BaseDirectory;
      var directPath = Path.Combine(startup, HelpFileName);
      if (File.Exists(directPath)) return directPath;

      try
      {
        var projectRoot = Path.GetFullPath(Path.Combine(startup, "..", ".."));
        var projectFile = Path.Combine(projectRoot, HelpFileName);
        if (File.Exists(projectFile)) return projectFile;
      }
      catch
      {
        // bỏ qua, fallback bên dưới
      }

      return directPath;
    }
  }
}

