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

namespace SerwerUDP_Multicast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket s;

        void wysylanie()
        {
			//tworzymy socket udp
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//tworzymy ip na ktore beda wysylane wiadomosci
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt16(numericUpDown1.Value));

			//bufor na dane
            byte[] buffer = UnicodeEncoding.Unicode.GetBytes(textBox2.Text);

            //socket członkiem grupy będzie mógł też odbierać dane 
            MulticastOption multi_option = new MulticastOption(IPAddress.Parse(textBox1.Text));
            s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multi_option);//dodajemy udzial w grupie multiucastowej
            
			//chyba ustawia ttl na 6
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.IpTimeToLive, 6);
			
			//wysyla wiadomosc do wszystkich ktorzy sa dodanie do tej grupy multicastowej
            s.SendTo(buffer, endPoint);
			
			//konczy dzialanie socketu
            s.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
			//po klikenieciu wysyla wiadomosc
            wysylanie();
        }
    }
}
