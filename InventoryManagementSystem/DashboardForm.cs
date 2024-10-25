using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            uc_Manage1.Hide();
            uc_View_Items1.Hide();
            uc_Order1.Hide();
            uc_View_Orders1.Hide();
            uc_Admin1.Hide();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            uc_Manage1.Show();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            uc_View_Items1.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            uc_Order1.Show();
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            uc_View_Orders1.Show();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            uc_Admin1.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
