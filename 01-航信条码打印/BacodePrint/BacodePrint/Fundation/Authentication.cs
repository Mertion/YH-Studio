using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BacodePrint.Fundation
{
    internal class Authentication
    {
        [DllImport("MachineCodeAuthenticationDLL", EntryPoint = "CheckMachineCodeAuthentication", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int MachineCodeAuthenticationDLL();

        public static int CheckCount()
        {
            IniFile tFilesINI = new IniFile();
            string strPath = "C:\\Windows\\Authentication.ini";
            //string strPath = "D:\\Authentication.ini";
            string str = tFilesINI.INIRead("Config", "Count", strPath);

            int nCount = 0;
            if (str=="")
            {
                nCount = 0;
            }
            else
            {
                nCount = Convert.ToInt32(str);
            }

            nCount++;
            tFilesINI.INIWrite("Config", "Count", nCount.ToString(), strPath);

            if (nCount > 100) 
            {
                return 1;
            }
            
            
            return 0;
        }
    }
}
