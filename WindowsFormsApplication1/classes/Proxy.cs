using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MRmonitorClient.classes
{
    
    class Proxy
    {
        private bool statusProxy = false;

        /*Refresh do sistema para que não seja necessario reiniciar o navegador ao alterar o proxy*/
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        bool settingsReturn, refreshReturn;
        /**/
        RegistryKey reg_key;
        TestarConexao testeconn = new TestarConexao();
        

        public void AtivarProxy(string IP, int port){
            reg_key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            string proxy = IP + ":" + port;
            reg_key.SetValue("ProxyEnable", 1);
            reg_key.SetValue("ProxyServer", proxy);
            RefreshSystem();
            statusProxy = true;

        }

        public void DesativarProxy()
        {
            reg_key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            reg_key.SetValue("ProxyEnable", 0);
            RefreshSystem();
            statusProxy = false;

        }
        private void RefreshSystem()
        {
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public bool AutoProxy(string IP, int port)
        {
            
            if (testeconn.Aberta(IP, port))
            {
                if (!statusProxy)
                {
                    AtivarProxy(IP, port);
                }
            }
            else
            {
                if (statusProxy)
                {
                    DesativarProxy();
                }
            }

            return statusProxy;//
        }

    }
}
