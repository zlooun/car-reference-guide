using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    class Car
    {
        public string num { get; set; }
        public int year { get; set; }
        public string name { get; set; }

        public Car(string z, int x, string c)
        {
            num = z;
            year = x;
            name = c;
        }
    }
}
