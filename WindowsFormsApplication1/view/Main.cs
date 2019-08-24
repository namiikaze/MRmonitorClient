using MRmonitorClient.classes;
using MRmonitorClient.configuracoes;
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
       public string ver = "0.1";
       public bool ativarProxy = true;
        SocketIO socketIO = new SocketIO();
        InfoMachine machine = new InfoMachine();
        Config conf = new Config();// configfile
        Update update = new Update();
        Proxy proxy = new Proxy();

        

        public Main()
        {
            InitializeComponent();
            Ocultar();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = conf.nome;   
            Main form = this;
            socketIO.ConexaoSocket("http://192.168.0.16:3000", form).ToString();
            form.Opacity = 0;
            this.ShowInTaskbar = false;

            Chat chat = new Chat(socketIO.GetSocket());
            chat.Show();
        }


        string ultimaAtividade = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            string atual = machine.PegarJanelaAberta();
            if (ultimaAtividade != atual)
            {
                ultimaAtividade = atual;
                socketIO.EnviarAtividade(ultimaAtividade);  
            }
        }

        private void txtUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            conf.nome = txtUsuario.Text;
            conf.Save();
        }

        public void Ocultar()
        {
            this.Visible = false;
            this.Hide();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            update.VerificarAtt(ver, timer2);

            if(ativarProxy){
          //      proxy.AutoProxy("192.168.0.30", 3128);
            }
        }
        public void DesativarProxy()
        {
            proxy.DesativarProxy();
            ativarProxy = false;
        }

        public void AtivarProxy()
        {
            proxy.AtivarProxy("192.168.0.30", 3128);
            ativarProxy = true;
        }
    }
}
