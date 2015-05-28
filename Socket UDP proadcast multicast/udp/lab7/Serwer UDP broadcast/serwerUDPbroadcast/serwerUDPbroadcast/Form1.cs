using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        void wysylanie()
        {
            
            //udp socket
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //255.255.255.255:7
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, Convert.ToInt16(numericUpDown1.Value));

			//bufor na dane
            byte[] buffer = UnicodeEncoding.Unicode.GetBytes(textBox1.Text);

            //opcje socketu jako broadcastowy
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
			
			//wysyla dane do wszystkich w sieci
            s.SendTo(buffer, endPoint);

			//czysci textbox
            textBox1.Text = "";
			
			//wylacza socket
            s.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
			//wysyla po kliknieciu klawisza
            wysylanie();
        }
    }
}
