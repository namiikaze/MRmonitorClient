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

namespace MRmonitorClient.classes
{
    class SocketIO
    {
        InfoMachine machine = new InfoMachine();
        ObjectThread objThread = new ObjectThread();

        private static Quobject.SocketIoClientDotNet.Client.Socket socket;
        public bool ConexaoSocket(string url, Main form)
        {
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
                socket.Emit("entrar", "client");
            });

            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_DISCONNECT, () =>
            {
                objThread.SetControlPropertyValue(form.lblSvStatus, "text", "Server Status: Desconectado!");
                objThread.SetControlPropertyValue(form.lblSvStatus, "ForeColor", Color.Red);   
            });
            //EVENT_RECONNECT
            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT, () =>
            {
                socket.Emit("entrar", "client");
            });

            string conteudo = "";
            socket.On("update", (data) =>
            {
                conteudo += data.ToString() + "\n";
                form.SvStatus(conteudo);
                objThread.SetControlPropertyValue(form.btnTeste, "text", conteudo);

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
                + " \"NomeRede\": \"" + machine.PegarNomeRede() + "\" }";

            return infoPC;
        }
    }

}
