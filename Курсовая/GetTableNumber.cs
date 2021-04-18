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
    public partial class GetTableNumber: Form
    {
        public GetTableNumber()
        {
            InitializeComponent();

            textBox1.Focus();
        }

        private int res = 10;

        public int Method()
        {
                return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out res))
            {
                errorProvider1.SetError(textBox1, "Некорректные данные.");
                return;
            }
            if (res < 1)
            {
                errorProvider1.SetError(textBox1, "Некорректные данные.");
                return;
            }

            errorProvider1.Clear();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out res))
            {
                e.Cancel = true;
                return;
            }
            if (res < 1)
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
