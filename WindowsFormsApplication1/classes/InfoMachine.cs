using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRmonitorClient.classes
{
    class InfoMachine{
        #region
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        #endregion  //DLL
        public string PegarIP()
        {
            String strHostName = String.Empty;
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0].ToString();
        }
        public string PegarJanelaAberta()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public string PegarNomeUsuarioPC()
        {
            return SystemInformation.UserName;            
        }
        public string PegarNomeRede()
        {
            return Dns.GetHostName();   
        }

    }
}
