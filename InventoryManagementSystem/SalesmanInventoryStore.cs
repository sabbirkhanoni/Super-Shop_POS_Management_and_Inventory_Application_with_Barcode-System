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
    public partial class SalesmanInventoryStore : Form
    {
        private DataAccess db;
        public SalesmanInventoryStore()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateGridViewSaleInventory();

            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVInventory.RowTemplate.Height = 32; // Example height in pixels

            DGVInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVInventory.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVInventory.DefaultCellStyle.ForeColor = Color.White;

        }


        private void PopulateGridViewSaleInventory(string filter = "")
        {
            try
            {
                string query = @"
        SELECT 
            p.ProductId,
            p.ProductName,
            c.CategoryName,
            b.BrandName,
            p.SalePrice,
            p.PurchaseCost,
            COALESCE(SUM(pd.Quantity), 0) AS Quantity
        FROM 
            ProductTable p
        LEFT JOIN 
            CategoryTable c ON p.CategoryId = c.CategoryId
        LEFT JOIN 
            BrandTable b ON p.BrandId = b.BrandId
        LEFT JOIN 
            PurchaseDetailTable pd ON p.ProductId = pd.ProductId
        GROUP BY 
            p.ProductId,
            p.ProductName,
            c.CategoryName,
            b.BrandName,
            p.SalePrice,
            p.PurchaseCost
        HAVING 
            COALESCE(SUM(pd.Quantity), 0) > 0";

                // Apply filter if provided
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += $" AND p.ProductName LIKE '%{filter}%'";
                }

                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVInventory.AutoGenerateColumns = true;
                    this.DGVInventory.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Database context is not initialized.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the DataGridView: " + ex.Message);
            }
        }





        private void btnBack_Click(object sender, EventArgs e)
        {
                SalesmanMainDashBoard salesmanMainDashBoard = new SalesmanMainDashBoard();
                salesmanMainDashBoard.Show();
                this.Hide();
        }

        private void DGVInventory_SelectionChanged(object sender, EventArgs e)
        {
            DGVInventory.ClearSelection();
            DGVInventory.CurrentCell = null;
        }


        private void BtnSalesMainDashBoard_Click(object sender, EventArgs e)
        {
            SalesmanMainDashBoard salesmanMainDashBoad = new SalesmanMainDashBoard();
            salesmanMainDashBoad.Show();
            this.Hide();
        }

        private void btnAddProducts_Click(object sender, EventArgs e)
        {
            AddProducts addProducts = new AddProducts();
            addProducts.Show();
            this.Hide();
        }

        private void btnStockProducts_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore inventory = new SalesmanInventoryStore();
            inventory.Show();
            this.Hide();
        }
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.Show();
            this.Hide();
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            AddSupplier addSupplier = new AddSupplier();
            addSupplier.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnProductSettings_Click(object sender, EventArgs e)
        {
            ProductSettings productSettings = new ProductSettings();
            productSettings.Show();
            this.Hide();
        }

        private void btnReturnAndDamage_Click(object sender, EventArgs e)
        {
            ReturnProducts returnProducts = new ReturnProducts();
            returnProducts.Show();
            this.Hide();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewSaleInventory(txtSearchBar.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchBar.Clear();
        }

        private void btnOthers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Available For You", "Feature Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

    }
}
