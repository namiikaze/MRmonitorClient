using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MRmonitorClient.classes
{
    class TestarConexao
    {
        public bool Aberta(string hostname, int porta)
        {
            TcpClient tcp = new TcpClient();
            bool PortaAberta = true;
            try
            {
                tcp.Connect(hostname, porta);
                tcp.Close();
            }
            catch
            {
                PortaAberta = false;
            }
            return PortaAberta;
        }

    }
}
