using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class AdminDamageReturnDetails : Form
    {
        private DataAccess db;
        public AdminDamageReturnDetails()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateDamegeData();
            PopulateReturnData();
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVDamage.RowTemplate.Height = 35; // Example height in pixels

            DGVDamage.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVDamage.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVDamage.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVDamage.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row


            DGVReturn.RowTemplate.Height = 35; // Example height in pixels

            DGVReturn.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVReturn.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVReturn.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVReturn.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }
        


        private void PopulateDamegeData(string query = @"
                            WITH ProductStock AS (
                            SELECT ProductId, SUM(Quantity) AS TotalPurchased
                            FROM PurchaseDetailTable
                            GROUP BY ProductId
                        ),
                        ProductSold AS (
                            SELECT ProductId, SUM(SaleQuantity) AS TotalSold
                            FROM SaleDetailTable
                            GROUP BY ProductId
                        ),
                        ProductDamaged AS (
                            SELECT ProductId, SUM(DamageQuantity) AS TotalDamaged
                            FROM DamageDetailTable
                            GROUP BY ProductId
                        ),
                        ProductReturned AS (
                            SELECT ProductId, SUM(ReturnQuantity) AS TotalReturned
                            FROM ReturnDetailTable
                            GROUP BY ProductId
                        )

                        SELECT 
                            p.ProductId,
                            p.ProductName,
                            cat.CategoryName,
                            b.BrandName,
                            p.PurchaseCost,
                            p.SalePrice,
                            dd.DamageQuantity AS DamagedQuantity,
                            dd.DamageInfo,

                            -- Current Available Quantity
                            (ISNULL(ps.TotalPurchased, 0) 
                             - ISNULL(psold.TotalSold, 0) 
                             - ISNULL(pd.TotalDamaged, 0) 
                             + ISNULL(pr.TotalReturned, 0)) AS CurrentAvailableQuantity

                        FROM DamageDetailTable dd
                        INNER JOIN ProductTable p 
                            ON dd.ProductId = p.ProductId
                        INNER JOIN CategoryTable cat 
                            ON p.CategoryId = cat.CategoryId
                        INNER JOIN BrandTable b 
                            ON p.BrandId = b.BrandId
                        LEFT JOIN ProductStock ps 
                            ON p.ProductId = ps.ProductId
                        LEFT JOIN ProductSold psold 
                            ON p.ProductId = psold.ProductId
                        LEFT JOIN ProductDamaged pd 
                            ON p.ProductId = pd.ProductId
                        LEFT JOIN ProductReturned pr 
                            ON p.ProductId = pr.ProductId
                        ORDER BY dd.DamageDetailId DESC;


            ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVDamage.AutoGenerateColumns = true;
                    this.DGVDamage.DataSource = ds.Tables[0];
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



        private void PopulateReturnData(string query = @"
                            WITH ProductStock AS(
                            SELECT ProductId, SUM(Quantity) AS TotalPurchased
                            FROM PurchaseDetailTable
                            GROUP BY ProductId
                        ),
                        ProductSold AS(
                            SELECT ProductId, SUM(SaleQuantity) AS TotalSold
                            FROM SaleDetailTable
                            GROUP BY ProductId
                        ),
                        ProductDamaged AS(
                            SELECT ProductId, SUM(DamageQuantity) AS TotalDamaged
                            FROM DamageDetailTable
                            GROUP BY ProductId
                        ),
                        ProductReturned AS(
                            SELECT ProductId, SUM(ReturnQuantity) AS TotalReturned
                            FROM ReturnDetailTable
                            GROUP BY ProductId
                        )

                        SELECT
                            p.ProductId,
                            p.ProductName,
                            cat.CategoryName,
                            b.BrandName,
                            p.PurchaseCost,
                            p.SalePrice,
                            rd.ReturnQuantity AS ReturnedQuantity,
                            c.CustomerName,

                            -- Current Available Quantity
                            (ISNULL(ps.TotalPurchased, 0)
                             - ISNULL(psold.TotalSold, 0)
                             - ISNULL(pd.TotalDamaged, 0)
                             + ISNULL(pr.TotalReturned, 0)) AS CurrentAvailableQuantity

                        FROM ReturnDetailTable rd
                        INNER JOIN ProductTable p
                            ON rd.ProductId = p.ProductId
                        INNER JOIN CustomerTable c
                            ON rd.CustomerId = c.CustomerId
                        INNER JOIN CategoryTable cat
                            ON p.CategoryId = cat.CategoryId
                        INNER JOIN BrandTable b
                            ON p.BrandId = b.BrandId
                        LEFT JOIN ProductStock ps
                            ON p.ProductId = ps.ProductId
                        LEFT JOIN ProductSold psold
                            ON p.ProductId = psold.ProductId
                        LEFT JOIN ProductDamaged pd
                            ON p.ProductId = pd.ProductId
                        LEFT JOIN ProductReturned pr
                            ON p.ProductId = pr.ProductId
                        ORDER BY rd.ReturnDetailId DESC;


            ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVReturn.AutoGenerateColumns = true;
                    this.DGVReturn.DataSource = ds.Tables[0];
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
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }


        

    }
}
