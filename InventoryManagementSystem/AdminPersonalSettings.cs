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
    public partial class AdminPersonalSettings : Form
    {
        public AdminPersonalSettings()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            AdminMainDashBoard adminMainDashBoard = new AdminMainDashBoard();
            adminMainDashBoard.Show();
            this.Hide();
        }
    }
}
