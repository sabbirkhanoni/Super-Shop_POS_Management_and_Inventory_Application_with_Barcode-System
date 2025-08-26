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
        private DataAccess db;
        public SupplierDueDetails()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateMoreSaleDetailsData();
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

        private void PopulateMoreSaleDetailsData(string query = @"
                        SELECT 
                        s.SupplierId,
                        s.SupplierName,
                        p.ProductId,
                        p.ProductName,
                        cat.CategoryName,
                        b.BrandName,
                        p.PurchaseCost,
                        p.SalePrice,
                        pd.Quantity AS PurchasedQuantity,
                        pt.TotalAmount,
                        pt.PaidAmount,
                        ISNULL(pt.Discount, 0) AS Discount,

                        -- Specific purchase due amount
                        ISNULL(sd.DueAmount, 0) AS PurchaseDueAmount,

                        -- Total due for the supplier
                        ISNULL(total_due.TotalDueAmount, 0) AS TotalDueAmount,

                        pt.PurchaseDate
                    FROM PurchaseTable pt
                    INNER JOIN SupplierTable s 
                        ON pt.SupplierId = s.SupplierId
                    INNER JOIN PurchaseDetailTable pd 
                        ON pt.PurchaseId = pd.PurchaseId
                    INNER JOIN ProductTable p 
                        ON pd.ProductId = p.ProductId
                    INNER JOIN CategoryTable cat 
                        ON p.CategoryId = cat.CategoryId
                    INNER JOIN BrandTable b 
                        ON p.BrandId = b.BrandId

                    -- Purchase-specific due amount
                    LEFT JOIN SupplierDueTable sd 
                        ON pt.PurchaseId = sd.PurchaseId 
                        AND pt.SupplierId = sd.SupplierId

                    -- Total due per supplier
                    LEFT JOIN (
                        SELECT SupplierId, SUM(DueAmount) AS TotalDueAmount
                        FROM SupplierDueTable
                        GROUP BY SupplierId
                    ) total_due 
                        ON pt.SupplierId = total_due.SupplierId

                    WHERE ISNULL(sd.DueAmount, 0) > 0
                    ORDER BY pt.PurchaseDate DESC, s.SupplierName;


            ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVSupplierDueDetails.AutoGenerateColumns = true;
                    this.DGVSupplierDueDetails.DataSource = ds.Tables[0];
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
