using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace ConnectionApp
{
    public class SessionInfo
    {
        public string User { get; set; }
        public string SessionId { get; set; }
    }
    #region ID текущей сессии ПК
    public class SessionManager
    {
        public static void ShowSessions()
        {
            try
            {
                var sessions = GetSessions();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var sessionWindow = new SessionWindow(sessions);
                    sessionWindow.Show();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get sessions.\nError: {ex.Message}");
            }
        }

        private static List<SessionInfo> GetSessions()
        {
            List<SessionInfo> sessions = new List<SessionInfo>();
            Process quserProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c quser",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            quserProcess.Start();

            string output = quserProcess.StandardOutput.ReadToEnd();
            quserProcess.WaitForExit();

            var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(1)) // Skip header line
            {
                var columns = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (columns.Length >= 2)
                {
                    sessions.Add(new SessionInfo
                    {
                        User = columns[0],
                        SessionId = columns[1]
                    });
                }
            }

            return sessions;
        }

    }
    #endregion
}
