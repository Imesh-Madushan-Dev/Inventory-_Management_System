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
    public partial class uc_View_Items : UserControl
    {
        public uc_View_Items()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        String connectionString = "Data Source=DESKTOP-Q4PDC5P\\SQLEXPRESS;Initial Catalog=InventorySystem;Integrated Security=True";

        private void txtSearch_TextChange(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string searchQuery = "SELECT ProductID, ProductName, Category, Supplier, UnitPrice, Quantity FROM Products " +
                                     "WHERE ProductID LIKE @Search OR ProductName LIKE @Search OR Category LIKE @Search";
                SqlCommand cmd = new SqlCommand(searchQuery, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + txtSearch.Text + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvdata.DataSource = dt;
            }
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProductID, ProductName, Category, Supplier, UnitPrice, Quantity FROM Products";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvdata.DataSource = dt;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void uc_View_Items_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
