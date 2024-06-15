using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ConnectionApp
{
    #region ID текущей сессии ПК
    public class SessionManager
    {
        public static string GetSessionId(string remotePC)
        {
            (string username, string password) = CredentialFileManager.ReadCredentials();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username or password is empty.");
                return null;
            }

            try
            {
                Process qwinstaProcess = new Process();
                qwinstaProcess.StartInfo.FileName = "qwinsta";
                qwinstaProcess.StartInfo.Arguments = $"/server:{remotePC}";
                qwinstaProcess.StartInfo.UseShellExecute = false;
                qwinstaProcess.StartInfo.RedirectStandardOutput = true;
                qwinstaProcess.StartInfo.RedirectStandardError = true;
                qwinstaProcess.StartInfo.UserName = username;
                qwinstaProcess.StartInfo.Password = SecurityHelper.ConvertToSecureString(password);
                qwinstaProcess.StartInfo.Domain = ""; // Укажите домен, если необходимо

                qwinstaProcess.Start();

                string output = qwinstaProcess.StandardOutput.ReadToEnd();
                qwinstaProcess.WaitForExit();

                if (qwinstaProcess.ExitCode == 0)
                {
                    // Парсинг вывода для получения ID сессии
                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        if (line.Contains(remotePC))
                        {
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length > 2)
                            {
                                return parts[2]; // ID сессии
                            }
                        }
                    }
                }
                else
                {
                    string error = qwinstaProcess.StandardError.ReadToEnd();
                    Console.WriteLine($"Error running qwinsta: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get session ID.\nError: {ex.Message}");
            }

            return null;
        }

    }
    #endregion
}
