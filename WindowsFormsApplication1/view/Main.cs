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
       public string ver = "0";
        SocketIO socket = new SocketIO();
        InfoMachine machine = new InfoMachine();
        Config conf = new Config();// configfile
        Update update = new Update();

        public Main()
        {
            InitializeComponent();

            Ocultar();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
            txtUsuario.Text = conf.nome;

            
            Main form = this;
            socket.ConexaoSocket("http://192.168.0.16:3000", form).ToString();
            form.Opacity = 0;
            this.ShowInTaskbar = false;

        }

        private void btnTeste_Click(object sender, EventArgs e)
        {
                
        }
        public void SvStatus(string status)
        {
        
        }
        string ultimaAtividade = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
         //   Ocultar();
            string atual = machine.PegarJanelaAberta();
            if (ultimaAtividade != atual)
            {
                ultimaAtividade = atual;
                socket.EnviarAtividade(ultimaAtividade);  
            }
        }

        private void txtUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            conf.nome = txtUsuario.Text;
            conf.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        { 

        }
        public void Ocultar()
        {
            this.Visible = false;
            this.Hide();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            update.VerificarAtt(ver, timer2);

        }
    }
}
