using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multiThreadServerv2
{
    class Program
    {
        static void Main(string[] args)
        {
            //odpala klasę windows.form 
            Server s = new Server();
            //zamiast show, żeby czekało na zakończenie
            s.ShowDialog();
        }
    }
}
