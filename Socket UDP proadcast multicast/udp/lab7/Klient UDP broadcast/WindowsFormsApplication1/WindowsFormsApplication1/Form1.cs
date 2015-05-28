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
        Thread watek_nasluchu;
        Socket s;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			//blokuje przycisk
            button2.Enabled = false;
        }

        void nasluch()
        {
            //udp socket
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //0.0.0.0:7
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt16(numericUpDown1.Value));
            
            //otwiera port 7
            try
            {
				//ustawia socket na tryb broadcastowy
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
				//powiązuje socket z konkretnym ip i portem i włącza nasluchiwanie na dane
                s.Bind(endPoint);
            }
            catch (Exception f)
            {
				//wyswietla error
                MessageBox.Show(f.Message);
            }

            //oczekuje na wiadomość
            while (true)
            {
                try
                {
					
                    StringBuilder sb = new StringBuilder();
					
                    EndPoint ip = (EndPoint)endPoint;
                    
                    //bifor na dane
                    byte[] buffer = new byte[1024];
					
					
					//oczekuje na wiadomość i blokuje wątek
                    s.ReceiveFrom(buffer, ref ip);

                    //wypisanie wiadomości przez delegatę
                    string wiadomosc = UnicodeEncoding.Unicode.GetString(buffer);
					//wyswietlaw panelu
                    listBox1.Invoke(new Action( () => listBox1.Items.Add(ip.ToString() + ": " + wiadomosc) ));

                }
                catch (Exception f)
                {
					//wywala error
                    MessageBox.Show(f.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
			
			
			//odpala finkcje nasluch jako nowy watek
            watek_nasluchu = new Thread(new ThreadStart(nasluch));
            watek_nasluchu.Start();
			
            
        }

        //WYŁĄCZANIE NASŁUCHIWANIA//
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            try
            {
				//probuje zakonczyc socket
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
                s.Close();
				//zakoncza socket
                watek_nasluchu.Abort();
        }
    }
}
