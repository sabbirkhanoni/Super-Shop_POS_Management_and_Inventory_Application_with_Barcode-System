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
    public partial class SupplierDueDetails : Form
    {
        public SupplierDueDetails()
        {
            InitializeComponent();
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVSupplierDueDetails.RowTemplate.Height = 35; // Example height in pixels

            DGVSupplierDueDetails.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVSupplierDueDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVSupplierDueDetails.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVSupplierDueDetails.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminMainDashBoard adminMainDashBoard = new AdminMainDashBoard();
            adminMainDashBoard.Show();
            this.Hide();
        }
    }
}
