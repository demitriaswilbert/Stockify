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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            ConfigureDataGridViewAtBottom();
        }
        private void ConfigureDataGridViewAtBottom()
        {
            // Enable both vertical and horizontal scrollbars for the DataGridView
            dataGridView2.ScrollBars = ScrollBars.Both;

            // Add the DataGridView to panel7 and position it at the bottom
            dataGridView2.Dock = DockStyle.Fill;
            panel4.Controls.Add(dataGridView2);
            panel4.AutoScroll = true; // Enable auto-scroll for panel7
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Form2().Show();
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

        SqlCommand CmdUserstock = Glb.CmdUserstock;
        private bool isProgrammaticChange = false;


        private void button16_Click(object sender, EventArgs e)
        {
            new Form8().Show();
            this.Hide();
        }

       private void button3_Click(object sender, EventArgs e)
{
    string itemCode = txtCode.Text;
    string txtInValue = txtIn.Text;

    if (!string.IsNullOrEmpty(itemCode))
    {
        try
        {
            int inQuantity = int.Parse(txtInValue);

            // Assuming you have a ComboBox named CmbProductName
            string selectedProductName = CmbName.SelectedItem.ToString();

            CmdUserstock.CommandText = @"
                INSERT INTO GoodsInDate1 (ItemCode, ProductName, GoodsIn, Date) 
                VALUES (@itemCode, @productName, @inQuantity, GETDATE());
                
                UPDATE ItemsData 
                SET Stock = Stock + @inQuantity 
                WHERE ItemCode = @itemCode";

            CmdUserstock.Parameters.Clear();
            CmdUserstock.Parameters.AddWithValue("@itemCode", itemCode);
            CmdUserstock.Parameters.AddWithValue("@productName", selectedProductName);
            CmdUserstock.Parameters.AddWithValue("@inQuantity", inQuantity);

            int rowsAffected = CmdUserstock.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                txtTotal.Text = (Convert.ToInt32(txtStock.Text) + inQuantity).ToString();
                MessageBox.Show("Goods In successfully added.");
            }
            else
            {
                MessageBox.Show("Failed to update stock quantity.");
            }
        }
        catch (FormatException)
        {
            MessageBox.Show("Please enter a valid number in TxtIn.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error updating stock quantity: " + ex.Message);
        }
        LoadTextField();
        LoadForm();
    }
    else
    {
        MessageBox.Show("Item Code is null or empty. Please select a product first.");
    }
}
       private void LoadTextField()
       {
           if (!isProgrammaticChange)
           {
               isProgrammaticChange = true;
               string itemCodeToSearch = txtCode.Text.Trim();

               if (!string.IsNullOrEmpty(itemCodeToSearch))
               {
                   // Handling Goods In
                   if (!string.IsNullOrEmpty(itemCodeToSearch))
                   {
                       // Use a parameterized query to retrieve data for goods in
                       CmdUserstock.CommandText = "SELECT * FROM ItemsData WHERE ItemCode = @itemCode";
                       CmdUserstock.Parameters.Clear();
                       CmdUserstock.Parameters.AddWithValue("@itemCode", itemCodeToSearch);

                       using (SqlDataReader RdUserstock = CmdUserstock.ExecuteReader())
                       {
                           if (RdUserstock.HasRows)
                           {
                               RdUserstock.Read();
                               txtBrand.Text = RdUserstock["Brand"].ToString();
                               txtUnit.Text = RdUserstock["Unit"].ToString();
                               txtPrice.Text = RdUserstock["Price"].ToString();
                               txtStock.Text = RdUserstock["Stock"].ToString();
                           }
                           else
                           {
                               txtBrand.Text = string.Empty;
                               txtUnit.Text = string.Empty;
                               txtPrice.Text = string.Empty;
                               txtStock.Text = string.Empty;
                           }
                       }
                       txtIn.Text = "";
                       txtTotal.Text = "";
                   }
                   else
                   {
                       // If the search box is empty, you may want to clear other textboxes
                       txtBrand.Text = string.Empty;
                       txtUnit.Text = string.Empty;
                       txtPrice.Text = string.Empty;
                       txtStock.Text = string.Empty;
                   }
                   isProgrammaticChange = false;
               }
           }
       }
       private void txtCode_TextChanged(object sender, EventArgs e)
       {
           LoadTextField();
       }

        private void Form4_Load(object sender, EventArgs e)
        {
            LoadProductNames();
            LoadForm();
        }

        private void LoadForm()
        {
            dataGridView2.DataSource = RetrieveItemsFromDatabase();
        }

        private void LoadGoodsInData()
        {
            try
            {
                // Assuming dataGridView1 is the name of your DataGridView control
                dataGridView2.AutoGenerateColumns = true; // Enable column generation based on data source

                // Use a SqlCommand to retrieve goods in information from the database
                CmdUserstock.CommandText = "SELECT * FROM GoodsInDate1";
                CmdUserstock.Parameters.Clear();

                DataTable goodsInTable = new DataTable();

                using (SqlDataAdapter adapter = new SqlDataAdapter(CmdUserstock))
                {
                    adapter.Fill(goodsInTable);
                }

                if (goodsInTable.Rows.Count > 0)
                {
                    // Bind the DataTable to the DataGridView
                    dataGridView2.DataSource = goodsInTable;
                }
                else
                {
                    MessageBox.Show("No data found in GoodsInDate table.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading goods in information: " + ex.Message);
            }
        }
        private void LoadProductNames()
        {
            try
            {
                // Use a SqlCommand to retrieve product names from the database
                CmdUserstock.CommandText = "SELECT ProductName FROM ItemsData";
                CmdUserstock.Parameters.Clear();

                using (SqlDataReader reader = CmdUserstock.ExecuteReader())
                {
                    // Clear existing items in the ComboBox
                    CmbName.Items.Clear();

                    // Loop through the result set and add product names to the ComboBox
                    while (reader.Read())
                    {
                        string productName = reader["ProductName"].ToString();
                        CmbName.Items.Add(productName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product names: " + ex.Message);
            }
        }
        private DataTable RetrieveItemsFromDatabase()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM GoodsInDate1", Glb.CmdUserstock.Connection))
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

      
        private void CmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isProgrammaticChange)
            {
                isProgrammaticChange = true;
                string selectedProductName = CmbName.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedProductName))
                {
                    try
                    {
                        // Use a SqlCommand to retrieve information for the selected product name
                        CmdUserstock.CommandText = "SELECT * FROM ItemsData WHERE ProductName = @productName";
                        CmdUserstock.Parameters.Clear();
                        CmdUserstock.Parameters.AddWithValue("@productName", selectedProductName);

                        using (SqlDataReader reader = CmdUserstock.ExecuteReader())
                        {
                            // Check if there are any rows in the result set
                            if (reader.HasRows)
                            {
                                // Read the first row (assuming there's only one match for a unique product name)
                                reader.Read();

                                // Populate textboxes with values from the database
                                txtCode.Text = reader["ItemCode"].ToString();
                                txtBrand.Text = reader["Brand"].ToString();
                                txtUnit.Text = reader["Unit"].ToString();
                                txtPrice.Text = reader["Price"].ToString();
                                txtStock.Text = reader["Stock"].ToString();
                            }
                        }
                        txtIn.Text = "";
                        txtTotal.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error retrieving product information: " + ex.Message);
                    }
                }
                isProgrammaticChange = false;
            }
        }
    }
}
