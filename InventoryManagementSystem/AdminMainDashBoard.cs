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
        private DataAccess db;
        public AdminMainDashBoard()
        {
            InitializeComponent();
            db = new DataAccess();
            this.DoubleBuffered = true;
        }

        private void btnCustomerDue_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(CustomerDueDetails));
        }

        private void btnSupplierDue_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(SupplierDueDetails));
        }

        private void btnDamageProducts_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminDamageReturnDetails));
        }

        private void btnCreateNewUser_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminCreatesNewUser));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSaleDetails_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminSaleDetails));
        }

        private void btnCustomerPurchaseDetails_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminCustomersPurchaseDetails));
        }

        private void BtnSalesMainDashBoard_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(SalesmanMainDashBoard));
        }

        private void btnInventorySec_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(SalesmanInventoryStore));
        }

        private void btnProductSettings_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(ProductSettings));
        }

        private void btnAddProducts_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AddProducts));
        }

        private void btnReturnAndDamage_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(ReturnProducts));
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AddCustomer));
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AddSupplier));
        }

        private void btnSalesMainDashBoardOpen_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(SalesmanMainDashBoard));
        }


        private void LoadDashboardData()
        {
            string query = @"
                        WITH DashboardMetrics AS (
                            SELECT 'Today' as Period,
                            -- Today Purchase Metrics
                            ISNULL((SELECT SUM(pd.Quantity * pd.UnitPrice)
                                FROM PurchaseDetailTable pd
                                JOIN PurchaseTable p ON pd.PurchaseId = p.PurchaseId
                                WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as PurchaseCost,
                            ISNULL((SELECT SUM(p.PaidAmount)
                                FROM PurchaseTable p
                                WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as PaidPurchaseCost,
                            ISNULL((SELECT SUM(sd.DueAmount)
                                FROM SupplierDueTable sd
                                JOIN PurchaseTable p ON sd.PurchaseId = p.PurchaseId
                                WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as PurchaseCostDue,
                            -- Today Sale Metrics
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE CAST(s.SaleDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as SoldPrice,
                            -- Today Real Sold (Actual Paid Amount)
                            ISNULL((SELECT SUM(CAST((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE CAST(s.SaleDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as RealSold,
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice) - SUM(sd.SaleQuantity * pt.PurchaseCost)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE CAST(s.SaleDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as Profit,
                            -- Today Current Profit (Based on Paid Amount)
                            ISNULL((SELECT SUM(CAST(ROUND(
                                    ((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0))
                                    - ((sd.SaleQuantity * pt.PurchaseCost * s.PaidAmount) / NULLIF(s.PayAmount, 0)), 2) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE CAST(s.SaleDate AS DATE) = CAST(GETDATE() AS DATE)), 0) as CurrentProfit
                            UNION ALL
                            SELECT 'Weekly' as Period,
                            -- Weekly Purchase Metrics
                            ISNULL((SELECT SUM(pd.Quantity * pd.UnitPrice)
                                FROM PurchaseDetailTable pd
                                JOIN PurchaseTable p ON pd.PurchaseId = p.PurchaseId
                                WHERE DATEPART(WEEK, p.PurchaseDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PurchaseCost,
                            ISNULL((SELECT SUM(p.PaidAmount)
                                FROM PurchaseTable p
                                WHERE DATEPART(WEEK, p.PurchaseDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PaidPurchaseCost,
                            ISNULL((SELECT SUM(sd.DueAmount)
                                FROM SupplierDueTable sd
                                JOIN PurchaseTable p ON sd.PurchaseId = p.PurchaseId
                                WHERE DATEPART(WEEK, p.PurchaseDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PurchaseCostDue,
                            -- Weekly Sale Metrics
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE DATEPART(WEEK, s.SaleDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as SoldPrice,
                            -- Weekly Real Sold (Actual Paid Amount)
                            ISNULL((SELECT SUM(CAST((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE DATEPART(WEEK, s.SaleDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as RealSold,
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice) - SUM(sd.SaleQuantity * pt.PurchaseCost)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE DATEPART(WEEK, s.SaleDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as Profit,
                            -- Weekly Current Profit (Based on Paid Amount)
                            ISNULL((SELECT SUM(CAST(ROUND(
                                    ((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0))
                                    - ((sd.SaleQuantity * pt.PurchaseCost * s.PaidAmount) / NULLIF(s.PayAmount, 0)), 2) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE DATEPART(WEEK, s.SaleDate) = DATEPART(WEEK, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as CurrentProfit
                            UNION ALL
                            SELECT 'Monthly' as Period,
                            -- Monthly Purchase Metrics
                            ISNULL((SELECT SUM(pd.Quantity * pd.UnitPrice)
                                FROM PurchaseDetailTable pd
                                JOIN PurchaseTable p ON pd.PurchaseId = p.PurchaseId
                                WHERE DATEPART(MONTH, p.PurchaseDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PurchaseCost,
                            ISNULL((SELECT SUM(p.PaidAmount)
                                FROM PurchaseTable p
                                WHERE DATEPART(MONTH, p.PurchaseDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PaidPurchaseCost,
                            ISNULL((SELECT SUM(sd.DueAmount)
                                FROM SupplierDueTable sd
                                JOIN PurchaseTable p ON sd.PurchaseId = p.PurchaseId
                                WHERE DATEPART(MONTH, p.PurchaseDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, p.PurchaseDate) = DATEPART(YEAR, GETDATE())), 0) as PurchaseCostDue,
                            -- Monthly Sale Metrics
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE DATEPART(MONTH, s.SaleDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as SoldPrice,
                            -- Monthly Real Sold (Actual Paid Amount)
                            ISNULL((SELECT SUM(CAST((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                WHERE DATEPART(MONTH, s.SaleDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as RealSold,
                            ISNULL((SELECT SUM(sd.SaleQuantity * sd.UnitPrice) - SUM(sd.SaleQuantity * pt.PurchaseCost)
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE DATEPART(MONTH, s.SaleDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as Profit,
                            -- Monthly Current Profit (Based on Paid Amount)
                            ISNULL((SELECT SUM(CAST(ROUND(
                                    ((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(s.PayAmount, 0))
                                    - ((sd.SaleQuantity * pt.PurchaseCost * s.PaidAmount) / NULLIF(s.PayAmount, 0)), 2) AS DECIMAL(18,2)))
                                FROM SaleDetailTable sd
                                JOIN SaleTable s ON sd.SaleId = s.SaleId
                                JOIN ProductTable pt ON sd.ProductId = pt.ProductId
                                WHERE DATEPART(MONTH, s.SaleDate) = DATEPART(MONTH, GETDATE())
                                AND DATEPART(YEAR, s.SaleDate) = DATEPART(YEAR, GETDATE())), 0) as CurrentProfit
                        ),
                        CountMetrics AS (
                            SELECT
                            (SELECT COUNT(*) FROM ProductTable) as TotalProducts,
                            (SELECT COUNT(*) FROM CategoryTable) as TotalCategories,
                            (SELECT COUNT(*) FROM BrandTable) as TotalBrands,
                            (SELECT COUNT(*) FROM CustomerTable) as TotalCustomers,
                            (SELECT COUNT(*) FROM SupplierTable) as TotalSuppliers,
                            (SELECT ISNULL(SUM(DueAmount),0) FROM CustomerDueTable WHERE DueAmount > 0) as CustomerDues,
                            (SELECT ISNULL(SUM(DueAmount),0) FROM SupplierDueTable WHERE DueAmount > 0) as SupplierDues,
                            (SELECT COUNT(DISTINCT ProductId) FROM DamageDetailTable) as DamageProducts,
                            (SELECT COUNT(DISTINCT ProductId) FROM ReturnDetailTable) as ReturnProducts
                        ),
                        NetMetrics AS (
                            SELECT
                            -- Net Purchase Cost (Current Available Quantity's Purchase Cost)
                            ISNULL(SUM(stock_info.CurrentStock * p.PurchaseCost), 0) as NetPurchaseCost,
                            -- Net Sale Price (Current Available Quantity's Sale Price)
                            ISNULL(SUM(stock_info.CurrentStock * p.SalePrice), 0) as NetSaleCost,
                            -- Net Profit (Current Available Quantity's Would be Profit)
                            ISNULL(SUM(stock_info.CurrentStock * (p.SalePrice - p.PurchaseCost)), 0) as NetProfit
                            FROM ProductTable p
                            LEFT JOIN (
                                SELECT
                                p.ProductId,
                                (ISNULL(purch.TotalPurchased, 0) - ISNULL(sales.TotalSold, 0) - 
                                 ISNULL(damages.TotalDamaged, 0) + ISNULL(returns.TotalReturned, 0)) AS CurrentStock
                                FROM ProductTable p
                                LEFT JOIN (SELECT ProductId, SUM(Quantity) AS TotalPurchased FROM PurchaseDetailTable GROUP BY ProductId) purch ON p.ProductId = purch.ProductId
                                LEFT JOIN (SELECT ProductId, SUM(SaleQuantity) AS TotalSold FROM SaleDetailTable GROUP BY ProductId) sales ON p.ProductId = sales.ProductId
                                LEFT JOIN (SELECT ProductId, SUM(ReturnQuantity) AS TotalReturned FROM ReturnDetailTable GROUP BY ProductId) returns ON p.ProductId = returns.ProductId
                                LEFT JOIN (SELECT ProductId, SUM(DamageQuantity) AS TotalDamaged FROM DamageDetailTable GROUP BY ProductId) damages ON p.ProductId = damages.ProductId
                            ) stock_info ON p.ProductId = stock_info.ProductId
                            WHERE stock_info.CurrentStock > 0
                        )
                        SELECT
                        -- Today Metrics
                        (SELECT PurchaseCost FROM DashboardMetrics WHERE Period = 'Today') as TodayPurchaseCost,
                        (SELECT PaidPurchaseCost FROM DashboardMetrics WHERE Period = 'Today') as TodayPaidPurchaseCost,
                        (SELECT PurchaseCostDue FROM DashboardMetrics WHERE Period = 'Today') as TodayPurchaseCostDue,
                        (SELECT SoldPrice FROM DashboardMetrics WHERE Period = 'Today') as TodaySold,
                        (SELECT RealSold FROM DashboardMetrics WHERE Period = 'Today') as TodayRealSold,
                        (SELECT Profit FROM DashboardMetrics WHERE Period = 'Today') as TodayProfit,
                        (SELECT CurrentProfit FROM DashboardMetrics WHERE Period = 'Today') as TodayCurrentProfit,
                        -- Weekly Metrics
                        (SELECT PurchaseCost FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyPurchaseCost,
                        (SELECT PaidPurchaseCost FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyPaidPurchaseCost,
                        (SELECT PurchaseCostDue FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyPurchaseCostDue,
                        (SELECT SoldPrice FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklySold,
                        (SELECT RealSold FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyRealSold,
                        (SELECT Profit FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyProfit,
                        (SELECT CurrentProfit FROM DashboardMetrics WHERE Period = 'Weekly') as WeeklyCurrentProfit,
                        -- Monthly Metrics
                        (SELECT PurchaseCost FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyPurchaseCost,
                        (SELECT PaidPurchaseCost FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyPaidPurchaseCost,
                        (SELECT PurchaseCostDue FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyPurchaseCostDue,
                        (SELECT SoldPrice FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlySold,
                        (SELECT RealSold FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyRealSold,
                        (SELECT Profit FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyProfit,
                        (SELECT CurrentProfit FROM DashboardMetrics WHERE Period = 'Monthly') as MonthlyCurrentProfit,
                        -- Net Metrics
                        nm.NetPurchaseCost,
                        nm.NetSaleCost,
                        nm.NetProfit,
                        -- Count Metrics
                        cm.TotalProducts,
                        cm.TotalCategories,
                        cm.TotalBrands,
                        cm.TotalCustomers,
                        cm.TotalSuppliers,
                        cm.CustomerDues,
                        cm.SupplierDues,
                        cm.DamageProducts,
                        cm.ReturnProducts
                        FROM CountMetrics cm, NetMetrics nm;
                        ";

            DataTable dt = db.ExecuteQueryTable(query);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                // Today Metrics
                lblTodayPurchaseCost.Text = row["TodayPurchaseCost"].ToString();
                lblTodayPaidPurchaseCost.Text = row["TodayPaidPurchaseCost"].ToString();
                lblTodayPurchaseCostDue.Text = row["TodayPurchaseCostDue"].ToString();
                lblTodaySold.Text = row["TodaySold"].ToString();
                lblTodayRealSold.Text = row["TodayRealSold"].ToString();
                lblTodayProfit.Text = row["TodayProfit"].ToString();
                lblTodayCurrentProfit.Text = row["TodayCurrentProfit"].ToString();

                // Weekly Metrics
                lblWeeklyPurchaseCost.Text = row["WeeklyPurchaseCost"].ToString();
                lblWeeklyPaidPurchaseCost.Text = row["WeeklyPaidPurchaseCost"].ToString();
                lblWeeklyPurchaseCostDue.Text = row["WeeklyPurchaseCostDue"].ToString();
                lblWeeklySold.Text = row["WeeklySold"].ToString();
                lblWeeklyRealSold.Text = row["WeeklyRealSold"].ToString();
                lblWeeklyProfit.Text = row["WeeklyProfit"].ToString();
                lblWeeklyCurrentProfit.Text = row["WeeklyCurrentProfit"].ToString();

                // Monthly Metrics
                lblMonthlyPurchaseCost.Text = row["MonthlyPurchaseCost"].ToString();
                lblMonthlyPaidPurchaseCost.Text = row["MonthlyPaidPurchaseCost"].ToString();
                lblMonthlyPurchaseCostDue.Text = row["MonthlyPurchaseCostDue"].ToString();
                lblMonthlySold.Text = row["MonthlySold"].ToString();
                lblMonthlyRealSold.Text = row["MonthlyRealSold"].ToString();
                lblMonthlyProfit.Text = row["MonthlyProfit"].ToString();
                lblMonthlyCurrentProfit.Text = row["MonthlyCurrentProfit"].ToString();

                // Net Metrics (New Fields)
                lblNetPurchaseCost.Text = row["NetPurchaseCost"].ToString();
                lblNetSaleCost.Text = row["NetSaleCost"].ToString();
                lblNetProfit.Text = row["NetProfit"].ToString();

                // Count Metrics (Original Fields)
                lblTotalProducts.Text = row["TotalProducts"].ToString();
                lblTotalCategory.Text = row["TotalCategories"].ToString();
                lblTotalBrand.Text = row["TotalBrands"].ToString();
                lblTotalCustomer.Text = row["TotalCustomers"].ToString();
                lblTotalSuppliers.Text = row["TotalSuppliers"].ToString();
                lblCustomerDues.Text = row["CustomerDues"].ToString();
                lblSuppliersDues.Text = row["SupplierDues"].ToString();
                lblDamageProducts.Text = row["DamageProducts"].ToString();
                lblReturnProducts.Text = row["ReturnProducts"].ToString();
            }
        }

        private void AdminMainDashBoard_VisibleChanged(object sender, EventArgs e)
        {
             LoadDashboardData();
        }

        private void btnMoreSaleDetails_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMoreSaleDetails));
        }

        private void addProductPurchaseDetails_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AddProductPurchaseDetails));
        }

        private void btnAllSaleDetails_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminAllSaleDetails));
        }

        private void btnAddCustomer_Click_1(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AddCustomer));
        }

        private void btnCreateNewUser_Click_1(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminCreatesNewUser));
        }
    }
}
