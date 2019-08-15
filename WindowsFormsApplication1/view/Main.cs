using MRmonitorClient.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRmonitorClient.view
{
    public partial class Main : Form
    {
        SocketIO socket = new SocketIO();
        InfoMachine machine = new InfoMachine();
        public Main()
        {
            InitializeComponent();
            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Main teste = this;
            
            socket.ConexaoSocket("http://localhost:3000", teste).ToString();

            

            //rtbLog.Text += machine.PegarIP() + "\n";
            rtbLog.Text += machine.PegarJanelaAberta() + "\n";

        }

        private void btnTeste_Click(object sender, EventArgs e)
        {
            
                socket.Desconectar();
                    
            
            
            
        }
        public void SvStatus(string status)
        {
        
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

            
        }
    }
}
