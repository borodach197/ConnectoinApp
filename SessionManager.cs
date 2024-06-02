using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectionApp
{
    public class SessionManager
    {
        public static string GetActiveSessionId(string remoteComputer)
        {
            string sessionId = null;

            // Выполняем команду quser для получения информации о сеансах на удаленном компьютере
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "quser.exe";
            processStartInfo.Arguments = $"/server:{remoteComputer}";
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;

            using (Process process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Извлекаем ID активной сессии из вывода команды quser
                    string[] lines = output.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Contains("Active"))
                        {
                            sessionId = line.Split().Last();
                            break;
                        }
                    }
                }
            }

            return sessionId;
        }
    }
}
