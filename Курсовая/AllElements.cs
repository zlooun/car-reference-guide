using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая
{
    public partial class AllElements : Form
    {
        public AllElements()
        {
            InitializeComponent();
        }

        public void PullIt(string s)
        {
            textBox1.Text += s + Environment.NewLine;
        }

        public void Clear() {textBox1.Text = string.Empty;}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
