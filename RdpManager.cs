using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionApp
{


    public class RdpManager
    {
        #region Подключение по РДП
        public static void ConnectRdp(string remotePC)
        {
            var (username, password) = CredentialFileManager.ReadCredentials();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            ProcessStartInfo mstscProcessInfo = new ProcessStartInfo
            {
                FileName = "mstsc.exe",
                Arguments = $"/v:{remotePC} /admin /user:{username} /password:{password}",
                UseShellExecute = false
            };

            Process mstscProcess = new Process
            {
                StartInfo = mstscProcessInfo
            };

            mstscProcess.Start();
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
    

