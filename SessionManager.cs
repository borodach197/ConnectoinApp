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
    #region ID текущей сессии ПК
    public class SessionManager
    {
        public static string GetSessionId(string remotePC, string username, string password)
        {
            (username, password) = CredentialFileManager.ReadCredentials();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username or password is empty.");
                return null;
            }

            try
            {
                // Создание процесса для выполнения команды qwinsta
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c qwinsta /server:{remotePC}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    UserName = username,
                    PasswordInClearText = password, // Прямая передача пароля, если метод ConvertToSecureString не требуется
                    Domain = "" // Задайте домен, если требуется
                };

                using (Process process = Process.Start(startInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string output = reader.ReadToEnd();
                        process.WaitForExit();

                        // Анализ результата для извлечения ID сессии
                        string sessionId = ParseSessionId(output);
                        return sessionId ?? "No active sessions found.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Failed to get session ID.\nError: {ex.Message}";
            }
        }

        private static string ParseSessionId(string output)
        {
            // Логика анализа вывода команды qwinsta для извлечения ID сессии
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.Contains("Active"))
                {
                    string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return parts[2]; // Предполагается, что ID сессии находится в третьем столбце
                }
            }
            return null;
        }

    }
    #endregion
}
