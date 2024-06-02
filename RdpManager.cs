using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionApp
{
    #region Подключение по РДП
    #endregion
    public class RdpManager
    {
        public static void ConnectRdp(string remoteComputer)
        {
            try
            {
                Process.Start("mstsc.exe", $"/v:{remoteComputer}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start RDP connection.\nError: {ex.Message}");
            }
        }

        #region Подключение по шадоу РДП с управлением с подтверждением пользователя
        #endregion
        public static void ConnectShadowRdp(string remoteComputer, string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                Console.WriteLine("Session ID is empty.");
                return;
            }

            try
            {
                Process.Start("mstsc.exe", $"/shadow:{sessionId} /control /v:{remoteComputer}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start Shadow RDP.\nError: {ex.Message}");
            }
        }

        #region ПОдключение по РДП с УЗ админа
        #endregion
        public static void ConnectWithSavedCredentials(string filePath)
        {
            //var filepath = CredentialFileManager.
            // Чтение учетных данных из файла
            (string username, string password) = CredentialFileManager.ReadCredentials();

            // Подключение по RDP с использованием учетных данных
            Process rdpProcess = new Process();
            rdpProcess.StartInfo.FileName = "mstsc.exe"; // Путь к клиенту RDP
            rdpProcess.StartInfo.Arguments = $"/v:remote_pc /u:{username} /p:{password}"; // Аргументы командной строки для подключения
            rdpProcess.Start();
        }
    }
}
