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
                SetControlPropertyValue(form.lblSvStatus, "text", "Server Status: Conectado!");   
                SetControlPropertyValue(form.lblSvStatus, "ForeColor", Color.Green);
                socket.Emit("entrar", "matheus");

                string infoPC = "{ \"IP\": \"" + machine.PegarIP() +"\", "
                +" \"Usuario\": \"" + machine.PegarNomeUsuarioPC() +"\", "
                +" \"NomeRede\": \"" + machine.PegarNomeRede() +"\" }";
                socket.Emit("update", infoPC);
            });

            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_DISCONNECT, () =>
            {                
                SetControlPropertyValue(form.lblSvStatus, "text", "Server Status: Desconectado!");
                SetControlPropertyValue(form.lblSvStatus, "ForeColor", Color.Red);   
            });

            string conteudo = "";
            socket.On("update", (data) =>
            {

                conteudo += data.ToString() + "\n";
                form.SvStatus(conteudo);
                SetControlPropertyValue(form.btnTeste, "text", conteudo);
                
                

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
                socket.Emit("imagem", imageConvert.ObterPrintString64());
            }
            catch { }
        }


     

        /*          Acesso a thread*/
        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {

            if (oControl.InvokeRequired)
            {

                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);

                oControl.Invoke(d, new object[] { oControl, propName, propValue });

            }

            else
            {

                Type t = oControl.GetType();

                PropertyInfo[] props = t.GetProperties();

                foreach (PropertyInfo p in props)
                {

                    if (p.Name.ToUpper() == propName.ToUpper())
                    {

                        p.SetValue(oControl, propValue, null);

                    }

                }

            }
        }
    }




}
