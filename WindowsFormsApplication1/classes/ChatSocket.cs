using MRmonitorClient.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace MRmonitorClient.classes
{
    
    class ChatSocket
    {
        ObjectThread objThread = new ObjectThread();
        string MeuSocketID;
        Quobject.SocketIoClientDotNet.Client.Socket socket;
        public ChatSocket(Quobject.SocketIoClientDotNet.Client.Socket socket, Chat form)
        {
            this.socket = socket;

           
            socket.On("MensagemRecebida", (data) =>
            {

                
            });
            
            socket.On("users", (data) =>
            {
                
            });
           


            socket.On("ClientId", (data) =>
            {
                MeuSocketID = data.ToString();
                form.msgRecebida(MeuSocketID);
            });
        }

        public void Mensagem(string msg,string destino)
        {
            socket.Emit("EnviarMensagem", EnviarJsonMessage(msg,destino));   
        }


        public string EnviarJsonMessage(string msg, string destino)
        {
            string retorno = "{ \"mensagem\": \""+msg +"\", " +
            "\"destino\": \""+destino+"\" }";
            return retorno;
        }


    }
}
