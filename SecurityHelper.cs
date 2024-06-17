using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Diagnostics;

namespace ConnectionApp
{
    public static class SecurityHelper
    {
        #region Запись учетных данных в cmdkey с последующим удалением

        public static void AddCredentialsToCredentialManager(string server, string username, string password)
        {
            try
            {
                Process cmdkeyProcess = new Process();
                cmdkeyProcess.StartInfo.FileName = "cmd.exe";
                cmdkeyProcess.StartInfo.Arguments = $"/c cmdkey /generic:TERMSRV/{server} /user:{username} /pass:{password}";
                cmdkeyProcess.StartInfo.CreateNoWindow = true;
                cmdkeyProcess.StartInfo.UseShellExecute = false;
                cmdkeyProcess.Start();
                cmdkeyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add credentials to Credential Manager.\nError: {ex.Message}");
            }
        }

        public static void RemoveCredentialsFromCredentialManager(string server)
        {
            try
            {
                Process cmdkeyProcess = new Process();
                cmdkeyProcess.StartInfo.FileName = "cmd.exe";
                cmdkeyProcess.StartInfo.Arguments = $"/c cmdkey /delete:TERMSRV/{server}";
                cmdkeyProcess.StartInfo.CreateNoWindow = true;
                cmdkeyProcess.Start();
                cmdkeyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove credentials from Credential Manager.\nError: {ex.Message}");
            }
        }

        #endregion

        #region 

        public static SecureString ConvertToSecureString(string str)
        {
            SecureString secureStr = new SecureString();
            foreach (char c in str)
            {
                secureStr.AppendChar(c);
            }
            secureStr.MakeReadOnly();
            return secureStr;
        }

        #endregion
    }
}
