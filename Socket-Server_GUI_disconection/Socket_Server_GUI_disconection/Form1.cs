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
namespace Socket_Server_GUI_disconection
{
    public partial class Form1 : Form
    {
        Socket sock;
        public Form1()
        {
            InitializeComponent();
            //tworzymy gniazdo
            sock = socket();
            //dodajemy metodę obsługującą zamykanie aplikacji
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //podczas zamykania aplikacji dorzucamy zamknięcie gniazda
            sock.Close();
        }

        //metoda do tworzenia nowych gniazd
        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //po wciśnięciu przycisku tworzymy pole z danymi -> wysyłamy dane do serwera -> czyścimy textboxa
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes("S: "+textBox2.Text);
            sock.Send(data, 0, data.Length, 0);
            textBox2.Clear();
        }

        void read()
        {
            //dane będą odbierane aż połączenie nie będzie zakończone wykonywane w nowym wątku
            while (true)
            {
                try
                {
                    //blok na dane do otrzymania
                    byte[] buffer = new byte[255];
                    //przypisanie otrzymanych danych do buffer
                    int rec = sock.Receive(buffer, 0, buffer.Length, 0);

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
                        listBox1.Items.Add(Encoding.Default.GetString(buffer));
                    });
                }
                catch
                {
                    //jeżeli połączenie zakończone to info -> opóźnienie -> zamknięcie aplikacji
                    //w przeciwieństwie do serwera dodany jest break; gdyż tu blok catch znajduje się w "nieskończonej" pętli oczywiście można zrobić tak jak w serwerze
                    listBox1.Items.Add("Rozłączono");
                    Thread.Sleep(2000);
                    sock.Close();
                    Application.Exit();
                    break;
                }
                
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //próba połączenia do serwera 
                sock.Connect(new IPEndPoint(IPAddress.Parse(textBox1.Text), 3333));
                listBox1.Items.Add("Połączono");
                //tworzymy nowy wątek w którym będzie wykonywałą się metoda read();  ()=>{} jest równoznaczne z delegate(){} opisane w serwerze
                new Thread(()=>{
                    read();
                }).Start();
            }
            catch
            {
                //jeżeli próba połączenia nieudana info
                listBox1.Items.Add("Nie można połączyć");
            }
        }
    }
}
