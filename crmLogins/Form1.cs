using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace crmLogins
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtUsername.Text = "Username"; // Atur teks default di awal
            txtUsername.Enter += txtUsername_Enter;
            txtPass.Text = "Password"; // Atur teks default di awal
            txtPass.Enter += txtPass_Enter; 
        }

        SqlCommand Cmduserstock = Glb.CmdUserstock;
        SqlDataReader Rduserstock;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cmduserstock.CommandText = @"Select * From userstock Where username = @username AND password = @password";
            Cmduserstock.Parameters.Clear();
            Cmduserstock.Parameters.AddWithValue("@username", txtUsername.Text);
            Cmduserstock.Parameters.AddWithValue("@password", txtPass.Text);
            Rduserstock = Cmduserstock.ExecuteReader();
            while (Rduserstock.Read())
            {
                new Form2().Show();
                this.Hide();
                Rduserstock.Close();
                return;
            }

            MessageBox.Show("The username or password you entered is incorrect, try again!");
            txtUsername.Clear();
            txtPass.Clear();
            txtUsername.Focus();
            Rduserstock.Close();

        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username") // Ganti "username" dengan teks default yang Anda gunakan
            {
                txtUsername.Text = "";
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "Password") // Ganti "username" dengan teks default yang Anda gunakan
            {
                txtPass.Text = "";
            }
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
