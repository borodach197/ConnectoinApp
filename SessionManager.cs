using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ConnectionApp
{
    public class SessionManager
    {
        public static string GetActiveSessionId(string remoteComputer)
        {
            try
            {
                Process quserProcess = new Process();
                quserProcess.StartInfo.FileName = @"C:\Windows\System32\quser.exe"; // Указать полный путь
                quserProcess.StartInfo.Arguments = $"/server:{remoteComputer}";
                quserProcess.StartInfo.UseShellExecute = false;
                quserProcess.StartInfo.RedirectStandardOutput = true;
                quserProcess.Start();

                string output = quserProcess.StandardOutput.ReadToEnd();
                quserProcess.WaitForExit();

                // Парсинг вывода для получения ID сессии
                string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (line.Contains(remoteComputer))
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 2)
                        {
                            return parts[2]; // ID сессии
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get session ID.\nError: {ex.Message}");
            }

            return null;
        }
    }
}
