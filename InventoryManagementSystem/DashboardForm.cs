using InventoryManagementSystem.UserControlls;
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
            uc_Manage1.Visible = false;
            uc_Order1.Visible = false;
            
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            uc_Manage1.Visible = true;
          
        }

       

        private void btnOrder_Click(object sender, EventArgs e)
        {

            uc_Order1.Visible = true ;
        }

        

        private void btnAdmin_Click(object sender, EventArgs e)
        {
           Admins Admin = new Admins();
            Admin.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        
    }
}
