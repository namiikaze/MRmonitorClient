using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRmonitorClient.classes
{
    class Update
    {
        WebClient client = new WebClient();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public void VerificarAtt(string ver, Timer timer)
        {
            try
            {
                if (client.DownloadString("http://127.0.0.1/down/ver.txt") != ver)
                {
                    timer.Enabled = false;
                    Process.Start("Updater.exe");
                    Application.Exit();
                }
            }
            catch
            { }
            
        }
    }
}
