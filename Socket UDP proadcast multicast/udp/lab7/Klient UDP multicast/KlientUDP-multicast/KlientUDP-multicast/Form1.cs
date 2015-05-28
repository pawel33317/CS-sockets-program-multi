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

namespace KlientUDP_multicast
{
    public partial class Form1 : Form
    {
        Socket s;
        Thread watek_nasluchu;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			//wlacza  przycisk
            button2.Enabled = false;
        }

        void nasluch()
        {
            //0.0.0.0:7 tworzy socket typu udp
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//host koncowy czyli kazdy host
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt16(numericUpDown1.Value));

            try
            {
				//ustawia milticastowe opcje socketu
                MulticastOption multi_option = new MulticastOption(IPAddress.Parse(textBox1.Text));
				//przypisuje multicastowe opcje socketu do socketu czyli wiadomosc dotrze do wszystkich ktorzy sa w tej grupie multicastowej 
				//dodajemy udzial w grupie multiucastowej
                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multi_option);
				//odpala nasluchiwanie
                s.Bind(endPoint);
            }
            catch (Exception f)
            {
				//wypisuje error
                MessageBox.Show(f.Message);
            }
			
			//odczytuje wiadomosc
            while (true)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    EndPoint ip = (EndPoint)endPoint;
					
					//bufor na dane
                    byte[] buffer = new byte[1024];

                    //oczekuje na wiadomość i blokuje wątek do czasu jej ottrzymania
                    s.ReceiveFrom(buffer, ref ip);

                    //wyświetla wiadomość
                    string wiadomosc = UnicodeEncoding.Unicode.GetString(buffer);
                    listBox1.Invoke(new Action(() => listBox1.Items.Add(wiadomosc + " - " + ip.ToString())));

                }
                catch (Exception f)
                {
					//wypisuje error
                    MessageBox.Show(f.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
			//odpala funbkcje nasluch jako nowy watek
            watek_nasluchu = new Thread(new ThreadStart(nasluch));
            watek_nasluchu.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            try
            {
				//probuje sakonczyc dzialanie socketu
                watek_nasluchu.Abort();
                s.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                s.Close();
				//probuje sakonczyc dzialanie socketu
                watek_nasluchu.Abort();
            }
            catch (Exception f)
            {
				//wypisuje error
               MessageBox.Show(f.Message);
            }
        }

    }
}
