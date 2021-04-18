using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Курсовая
{
    public partial class Form1 : Form
    {
        GetTableNumber window = new GetTableNumber();

        bool read = false;

        bool exp = false;

        public readonly int size = 0;

        AVL Tree;
        Hash Table;
        Hash Table1;
        Hash Table2;

        Series avl;
        Series hash;
        Series hash1;
        Series hash2;

        public Form1()
        { 
                InitializeComponent();

            chart1.Visible = false;
            tableLayoutPanel12.RowStyles[1].Height = 0;
            this.Height = 500;

            if (window.ShowDialog() == DialogResult.OK)
            {
                size = window.Method();

                Tree = new AVL();
                Table = new Hash(size);
                Table1 = new Hash(10);
                Table2 = new Hash(500);

                chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                chart1.Titles.Add("График операций");
                chart1.Series.Clear();
                avl = chart1.Series.Add("АВЛ");
                hash = chart1.Series.Add("ХЕШ " + size);
                hash1 = chart1.Series.Add("ХЕШ 10");
                hash2 = chart1.Series.Add("ХЕШ 500");

                label14.Text = size + " элементов";
                label15.Text = "10 элементов";
                label16.Text = "500 элементов";

                label16.Visible = false;
                label12.Visible = false;
                label15.Visible = false;
                label13.Visible = false;

                avl.Points.AddXY(1, 0);
                hash.Points.AddXY(2, 0);
                hash1.Points.AddXY(3, 0);
                hash2.Points.AddXY(4, 0);

            }
            
        }

        public string CheckData(string s1, string s2, string s3)
        {
            char[] numbers = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            char[] symbols = {'A','B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
                'L', 'M', 'N', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

            string a1 = "";
            string a2 = "Год - не число.";
            string a3 = "Год меньше 1850";
            string a4 = "Год больше 2019";
            string a5 = "Не верная длина номера кузова";
            string a6 = "Не верные символы номера кузова";
            string a7 = "Модель слишком длинная";


            if (!int.TryParse(s2, out int i2))
                return a2;

            if (i2 < 1850)
                return a3;

            if (i2 > 2019)
                return a4;

            if(s1.Length != 17)
                return a5;

            for (int i = 0; i < 13; i++)
            {
                if (!symbols.Contains(s1[i]))
                    return a6;
            }

            for (int i = 13; i < 17; i++)
            {
                if (!numbers.Contains(s1[i]))
                    return a6;
            }

            if (s3.Length > 50)
                return a7;

            return a1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // if (read)
             //   return;

            StreamReader sr;

            string path;

            if (!exp)
                path = @".\Файл.txt";
            else
                path = @".\Файл1.txt";

            try
            {
                sr = new StreamReader(path);
            }
            catch
            {
                label6.Text = "Файл не найден.";
                return;
            }

            read = true;

            using (sr)
            {
                int n = 0;

                uint countT = 0;
                uint countT1 = 0;
                uint countT2 = 0;
                uint countP = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length != 3 || !int.TryParse(fields[1], out int s) || CheckData(fields[0], fields[1], fields[2]) != "" || Table.CallFind(fields[0], ref countT) != null ||
                      Table1.CallFind(fields[0], ref countT1) != null || Table2.CallFind(fields[0], ref countT2) != null || Tree.Found(fields[0], ref countP))
                    {
                        label6.ForeColor = Color.Red;
                        label6.Font = new Font(label6.Font.Name, 9, label6.Font.Style);
                        label6.Text = "Некорректные данные в файле." + Environment.NewLine;
                        label6.Text += "Добавлено " + n + " шт. записей.";
                        return;
                    }
                    Car car = new Car(fields[0], int.Parse(fields[1]), fields[2]);
                    Tree.AddP(new AVL.Node(car), ref countP);
                    Table.CallAdd(car, ref countT);
                    Table1.CallAdd(car, ref countT1);
                    Table2.CallAdd(car, ref countT2);

                    n++;
                }
                label7.Text = "Кол-во операций при добавлении из файла:";
                label9.Text = "Кол-во операций при добавлении из файла:";

                label4.Text = countT.ToString();
                label13.Text = countT1.ToString();
                label12.Text = countT2.ToString();
                label8.Text = countP.ToString();

                label6.ForeColor = Color.Green;
                label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                label6.Text = "Добавлено " + n + " шт. записей.";

                avl.Points.Clear();
                hash.Points.Clear();
                hash1.Points.Clear();
                hash2.Points.Clear();

                avl.Points.AddXY(1, countP);
                hash.Points.AddXY(2, countT);
                hash1.Points.AddXY(3, countT1);
                hash2.Points.AddXY(4, countT2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            uint countT = 0;
            uint countT1 = 0;
            uint countT2 = 0;
            uint countP = 0;

            if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "") {

                string a = CheckData(textBox2.Text, textBox3.Text, textBox4.Text);


                Table.CallFind(textBox2.Text, ref countT);
                Table1.CallFind(textBox2.Text, ref countT1);
                Table2.CallFind(textBox2.Text, ref countT2);

                if (a == "")
                    if (!Tree.Found(textBox2.Text, ref countP))
                    {
                        if (!int.TryParse(textBox3.Text, out int s))
                        {
                            countP = 0;
                            label6.Text = "Вы ввели некорректные данные.";
                            return;
                        }

                        Car car = new Car(textBox2.Text, int.Parse(textBox3.Text), textBox4.Text);

                        Tree.AddP(new AVL.Node(car), ref countP);
                        Table.CallAdd(car, ref countT);
                        Table1.CallAdd(car, ref countT1);
                        Table2.CallAdd(car, ref countT2);
                        label6.ForeColor = Color.Green;
                        label6.Font = new Font(label6.Font.Name, 15, label6.Font.Style);
                        label6.Text = "Запись добавлена.";
                        textBox2.Text = null;
                        textBox3.Text = null;
                        textBox4.Text = null;
                    }
                    else
                    {
                        label6.ForeColor = Color.Red;
                        label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                        label6.Text = "Эта запись уже есть в базе.";
                    }
                else
                {
                    label6.Text = a;
                }
            }
            else
            {
                label6.ForeColor = Color.Red;
                label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                label6.Text = "Не все поля заполены.";
            }
            label9.Text = "Кол-во операций при добавлении:";
            label7.Text = "Кол-во операций при добавлении:";

            label4.Text = countT.ToString();
            label13.Text = countT1.ToString();
            label12.Text = countT2.ToString();
            label8.Text = countP.ToString();

            avl.Points.Clear();
            hash.Points.Clear();
            hash1.Points.Clear();
            hash2.Points.Clear();

            avl.Points.AddXY(1, countP);
            hash.Points.AddXY(2, countT);
            hash1.Points.AddXY(3, countT1);
            hash2.Points.AddXY(4, countT2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            uint countT = 0;
            uint countT1 = 0;
            uint countT2 = 0;
            uint countP = 0;

            if (textBox5.Text != "")
            {
                Car car;

                label6.ForeColor = Color.Green;
                label6.Font = new Font(label6.Font.Name, 9, label6.Font.Style);
                AVL.Node tmp = Tree.Show(textBox5.Text, ref countP);
                Table.CallFind(textBox5.Text, ref countT);
                car = Table1.CallFind(textBox5.Text, ref countT1);
                Table2.CallFind(textBox5.Text, ref countT2);

                if (tmp != null)
                {
                    label6.Text = tmp.get_num() + " " + tmp.get_year() + " " + tmp.get_name();
                }
                else
                {
                    label6.ForeColor = Color.Red;
                    label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                    label6.Text = "Такой записи нет.";

                }

            }
            else
            {
                label6.ForeColor = Color.Red;
                label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                label6.Text = "Не все поля заполены.";

            }

            label9.Text = "Кол-во операций при поиске:";
            label7.Text = "Кол-во операций при поиске:";

            label4.Text = countT.ToString();
            label13.Text = countT1.ToString();
            label12.Text = countT2.ToString();
            label8.Text = countP.ToString();

            avl.Points.Clear();
            hash.Points.Clear();
            hash1.Points.Clear();
            hash2.Points.Clear();

            avl.Points.AddXY(1, countP);
            hash.Points.AddXY(2, countT);
            hash1.Points.AddXY(3, countT1);
            hash2.Points.AddXY(4, countT2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            uint countT = 0;
            uint countT1 = 0;
            uint countT2 = 0;
            uint countP = 0;

            if (textBox5.Text != "")
            {
                label6.ForeColor = Color.Green;
                label6.Font = new Font(label6.Font.Name, 15, label6.Font.Style);

                AVL.Node tmp = Tree.Show(textBox5.Text, ref countP);
                Table.CallFind(textBox5.Text, ref countT);
                Table1.CallFind(textBox5.Text, ref countT1);
                Table2.CallFind(textBox5.Text, ref countT2);

                if (tmp != null)
                {
                    string s = tmp.get_num();

                    Table.CallDel(s, ref countT);
                    Table1.CallDel(s, ref countT1);
                    Table2.CallDel(s, ref countT2);
                    Tree.Del(tmp.get_num(), ref countP);

                    label6.ForeColor = Color.Green;
                    label6.Font = new Font(label6.Font.Name, 15, label6.Font.Style);
                    label6.Text = "Все отлично!";
                }
                else
                {
                    label6.ForeColor = Color.Red;
                    label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                    label6.Text = "Такой машины нет.";
                }

            }
            else
            {
                label6.ForeColor = Color.Red;
                label6.Font = new Font(label6.Font.Name, 12, label6.Font.Style);
                label6.Text = "Не все поля заполены.";
            }

            label7.Text = "Кол-во операций при удалении:";
            label9.Text = "Кол-во операций при удалении:";

            label4.Text = countT.ToString();
            label13.Text = countT1.ToString();
            label12.Text = countT2.ToString();
            label8.Text = countP.ToString();

            avl.Points.Clear();
            hash.Points.Clear();
            hash1.Points.Clear();
            hash2.Points.Clear();

            avl.Points.AddXY(1, countP);
            hash.Points.AddXY(2, countT);
            hash1.Points.AddXY(3, countT1);
            hash2.Points.AddXY(4, countT2);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            AllElements AllE = new AllElements();

            AllE.Clear();
            AllE.Show();

            Table.CallAllElements(AllE);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            Table.CallSaveFile();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "Включить эксперементы")
            {
                exp = true;

                avl.Points.Clear();
                hash.Points.Clear();
                hash1.Points.Clear();
                hash2.Points.Clear();

                label16.Visible = true;
                label12.Visible = true;
                label15.Visible = true;
                label13.Visible = true;

                chart1.Visible = true;
                tableLayoutPanel12.RowStyles[1].Height = 79.82456F;
                this.Height = 670;
                button6.Text = "Выключить эксперементы";
            }
            else
            {
                exp = false;

                label16.Visible = false;
                label12.Visible = false;
                label15.Visible = false;
                label13.Visible = false;

                chart1.Visible = false;
                tableLayoutPanel12.RowStyles[1].Height = 0;
                this.Height = 500;
                button6.Text = "Включить эксперементы";
            }
        }
    }
}
