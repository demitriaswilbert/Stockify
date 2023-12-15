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
    public partial class Form5 : Form
    {
        public Form5()
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
            new Form4().Show();
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

        private void button5_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

        SqlCommand CmdUserstock = Glb.CmdUserstock;
        private bool isProgrammaticChange = false;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string itemCode = txtCode.Text;
            string txtOutValue = txtOut.Text;

            if (!string.IsNullOrEmpty(itemCode))
            {
                try
                {
                    int outQuantity = int.Parse(txtOutValue);

                    int currentStock = Convert.ToInt32(txtStock.Text);

                    if (currentStock >= outQuantity)
                    {
                        string selectedProductName = GetProductNameByItemCode(itemCode);

                        if (!string.IsNullOrEmpty(selectedProductName))
                        {
                            CmdUserstock.CommandText = @"
                        INSERT INTO GoodsOutDate1 (ItemCode, ProductName, GoodsOut, Date) 
                        VALUES (@itemCode, @productName, @outQuantity, GETDATE());
                        
                        UPDATE ItemsData 
                        SET Stock = Stock - @outQuantity 
                        WHERE ItemCode = @itemCode AND Stock >= @outQuantity";

                            CmdUserstock.Parameters.Clear();
                            CmdUserstock.Parameters.AddWithValue("@outQuantity", outQuantity);
                            CmdUserstock.Parameters.AddWithValue("@itemCode", itemCode);
                            CmdUserstock.Parameters.AddWithValue("@productName", selectedProductName);

                            int rowsAffected = CmdUserstock.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                txtTotal.Text = (currentStock - outQuantity).ToString();
                                MessageBox.Show("Goods Out successfully processed.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to update stock quantity. Not enough stock available.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Selected product name not found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Goods Out quantity exceeds available stock.");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid number in TxtOut.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating stock quantity: " + ex.Message);
                }

                LoadData();
                UpdateTextField();
            }
            else
            {
                MessageBox.Show("Item Code is null or empty. Please select a product first.");
            }
        }
        private string GetProductNameByItemCode(string itemCode)
        {
            string productName = string.Empty;

            try
            {
                // Use a SqlCommand to retrieve the product name based on the item code
                CmdUserstock.CommandText = "SELECT ProductName FROM ItemsData WHERE ItemCode = @itemCode";
                CmdUserstock.Parameters.Clear();
                CmdUserstock.Parameters.AddWithValue("@itemCode", itemCode);

                object result = CmdUserstock.ExecuteScalar();
                if (result != null)
                {
                    productName = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving product name: " + ex.Message);
            }

            return productName;
        }

        private void UpdateTextField()
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
                        txtOut.Text = "";
                        txtTotal.Text = "";
                    }
                    // Handling Goods Out
                    else /* Condition for Goods Out, e.g., checking a flag or a specific button clicked */
                    {
                        // Code for handling goods out when the item code changes
                        // This part will be similar to the logic for goods in but for goods out
                    }
                }
                else
                {
                    txtBrand.Text = string.Empty;
                    txtUnit.Text = string.Empty;
                    txtPrice.Text = string.Empty;
                    txtStock.Text = string.Empty;
                }
                isProgrammaticChange = false;
            }
        }
        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            UpdateTextField();
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

                                // Populate textboxes with values from the database for goods out
                                // Replace this part with the logic for handling goods out
                                // For example, set the appropriate text boxes with data for goods out

                                 txtCode.Text = reader["ItemCode"].ToString();
                                 txtBrand.Text = reader["Brand"].ToString();
                                 txtUnit.Text = reader["Unit"].ToString();
                                 txtPrice.Text = reader["Price"].ToString();
                                 txtStock.Text = reader["Stock"].ToString();
                            }
                        }
                        // Clear fields related to goods in
                        txtOut.Text = "";
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

        private void Form5_Load(object sender, EventArgs e)
        {
            // Assuming you want to load product names specifically for goods out on Form5 load
            LoadData();
        }

        private void LoadData()
        {
            LoadProductNamesForGoodsOut();
            LoadGoodsOutData();
        }
        private void LoadGoodsOutData()
        {
            try
            {
                // Use a SqlCommand to retrieve goods out information from the database
                CmdUserstock.CommandText = "SELECT * FROM GoodsOutDate1";
                CmdUserstock.Parameters.Clear();

                DataTable goodsOutTable = new DataTable();

                using (SqlDataAdapter adapter = new SqlDataAdapter(CmdUserstock))
                {
                    adapter.Fill(goodsOutTable);
                }

                if (goodsOutTable.Rows.Count > 0)
                {
                    // Bind the DataTable to the DataGridView
                    dataGridView2.DataSource = goodsOutTable;
                }
                else
                {
                    MessageBox.Show("No data found in GoodsOutDate1 table.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading goods out information: " + ex.Message);
            }
        }
        private void LoadProductNamesForGoodsOut()
        {
            try
            {
                // Modify the query to select product names specifically for goods out
                CmdUserstock.CommandText = "SELECT ProductName FROM ItemsData WHERE Stock > 0";
                CmdUserstock.Parameters.Clear();

                using (SqlDataReader reader = CmdUserstock.ExecuteReader())
                {
                    CmbName.Items.Clear();

                    while (reader.Read())
                    {
                        string productName = reader["ProductName"].ToString();
                        CmbName.Items.Add(productName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product names for goods out: " + ex.Message);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }
    
        }
        
    }

