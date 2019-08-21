using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject;
using Quobject.SocketIoClientDotNet.Client;
using System.Windows.Forms;
using System.Reflection;
using MRmonitorClient.view;
using System.Drawing;
using System.IO;
using MRmonitorClient.configuracoes;

namespace MRmonitorClient.classes
{
    class SocketIO
    {
        InfoMachine machine = new InfoMachine();
        ObjectThread objThread = new ObjectThread();
        Config conf = new Config();// configfile
        String nome = "";
        Main formulario = null;
        private static Quobject.SocketIoClientDotNet.Client.Socket socket;
        public bool ConexaoSocket(string url, Main form)
        {
            nome = conf.nome;
            formulario = form;
            try
            {
                socket.Disconnect();
            }
            catch { }
            socket = IO.Socket(url);
            
            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_CONNECT, () =>
            {
                objThread.SetControlPropertyValue(form.lblSvStatus, "text", "Server Status: Conectado!");
                objThread.SetControlPropertyValue(form.lblSvStatus, "ForeColor", Color.Green);
                socket.Emit("entrar", nome);
            });

            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_DISCONNECT, () =>
            {
                objThread.SetControlPropertyValue(form.lblSvStatus, "text", "Server Status: Desconectado!");
                objThread.SetControlPropertyValue(form.lblSvStatus, "ForeColor", Color.Red);   
            });
            //EVENT_RECONNECT
            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT, () =>
            {
                socket.Emit("entrar", nome);
            });

            socket.On("proxy", (data) =>
            {
                try
                {
                    bool recebido = Convert.ToBoolean(data);
                    if (recebido)
                    {
                        form.AtivarProxy();
                    }
                    else
                    {
                        form.DesativarProxy();
                    }
                }
                catch { }

            });
            string conteudo = "";
            socket.On("update", (data) =>
            {
                conteudo += data.ToString() + "\n";
              
            });
            socket.On("setarNome", (data) =>
            {
                try
                {
                    conf.nome = data.ToString();
                    nome = data.ToString();
                    conf.Save();
                    socket.Emit("infoMachine", JsonInfo());
                    
                }
                catch { }
            });
            socket.On("alert", (data) =>
            {
                try
                {
                    form.notifyIcon1.BalloonTipText = data.ToString();
                    form.notifyIcon1.BalloonTipTitle = "Alerta";
                    form.notifyIcon1.Icon = new Icon("alert.ico");
                    form.notifyIcon1.Visible = true;
                    form.notifyIcon1.ShowBalloonTip(15000);
                }
                catch { }  
            });
            socket.On("obterInfoMachine", (data) =>
            {
                socket.Emit("infoMachine", JsonInfo());
            });
            socket.On("obterPrint", (data) =>
            {
                EnviarPrint();
            });
            return true;
        }
        public void Desconectar()
        {
            try
            {
                socket.Disconnect();
                socket.Close();
            }
            catch { }
        }
        public void EnviarAtividade(string atividade)
        {
            try
            {
                socket.Emit("atividade", atividade);
            }
            catch { }
        }
        public void EnviarPrint()
        {
            try
            {                
                ImageConvert imageConvert = new ImageConvert();
                socket.Emit("print", imageConvert.ObterPrintString64());
            }
            catch { }
        }
        public string JsonInfo()
        {
            string infoPC = "{ \"IP\": \"" + machine.PegarIP() + "\", "
                + " \"Usuario\": \"" + machine.PegarNomeUsuarioPC() + "\", "
                + " \"NomeRede\": \"" + machine.PegarNomeRede() + "\" , "
                + " \"Nome\": \"" + nome + "\" , "
                + " \"Version\": \"" + formulario.ver + "\" }";

            return infoPC;
        }
    }

}
