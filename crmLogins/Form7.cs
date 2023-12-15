using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crmLogins
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            new Form8().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }
    }
}
