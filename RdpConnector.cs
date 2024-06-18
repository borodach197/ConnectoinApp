using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows;
using System.IO;

namespace ConnectionApp
{


    public class RdpConnector
    {
        #region Подключение по РДП
        public static void ConnectRdp(string remotePC, string username, string password)
        {
            try
            {
                // Add credentials to Credential Manager
                SecurityHelper.AddCredentialsToCredentialManager(remotePC, username, password);

                // Start RDP connection
                Process mstscProcess = new Process();
                mstscProcess.StartInfo.FileName = "mstsc.exe";
                mstscProcess.StartInfo.Arguments = $"/v:{remotePC}";
                mstscProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to RDP.\nError: {ex.Message}");
            }
        }
        #endregion

        #region Подключение по шадоу РДП с управлением с подтверждением пользователя

        public static void ConnectShadowRDP(string server, string username, string password)
        {
            try
            {
                // Получение текущего ID сессии
                string sessionId = SessionManager.GetSessionId(server, username, password);
                if (sessionId == "No active sessions found.")
                {
                    Console.WriteLine("No active sessions found.");
                    return;
                }

                // Запуск командной строки для выполнения команды shadow rdp
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mstsc /shadow:{sessionId} /v:{server} /control /noConsentPrompt",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UserName = username,
                    PasswordInClearText = password, // Прямая передача пароля
                    Domain = "" // Задайте домен, если требуется
                };

                using (Process process = Process.Start(startInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string output = reader.ReadToEnd();
                        process.WaitForExit();

                        // Вывод результата выполнения команды
                        Console.WriteLine(output);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect via shadow RDP.\nError: {ex.Message}");
            }
        }
        #endregion

    }
}
    

