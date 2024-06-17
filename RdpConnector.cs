using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows;

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

        public static void ConnectShadowRdp(string remotePC, string sessionId)
        {
            var (username, password) = CredentialFileManager.ReadCredentials();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            sessionId = SessionManager.GetSessionId(remotePC);

            if (sessionId == null)
            {
                Console.WriteLine("No active sessions found.");
                return;
            }

            ProcessStartInfo shadowProcessInfo = new ProcessStartInfo
            {
                FileName = "mstsc.exe",
                Arguments = $"/shadow:{sessionId} /control /noConsentPrompt /user:{username} /password:{password}",
                UseShellExecute = false
            };

            Process shadowProcess = new Process
            {
                StartInfo = shadowProcessInfo
            };

            shadowProcess.Start();
        }
        #endregion

    }
}
    

