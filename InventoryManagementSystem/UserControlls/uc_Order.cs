using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem.UserControlls
{
    public partial class uc_Order : UserControl
    {
        private PrintDocument printDocument = new PrintDocument();
        private Order currentOrder;
        public uc_Order()
        {
            InitializeComponent();
            AutoIncrementOrderId();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

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
        private void LoadOrdersData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Orders", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvdata.DataSource = dt;
            }
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

        private void txtSearch_TextChange(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Orders WHERE ProductID LIKE @search OR ProductName LIKE @search";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + searchValue + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvdata.DataSource = dt;
                }
            }
            else
            {
                LoadOrdersData();
            }
        }


        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadOrdersData();
        }

        private void uc_Order_Load(object sender, EventArgs e)
        {
            LoadOrdersData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (int.TryParse(txtOrderId.Text, out int orderId))
            {
                currentOrder = GetOrderDetails(orderId);

                if (currentOrder != null)
                {

                    PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                    previewDialog.Document = printDocument;

                    Form previewForm = (Form)previewDialog;
                    previewForm.WindowState = FormWindowState.Maximized;

                    previewDialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Order not found. Please enter a valid Order ID.", "Order Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Order ID number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Order GetOrderDetails(int orderId)
        {
            String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";
            string query = "SELECT * FROM Orders WHERE OrderID = @OrderId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Order
                    {
                        OrderID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        ProductName = reader.GetString(2),
                        UnitPrice = reader.GetDecimal(3),
                        Quantity = reader.GetInt32(4),
                        Total = reader.GetDecimal(5)
                    };
                }
            }
            return null;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (currentOrder != null)
            {
                Font headerFont = new Font("Arial", 16, FontStyle.Bold);
                Font labelFont = new Font("Arial", 12, FontStyle.Bold);
                Font contentFont = new Font("Arial", 12, FontStyle.Regular);
                Font footerFont = new Font("Arial", 10, FontStyle.Italic);
                int startX = 50, startY = 60, offset = 20;
                int lineHeight = 30;


                e.Graphics.DrawString("Order Receipt", headerFont, Brushes.DarkBlue, startX + 100, startY);
                offset += lineHeight + 10;
                e.Graphics.DrawLine(Pens.Gray, startX, startY + offset, startX + 400, startY + offset);  
                offset += lineHeight;

               
                e.Graphics.DrawString("Order ID:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString(currentOrder.OrderID.ToString(), contentFont, Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

                e.Graphics.DrawString("Product ID:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString(currentOrder.ProductID.ToString(), contentFont, Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

                e.Graphics.DrawString("Product Name:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString(currentOrder.ProductName, contentFont, Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

                e.Graphics.DrawString("Unit Price:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString($"LKR{currentOrder.UnitPrice:F2}", contentFont, Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

                e.Graphics.DrawString("Quantity:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString(currentOrder.Quantity.ToString(), contentFont, Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

                e.Graphics.DrawString("Total:", labelFont, Brushes.Black, startX, startY + offset);
                e.Graphics.DrawString($"LKR{currentOrder.Total:F2}", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, startX + 120, startY + offset);
                offset += lineHeight;

          
                offset += 20; 
                e.Graphics.DrawLine(Pens.Gray, startX, startY + offset, startX + 400, startY + offset); 
                offset += lineHeight;
                e.Graphics.DrawString("Thank you for your purchase!", footerFont, Brushes.Gray, startX + 80, startY + offset);
            }
        }

    }

    public class Order
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
