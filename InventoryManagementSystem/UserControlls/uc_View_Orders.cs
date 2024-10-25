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
    public partial class uc_View_Orders : UserControl
    {
        public uc_View_Orders()
        {
            InitializeComponent();
        }

        String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void uc_View_Orders_Load(object sender, EventArgs e)
        {
            LoadOrdersData();
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
                // Reload all data if the search box is cleared
                LoadOrdersData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrdersData();
        }
    }
}
