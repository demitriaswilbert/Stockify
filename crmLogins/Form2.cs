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
    public partial class Form2 : Form
    {
        private string loggedInUsername = "";
        public Form2()
        {
            InitializeComponent();
            
        }

      
        private void button9_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Form7().Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            new Form8().Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new Form7().Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

       
    }
}
