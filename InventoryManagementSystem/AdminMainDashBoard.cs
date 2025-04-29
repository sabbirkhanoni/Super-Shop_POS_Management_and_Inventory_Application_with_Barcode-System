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
    public partial class AdminMainDashBoard : Form
    {
        public static string SessionNameForAdminGoToInventory { get; set; }
        public AdminMainDashBoard()
        {
            InitializeComponent();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore inventory = new SalesmanInventoryStore();
            inventory.Show();
            this.Hide();
        }

        private void btnCustomerDue_Click(object sender, EventArgs e)
        {
            CustomerDueDetails details = new CustomerDueDetails();
            details.Show();
            this.Hide();
        }

        private void btnSupplierDue_Click(object sender, EventArgs e)
        {
            SupplierDueDetails details = new SupplierDueDetails();
            details.Show();
            this.Hide();
        }

        private void btnDamageProducts_Click(object sender, EventArgs e)
        {
            AdminDamageReturnDetails details = new AdminDamageReturnDetails();
            details.Show();
            this.Hide();
        }

        private void btnAdminSettings_Click(object sender, EventArgs e)
        {
            AdminPersonalSettings details = new AdminPersonalSettings();
            details.Show();
            this.Hide();
        }

        private void btnCreateNewUser_Click(object sender, EventArgs e)
        {
            AdminCreatesNewUser adminCreatesNewUser = new AdminCreatesNewUser();
            adminCreatesNewUser.Show();
            this.Hide();
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm loginFormInstance = new LoginForm();
            loginFormInstance.Show();
            this.Hide();
        }

        private void btnContinueAsSalesman_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore store = new SalesmanInventoryStore();
            store.Show();
            this.Hide();
        }

        private void btnSaleDetails_Click(object sender, EventArgs e)
        {
            AdminSaleDetails details = new AdminSaleDetails();
            details.Show();
            this.Hide();
        }

        private void btnCustomerPurchaseDetails_Click(object sender, EventArgs e)
        {
            AdminCustomersPurchaseDetails details = new AdminCustomersPurchaseDetails();
            details.Show();
            this.Hide();
        }
    }
}
