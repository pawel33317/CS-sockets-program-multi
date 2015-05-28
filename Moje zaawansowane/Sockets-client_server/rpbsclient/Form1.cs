using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace rpbsclient
{
    public partial class Form1 : Form
    {
        Socket sck;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //połączenie
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sck.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"),8888));
            }
            catch
            {
                MessageBox.Show("Nie można Połączyć");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //zapisujemy do tablicy dane z textboxa
            byte[] data = Encoding.Default.GetBytes(richTextBox1.Text);

            //wysyłamy rozmiar danych
            sck.Send(BitConverter.GetBytes(data.Length), 0, 4, 0);

            //wysyłamy dane
            sck.Send(data);
        }
    }
}
