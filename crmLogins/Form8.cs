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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            ConfigureDataGridViewAtBottom();
        }
        private void ConfigureDataGridViewAtBottom()
        {
            // Enable both vertical and horizontal scrollbars for the DataGridView
            dataGridView1.ScrollBars = ScrollBars.Both;

            // Add the DataGridView to panel7 and position it at the bottom
            dataGridView1.Dock = DockStyle.Fill;
            panel3.Controls.Add(dataGridView1);
            panel3.AutoScroll = true; // Enable auto-scroll for panel7
             dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form7().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new Form7().Show();
            this.Hide();
        }
        SqlCommand CmdUserstock = Glb.CmdUserstock;

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string itemCode = txtCode.Text;
                string productName = txtName.Text;
                string brand = txtBrand.Text;
                string unit = txtUnit.Text;
                decimal price = Convert.ToDecimal(txtPrice.Text); // Assuming the price is a decimal
                int stock = Convert.ToInt32(txtStock.Text); // Assuming the stock is an integer

                CmdUserstock.CommandText = @"SELECT * FROM ItemsData WHERE ItemCode = @itemCode";
                CmdUserstock.Parameters.Clear();
                CmdUserstock.Parameters.AddWithValue("@itemCode", itemCode);

                using (SqlDataReader RdUserstock = CmdUserstock.ExecuteReader())
                {
                    // Check if there are any rows in the result set
                    if (RdUserstock.HasRows)
                    {
                        MessageBox.Show("Item code already exists!");
                        return; // Exit the method or function
                    }
                }

                // SQL query with parameters to avoid SQL injection
                CmdUserstock.Parameters.Clear();
                CmdUserstock.CommandText = "INSERT INTO ItemsData (ItemCode, ProductName, Brand, Unit, Price, Stock) VALUES (@ItemCode, @ProductName, @Brand, @Unit, @Price, @Stock)";
                CmdUserstock.Parameters.AddWithValue("@ItemCode", itemCode);
                CmdUserstock.Parameters.AddWithValue("@ProductName", productName);
                CmdUserstock.Parameters.AddWithValue("@Brand", brand);
                CmdUserstock.Parameters.AddWithValue("@Unit", unit);
                CmdUserstock.Parameters.AddWithValue("@Price", price);
                CmdUserstock.Parameters.AddWithValue("@Stock", stock);
                CmdUserstock.ExecuteNonQuery();

                MessageBox.Show("Item successfully added!");

                // Clear the form after successful addition
                txtCode.Text = "";
                txtName.Text = "";
                txtBrand.Text = "";
                txtUnit.Text = "";
                txtPrice.Text = "";
                txtStock.Text = "";

                LoadForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding item: " + ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            // Get the selected item's information
            int selectedIndex = dataGridView1.SelectedRows[0].Index;
            string itemCodeToDelete = dataGridView1.Rows[selectedIndex].Cells["ItemCode"].Value.ToString();
            string itemNameToDelete = dataGridView1.Rows[selectedIndex].Cells["ProductName"].Value.ToString(); // Change this column name to match yours

            // Ask for confirmation before deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete the item ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                CmdUserstock.CommandText = "DELETE FROM ItemsData WHERE ItemCode = @itemCode";
                CmdUserstock.Parameters.Clear();
                CmdUserstock.Parameters.AddWithValue("@itemCode", itemCodeToDelete);

                int rowsAffected = CmdUserstock.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    dataGridView1.Rows.RemoveAt(selectedIndex); // Remove the row from the DataGridView
                    MessageBox.Show("Item deleted successfully.");
                    // Additional actions after successful deletion
                }
                else
                {
                    MessageBox.Show("No item found to delete.");
                }
            }
        }
        else
        {
            MessageBox.Show("Please select an item to delete.");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error deleting item: " + ex.Message);
    }
        }

        private DataTable RetrieveItemsFromDatabase()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM ItemsData", Glb.CmdUserstock.Connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving items: " + ex.Message);
            }

            return dt;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void LoadForm()
        {
            dataGridView1.DataSource = RetrieveItemsFromDatabase();
            dataGridView1.Sort(dataGridView1.Columns["itemCode"], ListSortDirection.Ascending);
        }
    }
}
