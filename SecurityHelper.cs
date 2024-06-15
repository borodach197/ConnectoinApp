using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace ConnectionApp
{
    public static class SecurityHelper
    {
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
    }
}
