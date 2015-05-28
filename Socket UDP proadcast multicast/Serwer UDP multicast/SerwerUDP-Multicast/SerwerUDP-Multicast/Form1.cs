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
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt16(numericUpDown1.Value));

            byte[] buffer = UnicodeEncoding.Unicode.GetBytes(textBox2.Text);

            //socket członkiem grupy będzie mógł też odbierać
            MulticastOption multi_option = new MulticastOption(IPAddress.Parse(textBox1.Text));
            s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multi_option);//dodajemy udzial w grupie multiucastowej
            
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.IpTimeToLive, 6);
            s.SendTo(buffer, endPoint);
            s.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            wysylanie();
        }
    }
}
