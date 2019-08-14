using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;
using System.Reflection;
using System.IO;
	


namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        Graphics g;
        int TelaLargura = Screen.PrimaryScreen.Bounds.Width;
        int TelaAltura = Screen.PrimaryScreen.Bounds.Height;
        public Main()
        {
            InitializeComponent();
            
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {


        }


        Quobject.SocketIoClientDotNet.Client.Socket socket;
        private void ConexaoSocket()
        {
            
            socket = IO.Socket(txtIP.Text);
            

            socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_CONNECT, () =>
            {
                socket.Emit("entrar", "matheus");
            });

            string conteudo = "";
            socket.On("update", (data) =>
            {

                conteudo += data.ToString() + "\n";
                SetControlPropertyValue(rtbLog, "text", conteudo);

            });
            
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            socket.Emit("entrar", "matheus");
            
            //armazena a imagem no bitmap
            Bitmap b = new Bitmap(TelaLargura, TelaAltura);
            //copia  a tela no bitmap
            g = Graphics.FromImage(b);
            g.CopyFromScreen(Point.Empty, Point.Empty, Screen.PrimaryScreen.Bounds.Size);
            //atribui a imagem ao picturebox exibindo-a
            //picTela.Image = b;
            this.Show();
            //habilita o botão para salvar a tela
         /**/
            byte[] bufferTempTemp = imageToByteArray(b);
            Image img = byteArrayToImage(bufferTempTemp);
            picTela.Image = img;
            //socket.Emit("imagem", bufferTempTemp);
            //socket.Emit("update", "teste");
            /**/

             
        string img64Bit = ConvertImageToBase64String(img);
        Image newImg = ConvertBase64StringToImage(img64Bit);

        socket.Emit("imagem", img64Bit);
            

        }
        public static int CompareImages(Image i1, Image i2)
        {
            string img1 = ConvertImageToBase64String(i1);
            string img2 = ConvertImageToBase64String(i2);
            return String.Compare(img1, img2);
        }
        public static string ConvertImageToBase64String(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Image ConvertBase64StringToImage(string image64Bit)
        {
            byte[] imageBytes = Convert.FromBase64String(image64Bit);
            return new Bitmap(new MemoryStream(imageBytes));
        }
       

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        
        
        private void btnConectar_Click(object sender, EventArgs e)
        {
            ConexaoSocket();
         
            
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            socket.Disconnect();
        }












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
