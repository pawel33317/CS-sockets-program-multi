using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Socket_Client_GUI_disconection
{
    public partial class Form1 : Form
    {
        Socket sock, acc;
        public Form1()
        {
            InitializeComponent();
        }
        //metoda do tworzenia nowych gniazd
        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tworzymy gniazdo
            sock = socket();

            //uruchamiamy gniazdo na ip 127.0.0.1 i porcie 333
            sock.Bind(new IPEndPoint(0,3333));
            listBox1.Items.Add("Rozpoczęto nasłuchiwanie");

            //włączamy nasłuchiwanie
            sock.Listen(0);

            //w nowym wątku odpalamy delegatę (nie możemy odpalić samego ciągu instrukcji w wątku lecz jedynie metody i delegaty)
            //tworzymy nowy wątek (mimo iż jest to serwer obsługujący tylko 1 klienta) aby możliwe było jednoczesne wysyłanie i odbieranie danych
            new Thread(delegate()
                {
                    //gniazdo do odbierania danych
                    acc = sock.Accept();

                    //info
                    listBox1.Items.Add("Połączono");

                    //zamykamy gniazdo nasłuchiwania gdyż jest to single server i nie będzie obsługiwał > 1 klientów
                    sock.Close();
                    try
                    {
                        //dane będą odbierane aż połączenie nie będzie zakończone
                        while (true)
                        {
                            //blok na dane
                            byte[] buffer = new byte[255];
                            //odbieramy dane do buffer a do rec ich długość
                            int rec = acc.Receive(buffer, 0, buffer.Length, 0);

                            //jeżeli połączenie zostało zakończone (w C# rec spełni właśnie ten warunek) rzucemy wyjątek
                            if (rec <= 0)
                            {
                                throw new SocketException();
                            }

                            //zmniejszamy buffer do rozmiarów danych
                            Array.Resize(ref buffer, rec);

                            //invoke służy do wywołania instrukcji z wątku głównego z którego ten został wywołany bez tego bardzo prawdopodobne było by 
                            //wysypanie się programu przy próbie operacji na obiektach/metodach głównego wątku
                            Invoke((MethodInvoker)delegate
                            {
                                //wypisujemy otrzymane dane
                                listBox1.Items.Add(Encoding.Default.GetString(buffer));
                            });
                        }
                    }
                    catch
                    {
                        //jeżeli połączenie zakończone to info -> opóźnienie -> zamknięcie aplikacji
                        listBox1.Items.Add("Rozłączono");
                        Thread.Sleep(2000);
                        Application.Exit();
                    }
                }
            ).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //po wciśnięciu przycisku tworzymy pole z danymi -> wysyłamy dane do klienta -> czyścimy textboxa
            byte[] data = Encoding.Default.GetBytes("K: "+textBox1.Text);
            acc.Send(data, 0, data.Length, 0);
            textBox1.Clear();
        }
    }
}
