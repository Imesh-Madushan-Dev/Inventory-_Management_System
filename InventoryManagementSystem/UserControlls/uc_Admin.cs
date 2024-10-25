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
    public partial class uc_Admin : UserControl
    {
        public uc_Admin()
        {
            InitializeComponent();
        }

        String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Users", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvdata.DataSource = dt;
            }
        }

        private void uc_Admin_Load(object sender, EventArgs e)
        {

            LoadData();
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Order ID is required.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadData();
                            MessageBox.Show("Order deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No user found with the specified username.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while accessing the database: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

    }
}
