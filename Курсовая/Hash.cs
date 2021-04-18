using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    class Hash
    {
        public int size = 0;

        private Hlist[] table;

        public Hash(int size)
        {
            table = new Hlist[size];

            for (int i = 0; i < size; i++)
            {
                table[i] = new Hlist();
            }

            this.size = size;
        }

        class Hlist
        {
            private NodeOfTable list;

            public Hlist()
            {
                list = null;
            }

            class NodeOfTable
            {
                public NodeOfTable Prev { get; set; }
                public NodeOfTable Next { get; set; }
                public Car Car { get; set; }

                public NodeOfTable(Car car)
                {
                    Car = car;

                }
            }

            bool More(string s, string v)
            {
                int s1 = s.Length;
                int v1 = v.Length;
                int j;

                if (s1 > v1) { j = v1; }
                else { j = s1; }

                for (int i = 0; i < j; i++)
                {
                    if (s[i] > v[i]) { return true; }
                    if (s[i] < v[i]) { return false; }
                }

                if (s1 < v1)
                    return false;
                else
                    return true;
            }

            public void AddT(Car car, ref uint count)
            {
                NodeOfTable newCar = new NodeOfTable(car);
                NodeOfTable tmpCar = list;


                list = newCar;

                if (tmpCar != null)
                {
                    newCar.Next = tmpCar;

                    tmpCar.Prev = newCar;

                    count++;
                }

                count++;

                return;
            }

            public void DelT(string n, ref uint count)
            {
                NodeOfTable tmpCar = list;

                if (tmpCar.Car.num == n)
                {
                    list = tmpCar.Next;

                    count++;

                    return;
                }

                while (tmpCar.Next.Car.num != n)
                {
                    tmpCar = tmpCar.Next;
                }

                tmpCar.Next = tmpCar.Next.Next;

                count++;

                if (tmpCar.Next != null)
                {
                    tmpCar.Next.Prev = tmpCar;

                    count++;
                }
            }

            public Car FindT(string n, ref uint count)
            {
                NodeOfTable tmpCar = list;

                while (tmpCar != null && n != tmpCar.Car.num)
                {
                    tmpCar = tmpCar.Next;

                    count++;
                }

                count++;

                if (tmpCar != null && tmpCar.Car.num == n)
                    return tmpCar.Car;
                else
                    return null;
            }

            public void AllElements(AllElements AllE, int i)
            {
                NodeOfTable tmp = list;

                    while (tmp != null)
                    {
                        AllE.PullIt("Index[" + i + "]" + " " + tmp.Car.num + " " + tmp.Car.year + " " + tmp.Car.name);

                        tmp = tmp.Next;
                    }

            }

            public void SaveFile(StreamWriter sr)
            {
                NodeOfTable tmp = list;

                while (tmp != null)
                {
                    sr.WriteLine(tmp.Car.num + " " + tmp.Car.year + " " + tmp.Car.name);

                    tmp = tmp.Next;
                }
            }
        }

        private int HashFunction(string x)
        {
            float y = 0;

            for (int i = 0; i < x.Length; i++)
            {
                y += x[i] * (i + 1);
            }

            return Convert.ToInt32(Math.Floor(y % size));
        }

        public void CallAdd(Car car, ref uint count)
        {
            table[HashFunction(car.num)].AddT(car, ref count);
        }

        public void CallDel(string n, ref uint count)
        {
            table[HashFunction(n)].DelT(n, ref count);
        }

        public Car CallFind(string n, ref uint count)
        {
            return table[HashFunction(n)].FindT(n, ref count);
        }

        public void CallAllElements(AllElements AllE)
        {

            for (int i = 0; i < size; i++)
            {
                table[i].AllElements(AllE, i);
            }

        }

        public void CallSaveFile()
        {
            StreamWriter sr;

            string path = @".\Файл.txt";

            try
            {
                sr = new StreamWriter(path);
            }
            catch
            {
                return;
            }

            using (sr)
            {
                for (int i = 0; i < size; i++)
                {
                    table[i].SaveFile(sr);
                }
            }
        }

    }
}
