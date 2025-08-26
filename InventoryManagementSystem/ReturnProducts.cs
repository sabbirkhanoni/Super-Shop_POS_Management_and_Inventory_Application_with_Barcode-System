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

namespace InventoryManagementSystem
{
    public partial class ReturnProducts : Form
    {
        DataAccess db;
        int selectedProductId = -1;
        public ReturnProducts()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateGridViewSaleInventory();
            PopulateCustomerComboBox();

            ExtraDesign();
        }


        public void ExtraDesign()
        {
            DGVReturnDamageProducts.RowTemplate.Height = 35; // Example height in pixels

            DGVReturnDamageProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVReturnDamageProducts.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVReturnDamageProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVReturnDamageProducts.DefaultCellStyle.ForeColor = Color.White;
        }


        private void PopulateCustomerComboBox()
        {
            try
            {
                string query = "SELECT CustomerId, CustomerName FROM CustomerTable";
                var ds = db.ExecuteQuery(query);
                cmbCustomerName.DataSource = ds.Tables[0];
                cmbCustomerName.DisplayMember = "CustomerName";
                cmbCustomerName.ValueMember = "CustomerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Customers : " + ex.Message);
            }
        }


        private void PopulateGridViewSaleInventory(string query = @"
                -- Inventory Display Query - Current Stock Status with Returned and Damage Quantities
                    WITH ProductStock AS (
                        -- Calculate total purchased per product
                        SELECT 
                            ProductId,
                            SUM(Quantity) as TotalPurchased
                        FROM PurchaseDetailTable
                        GROUP BY ProductId
                    ),
                    ProductSold AS (
                        -- Calculate total sold per product
                        SELECT 
                            ProductId,
                            SUM(SaleQuantity) as TotalSold
                        FROM SaleDetailTable
                        GROUP BY ProductId
                    ),
                    ProductDamaged AS (
                        -- Calculate total damaged per product
                        SELECT 
                            ProductId,
                            SUM(DamageQuantity) as TotalDamaged
                        FROM DamageDetailTable
                        GROUP BY ProductId
                    ),
                    ProductReturned AS (
                        -- Calculate total returned per product
                        SELECT 
                            ProductId,
                            SUM(ReturnQuantity) as TotalReturned
                        FROM ReturnDetailTable
                        GROUP BY ProductId
                    )
                    SELECT 
                        p.ProductId,
                        p.ProductName,
                        c.CategoryName,
                        b.BrandName,
                        p.PurchaseCost,
                        p.SalePrice,
                        -- Total Purchased Quantity
                        ISNULL(ps.TotalPurchased, 0) AS TotalPurchased,
                        -- Total Sold Quantity
                        ISNULL(psold.TotalSold, 0) AS TotalSold,
                        -- Returned Quantity (Total Returned Quantity Of That Product)
                        ISNULL(pr.TotalReturned, 0) AS ReturnedQuantity,
                        -- Damage Quantity (Total Damage Quantity Of That Product)
                        ISNULL(pd.TotalDamaged, 0) AS DamageQuantity,
                        -- Current Available Quantity
                        (ISNULL(ps.TotalPurchased, 0) 
                            - ISNULL(psold.TotalSold, 0) 
                            - ISNULL(pd.TotalDamaged, 0) 
                            + ISNULL(pr.TotalReturned, 0)) AS CurrentAvailableQuantity
                    FROM 
                        ProductTable p
                        INNER JOIN CategoryTable c ON p.CategoryId = c.CategoryId
                        INNER JOIN BrandTable b ON p.BrandId = b.BrandId
                        LEFT JOIN ProductStock ps ON p.ProductId = ps.ProductId
                        LEFT JOIN ProductSold psold ON p.ProductId = psold.ProductId
                        LEFT JOIN ProductDamaged pd ON p.ProductId = pd.ProductId
                        LEFT JOIN ProductReturned pr ON p.ProductId = pr.ProductId
                    -- Optional filters (uncomment as needed):
                    WHERE (ISNULL(ps.TotalPurchased, 0) - ISNULL(psold.TotalSold, 0) - ISNULL(pd.TotalDamaged, 0) + ISNULL(pr.TotalReturned, 0)) >= 0  -- Only products with stock
                    -- WHERE (ISNULL(ps.TotalPurchased, 0) - ISNULL(psold.TotalSold, 0) - ISNULL(pd.TotalDamaged, 0) + ISNULL(pr.TotalReturned, 0)) <= 5  -- Low stock alert
                    ORDER BY 
                        p.ProductId DESC;

           ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVReturnDamageProducts.AutoGenerateColumns = true;
                    this.DGVReturnDamageProducts.DataSource = ds.Tables[0];
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

        private void btnReturnProduct_Click(object sender, EventArgs e)
        {
            if (selectedProductId == -1)
            {
                MessageBox.Show("Please Select The Product Which You Want To Return Or Damage", "Select Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please Enter a Valid Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbCustomerName.SelectedValue == null || !int.TryParse(cmbCustomerName.SelectedValue.ToString(), out int customerId))
            {
                MessageBox.Show("Please Select a Valid Customer.", "Invalid Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try { 
                // Insert a record into the ReturnDetailTable
                string insertQuery = "INSERT INTO ReturnDetailTable (CustomerId, ProductId, ReturnQuantity) VALUES (@CustomerId, @ProductId, @ReturnQuantity)";
            var insertParams = new[]
            {
            new SqlParameter("@CustomerId", customerId),
            new SqlParameter("@ProductId", selectedProductId),
            new SqlParameter("@ReturnQuantity", quantity)

                };

                int rowsInserted = db.ExecuteDMLQuery(insertQuery, insertParams);

                if (rowsInserted > 0)
                {
                    MessageBox.Show("Product successfully returned.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateGridViewSaleInventory(); // Refresh the grid view
                    clearFeilds(); // Clear the input fields
                }
                else
                {
                    MessageBox.Show("Failed To Log The Return. Please Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (SqlException ex)
            {
                MessageBox.Show("A database error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDamageProduct_Click(object sender, EventArgs e)
        {
            if (selectedProductId == -1)
            {
                MessageBox.Show("Please Select The Product Which You Want To Return Or Damage", "Select Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO DamageDetailTable (ProductId, DamageQuantity) VALUES (@ProductId, @DamageQuantity)";
                var insertParams = new[]
                {
                new SqlParameter("@ProductId", selectedProductId),
                new SqlParameter("@DamageQuantity", quantity)
                        };

                var insertRowAffected = db.ExecuteDMLQuery(insertQuery, insertParams);

                if (insertRowAffected > 0)
                {
                    MessageBox.Show("Oops! Damage Occurred", "Damage Effect Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateGridViewSaleInventory();
                    clearFeilds();
                }
                else
                {
                    MessageBox.Show("Error Occurred In Damage Insertion", "Insertion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("A database error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearFeilds()
        {
            txtQuantity.Clear();
            txtSearchBar.Clear();
            cmbCustomerName.SelectedIndex = -1;
            selectedProductId = -1;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }

        private void DGVReturnDamageProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int productId = Convert.ToInt32(DGVReturnDamageProducts.Rows[e.RowIndex].Cells[0].Value);
            selectedProductId = productId;
        }

        private void DGVReturnDamageProducts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVReturnDamageProducts.ClearSelection();
            DGVReturnDamageProducts.CurrentCell = null;
        }
    }
}
