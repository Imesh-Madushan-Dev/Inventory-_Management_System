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

namespace InventoryManagementSystem.UserControlls
{
    public partial class uc_Order : UserControl
    {
        public uc_Order()
        {
            InitializeComponent();
            AutoIncrementOrderId();
        }

        String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";

        private void AutoIncrementOrderId()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(OrderID), 0) + 1 FROM Orders", conn);
                int newOrderId = (int)cmd.ExecuteScalar();
                txtOrderId.Text = newOrderId.ToString();
            }
        }

        private void ClearFields()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            lblTotal.Text = "LKR 0.00";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtProductId_TextChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ProductName FROM Products WHERE ProductID = @ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", txtProductId.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtProductName.Text = reader["ProductName"].ToString();
                    }
                    else
                    {
                        txtProductName.Clear();
                    }
                }
            }
            else
            {
                txtProductName.Clear();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text) || string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Order ID and Product ID are required.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Orders (OrderID, ProductID, ProductName, UnitPrice, Quantity) VALUES (@OrderID, @ProductID, @ProductName, @UnitPrice, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", txtOrderId.Text);
                cmd.Parameters.AddWithValue("@ProductID", txtProductId.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                cmd.ExecuteNonQuery();
                ClearFields();
                AutoIncrementOrderId();
                MessageBox.Show("Order saved successfully.");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Order ID is required.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Orders WHERE OrderID = @OrderID", conn);
                cmd.Parameters.AddWithValue("@OrderID", txtOrderId.Text);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtProductId.Text = reader["ProductID"].ToString();
                    txtProductName.Text = reader["ProductName"].ToString();
                    txtUnitPrice.Text = reader["UnitPrice"].ToString();
                    txtQuantity.Text = reader["Quantity"].ToString();
                }
                else
                {
                    MessageBox.Show("Order not found.");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Order ID is required.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Orders SET ProductID = @ProductID, ProductName = @ProductName, UnitPrice = @UnitPrice, Quantity = @Quantity WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", txtOrderId.Text);
                cmd.Parameters.AddWithValue("@ProductID", txtProductId.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                cmd.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                cmd.ExecuteNonQuery();
                ClearFields();
                AutoIncrementOrderId();
                MessageBox.Show("Order updated successfully.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Order ID is required.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", txtOrderId.Text);
                cmd.ExecuteNonQuery();
                ClearFields();
                AutoIncrementOrderId();
                MessageBox.Show("Order deleted successfully.");
            }
        }

        private void txtQuantity_TextChange(object sender, EventArgs e)
        {
            double unitPrice, quantity;
            if (double.TryParse(txtUnitPrice.Text, out unitPrice) && double.TryParse(txtQuantity.Text, out quantity))
            {
                double total = unitPrice * quantity;
                lblTotal.Text = "LKR " + total.ToString("0.00");
            }
            else
            {
                lblTotal.Text = "LKR 0.00";
            }
        }
    }
}
