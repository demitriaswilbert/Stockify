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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            txtUsernama.Text = "Username"; // Atur teks default di awal
            txtUsernama.Enter += txtUsernama_Enter;
            txtPassw.Text = "Password"; // Atur teks default di awal
            txtPassw.Enter += txtPassw_Enter;
            txtPassc.Text = "Confirm Password"; // Atur teks default di awal
            txtPassc.Enter += txtPassc_Enter; 
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }




        SqlCommand Cmduserstock = Glb.CmdUserstock;
        SqlDataReader Rduserstock;
        private void btnSignup_Click(object sender, EventArgs e)
        {
            if (txtUsernama.Text == "" || txtPassw.Text == "" || txtPassc.Text == "")
            {
                MessageBox.Show("Tolong isi username dan password!");
                return;
            }
            if (txtPassw.Text != txtPassc.Text)
            {
                MessageBox.Show("Confirm password salah!");
                txtPassw.Text = "";
                txtPassc.Text = "";
                return;
            }
            Cmduserstock.CommandText = @"Select * From userstock Where username = @username";
            Cmduserstock.Parameters.Clear();
            Cmduserstock.Parameters.AddWithValue("@username", txtUsernama.Text);
            Rduserstock = Cmduserstock.ExecuteReader();
            while (Rduserstock.Read())
            {
                
                MessageBox.Show("Username sudah dipakai!");
                Rduserstock.Close();
                return;
            }
            Rduserstock.Close();
            Cmduserstock.CommandText = @"INSERT INTO userstock(username, password) VALUES(@username, @password)";
            Cmduserstock.Parameters.Clear();
            Cmduserstock.Parameters.AddWithValue("@username", txtUsernama.Text);
            Cmduserstock.Parameters.AddWithValue("@password", txtPassw.Text);
            Cmduserstock.ExecuteNonQuery();
            MessageBox.Show("Akun berhasil dibuat!");
            new Form1().Show();
            this.Hide();
            
        }

        private void txtUsernama_Enter(object sender, EventArgs e)
        {
            if (txtUsernama.Text == "Username") // Ganti "username" dengan teks default yang Anda gunakan
            {
                txtUsernama.Text = "";
            }
        }

        private void txtPassw_Enter(object sender, EventArgs e)
        {
            if (txtPassw.Text == "Password") // Ganti "username" dengan teks default yang Anda gunakan
            {
                txtPassw.Text = "";
            }
        }

        private void txtPassc_Enter(object sender, EventArgs e)
        {
            if (txtPassc.Text == "Confirm Password") // Ganti "username" dengan teks default yang Anda gunakan
            {
                txtPassc.Text = "";
            }
        }
    }
}
