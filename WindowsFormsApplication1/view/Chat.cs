using MRmonitorClient.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRmonitorClient.view
{
    public partial class Chat : Form
    {
        

        ChatSocket chatSocket;
        public Chat(Quobject.SocketIoClientDotNet.Client.Socket socket)
        {
            
            InitializeComponent();
             chatSocket = new ChatSocket(socket,this);

            
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            MensagemGlobal();
            
        }

        private void txtMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MensagemGlobal();
            }
        }

        private void MensagemGlobal()
        {
            if (!string.IsNullOrEmpty(txtMensagem.Text) && txtMensagem.Text != " ")
            {
                rtbConversa.Text += "Você: " + txtMensagem.Text + "\n";
                chatSocket.Mensagem(txtMensagem.Text, "all");
                txtMensagem.Text = " ";
            }
        }

        delegate void msgRecebidaCallback(string msg);
        public void msgRecebida(string msg)
        {
            if (InvokeRequired)
            {
                msgRecebidaCallback callback = msgRecebida;
                Invoke(callback, msg);
            }
            else
            {
                rtbConversa.Text += msg + "\n";
            }
            
            
        }

    }
}
