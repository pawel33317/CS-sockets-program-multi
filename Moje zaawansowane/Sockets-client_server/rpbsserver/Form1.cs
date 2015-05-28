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
using System.Threading;
using System.IO;

namespace rpbsserver
{
    public partial class Form1 : Form
    {
        Socket sck, acc;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //odpalamy socket
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //ustawiamy port i ip
            sck.Bind(new IPEndPoint(0,8888));

            //włączamy nasłuchiwanie
            sck.Listen(0);

            //tworzymy połączenie z klientem
            acc = sck.Accept();

            //zamykamy nasłuch
            sck.Close();

            new Thread(() =>
            {
                while (true)
                {
                    //tablica na rozmiar danych 
                    byte[] sizeBuf = new byte[4];


                    //odbieramy rozmiar danych
                    acc.Receive(sizeBuf, 0, sizeBuf.Length, 0);
                    
                    //zapisujemy do "size" rozmiar danych
                    int size = BitConverter.ToInt32(sizeBuf, 0);

                    //Stream na dane
                    MemoryStream ms = new MemoryStream();

                    //jeżeli rozmiar większy od 0 czyli wysłano dane
                    while (size > 0)
                    {
                        //tablica na dane
                        byte[] buffer;
                        
                        //jeżeli dane są mniejsze od maksymalnego dopuszczalnego rozmiaru buffora wysyłanych danych
                        //buffer otrzymuje rozmiar na te dane
                        if (size < acc.ReceiveBufferSize)
                        {
                            buffer = new byte[size];
                        }
                        //jeżeli dane są większe od maksymalnego dopuszczalnego rozmiaru buffora wysyłanych danych
                        //buffer otrzymuje rozmiar maksymalny buffora wysyłu danych
                        else
                        {
                            buffer = new byte[acc.ReceiveBufferSize];
                        }

                        //zapisujemy do buffer otrzymane dane i ich rozmiar do rec
                        int rec = acc.Receive(buffer,0,buffer.Length,0);

                        //zmniejszamy całkowity rozmiar danych o te które już otrzymaliśmy
                        //jeżeli wykonał się if a nie else to size = 0 i koniec pętli 
                        //jeżeli wykonał się else i sieze != acc.ReceiveBufferSize to nastąpi kolejne wykonanie petli 
                        //czyli odabranie kolejnego fragmentu danych
                        size -= rec;

                        //DOPISUJEMY do stream ms  otrzymane dane ()
                        ms.Write(buffer,0,buffer.Length);
                    }

                    //zamykamy stream
                    ms.Close();

                    //zapisujemy wszystkie odebrane dane do tablicy
                    byte[] data = ms.ToArray();

                    //czyścimy pamięć po ms
                    ms.Dispose();

                    //wyświetlamy otrzymane dane
                    Invoke((MethodInvoker)delegate
                    {
                        richTextBox1.Text = Encoding.Default.GetString(data);
                    });

                }
            }).Start();
        }
    }
}
