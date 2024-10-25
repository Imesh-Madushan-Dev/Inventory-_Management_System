using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class uc_Manage : UserControl
    {
        public uc_Manage()
        {
            InitializeComponent();
            AutoIncrementProductId();
        }

        private void ClearTextBoxes()
        {
            txtProductID.Clear();
            txtProductName.Clear();
            txtSupplier.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
        }

        String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";

        private void AutoIncrementProductId()
        {        
              using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ISNULL(MAX(CAST(ProductID AS INT)), 0) + 1 FROM Products"; 
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int newProductId = Convert.ToInt32(cmd.ExecuteScalar());
                        txtProductID.Text = newProductId.ToString(); 
                    }
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string productId = txtProductID.Text;
            string productName = txtProductName.Text;
            string category = cmbCategory.Text;
            string supplier = txtSupplier.Text;
            string unitPrice = txtUnitPrice.Text;
            string quantity = txtQuantity.Text;

            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(unitPrice) || string.IsNullOrEmpty(quantity))
            {
                MessageBox.Show("Please fill all required fields.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Products (ProductID, ProductName, Category, Supplier, UnitPrice, Quantity) " +
                                   "VALUES (@ProductID, @ProductName, @Category, @Supplier, @UnitPrice, @Quantity)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Supplier", supplier);
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item saved successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            ClearTextBoxes(); 
            AutoIncrementProductId();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            string productId = txtProductID.Text;

            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Please enter Product ID to load data.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtProductName.Text = reader["ProductName"].ToString();
                                cmbCategory.Text = reader["Category"].ToString();
                                txtSupplier.Text = reader["Supplier"].ToString();
                                txtUnitPrice.Text = reader["UnitPrice"].ToString();
                                txtQuantity.Text = reader["Quantity"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Product not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string productId = txtProductID.Text;
            string productName = txtProductName.Text;
            string category = cmbCategory.Text;
            string supplier = txtSupplier.Text;
            string unitPrice = txtUnitPrice.Text;
            string quantity = txtQuantity.Text;

            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Please enter Product ID to update.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Products SET ProductName = @ProductName, Category = @Category, Supplier = @Supplier, " +
                                   "UnitPrice = @UnitPrice, Quantity = @Quantity WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Supplier", supplier);
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Item updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Product ID not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            ClearTextBoxes();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string productId = txtProductID.Text;

            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Please enter Product ID to delete.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Item deleted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Product ID not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            ClearTextBoxes();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
