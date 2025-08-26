using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using Rectangle = iTextSharp.text.Rectangle;


namespace InventoryManagementSystem
{
    public partial class SalesmanMainDashBoard : Form
    {
        public static string SessionNameForGoInventory { get; set; }

        private DataAccess db;

        private DataTable cartTable;
        private bool chkbDiscountClick = false;
        private bool walkInCustomer = false;


        //Printing
        private PrintDocument printDocument;
        private string invoiceToPrint;
        private int saleIdForInvoice;

        public SalesmanMainDashBoard()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateGridViewSaleInventory();
            PopulateCustomerComboBox();
            InitializeCartTable();
            ExtraDesign();

        }

        private void InitializePrinting()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void SalesmanMainDashBoard_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                PopulateGridViewSaleInventory();
                PopulateCustomerComboBox();
            }
        }



        public void ExtraDesign()
        {
            DGVSaleCart.RowTemplate.Height = 35; // Example height in pixels
            DGVSaleInventory.RowTemplate.Height = 35; // Example height in pixels

            DGVSaleCart.RowsDefaultCellStyle.BackColor = Color.AliceBlue; // Default row color
            DGVSaleCart.AlternatingRowsDefaultCellStyle.BackColor = Color.CadetBlue; // Alternate row color

            DGVSaleInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVSaleInventory.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVSaleInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVSaleInventory.DefaultCellStyle.ForeColor = Color.White;

            InitializePrinting();
        }

        private void InitializeCartTable()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductId", typeof(int));
            cartTable.Columns.Add("ProductName", typeof(string));
            cartTable.Columns.Add("Price", typeof(decimal)); // Original price
            cartTable.Columns.Add("SalePrice", typeof(decimal)); // Price after discount
            cartTable.Columns.Add("SaleQuantity", typeof(int));
            cartTable.Columns.Add("Discount", typeof(decimal)); // Discount amount
            cartTable.Columns.Add("SaleValue", typeof(decimal)); // Total after discount

            DGVSaleCart.DataSource = cartTable;

            // Set column headers for better readability
            DGVSaleCart.Columns["ProductId"].HeaderText = "ID";
            DGVSaleCart.Columns["ProductName"].HeaderText = "Product Name";
            DGVSaleCart.Columns["Price"].HeaderText = "Price";
            DGVSaleCart.Columns["SalePrice"].HeaderText = "Sale Price";
            DGVSaleCart.Columns["SaleQuantity"].HeaderText = "Quantity";
            DGVSaleCart.Columns["Discount"].HeaderText = "Discount";
            DGVSaleCart.Columns["SaleValue"].HeaderText = "Total";

            // Remove any existing delete button column to avoid duplicates
            if (DGVSaleCart.Columns.Contains("Delete"))
            {
                DGVSaleCart.Columns.Remove("Delete");
            }

            // Add a delete button column
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "Delete";
            deleteButton.HeaderText = "Action";
            deleteButton.Text = "-";
            deleteButton.UseColumnTextForButtonValue = true;
            DGVSaleCart.Columns.Add(deleteButton);
        }

        private void PopulateGridViewSaleInventory(string filter = "")
        {
            try
            {
                string query = @"
            WITH ProductStock AS (
                SELECT ProductId, SUM(Quantity) as TotalPurchased
                FROM PurchaseDetailTable
                GROUP BY ProductId
            ),
            ProductSold AS (
                SELECT ProductId, SUM(SaleQuantity) as TotalSold
                FROM SaleDetailTable
                GROUP BY ProductId
            ),
            ProductDamaged AS (
                SELECT ProductId, SUM(DamageQuantity) as TotalDamaged
                FROM DamageDetailTable
                GROUP BY ProductId
            ),
            ProductReturned AS (
                SELECT ProductId, SUM(ReturnQuantity) as TotalReturned
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
            WHERE (ISNULL(ps.TotalPurchased, 0) 
                   - ISNULL(psold.TotalSold, 0) 
                   - ISNULL(pd.TotalDamaged, 0) 
                   + ISNULL(pr.TotalReturned, 0)) > 0
        ";

                // Apply ProductName or ProductId search
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += " AND (p.ProductName LIKE @filter OR CAST(p.ProductId AS NVARCHAR) LIKE @filter)";
                }

                if (db != null)
                {
                    var parameters = new List<SqlParameter>();
                    if (!string.IsNullOrWhiteSpace(filter))
                        parameters.Add(new SqlParameter("@filter", filter + "%"));

                    var ds = this.db.ExecuteQuery(query, parameters.ToArray());
                    this.DGVSaleInventory.AutoGenerateColumns = true;
                    this.DGVSaleInventory.DataSource = ds.Tables[0];
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

        private void PopulateCustomerComboBox()
        {
            try
            {
                var ds = this.db.ExecuteQuery("SELECT CustomerId, CustomerName FROM CustomerTable");
                cmbCustomerName.DataSource = ds.Tables[0];
                cmbCustomerName.DisplayMember = "CustomerName";
                cmbCustomerName.ValueMember = "CustomerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the customer combo box: " + ex.Message);
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            AddToCartButtonAction();
        }

        private void AddToCartButtonAction()
        {
            if (!string.IsNullOrEmpty(txtBarcodeInput.Text.Trim()))
            {
                AddProductToCartFromBarcode();
            }
            else if (!string.IsNullOrEmpty(txtSearchBar.Text.Trim()))
            {
                AddProductToCartFromSearch();
            }
            else
            {
                MessageBox.Show("Please Fill Barcode Or Search Box", "Fill Instruction", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AddProductToCartFromSearch()
        {
            var productId = 0;
            if (IsTextAnInteger(txtSearchBar.Text))
            {
                productId = Convert.ToInt32(txtSearchBar.Text.Trim());
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Product ID");
                txtSearchBar.Text = string.Empty;
                return;
            }

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
                WHERE
                    COALESCE(pd.Quantity, 0) > 0 AND
                    p.ProductId = " + productId + @"
                GROUP BY 
                    p.ProductId,
                    p.ProductName,
                    c.CategoryName,
                    b.BrandName,
                    p.SalePrice,
                    p.PurchaseCost";

            var ds = db.ExecuteQuery(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                AddProductToCart((int)row["ProductId"], (string)row["ProductName"], (decimal)row["SalePrice"]);
            }
            else
            {
                MessageBox.Show("Product is Not Available");
            }
        }



        //Using Product Name Move to Cart.
        private void AddProductToCartFromSearch(int productId)
        {

            productId = Convert.ToInt32(productId);

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
                WHERE
                    COALESCE(pd.Quantity, 0) > 0 AND
                    p.ProductId = " + productId + @"
                GROUP BY 
                    p.ProductId,
                    p.ProductName,
                    c.CategoryName,
                    b.BrandName,
                    p.SalePrice,
                    p.PurchaseCost";
            var ds = db.ExecuteQuery(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                AddProductToCart((int)row["ProductId"], (string)row["ProductName"], (decimal)row["SalePrice"]);
            }
            else
            {
                MessageBox.Show("Product is Not Available");
                txtSearchBar.Text = string.Empty;
                return;
            }
        }

        private void AddProductToCartFromBarcode()
        {
            var productId = 0;
            if (IsTextAnInteger(txtBarcodeInput.Text))
            {
                productId = Convert.ToInt32(txtBarcodeInput.Text.Trim());
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Barcode");
                return;
            }

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
                WHERE
                    COALESCE(pd.Quantity, 0) > 0 AND
                    p.ProductId = " + productId + @"
                GROUP BY 
                    p.ProductId,
                    p.ProductName,
                    c.CategoryName,
                    b.BrandName,
                    p.SalePrice,
                    p.PurchaseCost";

            var ds = db.ExecuteQuery(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                AddProductToCart((int)row["ProductId"], (string)row["ProductName"], (decimal)row["SalePrice"]);
            }
            else
            {
                MessageBox.Show("This Product Is Not Available");
            }
        }


        //New Validation Features
        private void AddProductToCart(int productId, string productName, decimal salePrice)
        {
            // Get current available quantity for the product
            int currentAvailableQuantity = GetCurrentAvailableQuantity(productId);
            var existingRow = cartTable.AsEnumerable().FirstOrDefault(row => row.Field<int>("ProductId") == productId);
            int currentCartQuantity = existingRow?.Field<int>("SaleQuantity") ?? 0;

            // Check if adding one more item would exceed available stock
            if (currentCartQuantity + 1 > currentAvailableQuantity)
            {
                MessageBox.Show($"Cannot add more items. Available stock: {currentAvailableQuantity}, Current cart quantity: {currentCartQuantity}",
                               "Stock Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the original price
            decimal originalPrice = GetOriginalProductPrice(productId);

            if (existingRow != null)
            {
                existingRow.SetField("SaleQuantity", existingRow.Field<int>("SaleQuantity") + 1);
                // Recalculate values
                int newQuantity = existingRow.Field<int>("SaleQuantity");
                decimal discountPerUnit = originalPrice - existingRow.Field<decimal>("SalePrice");
                existingRow.SetField("Discount", discountPerUnit * newQuantity);
                existingRow.SetField("SaleValue", existingRow.Field<decimal>("SalePrice") * newQuantity);
            }
            else
            {
                int quantity = 1;
                decimal discountPerUnit = originalPrice - salePrice;
                decimal discount = discountPerUnit * quantity;
                decimal saleValue = salePrice * quantity;

                // Add row in the new column order: ProductId, ProductName, Price, SalePrice, SaleQuantity, Discount, SaleValue
                cartTable.Rows.Add(productId, productName, originalPrice, salePrice, quantity, discount, saleValue);
            }
            UpdateCartTotal();
        }

        // Helper method to get the original product price
        private decimal GetOriginalProductPrice(int productId)
        {
            try
            {
                string query = "SELECT SalePrice FROM ProductTable WHERE ProductId = @ProductId";
                var parameters = new[] { new SqlParameter("@ProductId", productId) };
                var ds = db.ExecuteQuery(query, parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToDecimal(ds.Tables[0].Rows[0]["SalePrice"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting product price: " + ex.Message);
                return 0;
            }
        }



        // Add this new method to get current available quantity:
        private int GetCurrentAvailableQuantity(int productId)
        {
            string query = @"
                WITH ProductStock AS (
                    SELECT 
                        ProductId,
                        SUM(Quantity) as TotalPurchased
                    FROM PurchaseDetailTable
                    WHERE ProductId = " + productId + @"
                    GROUP BY ProductId
                ),
                ProductSold AS (
                    SELECT 
                        ProductId,
                        SUM(SaleQuantity) as TotalSold
                    FROM SaleDetailTable
                    WHERE ProductId = " + productId + @"
                    GROUP BY ProductId
                ),
                ProductDamaged AS (
                    SELECT 
                        ProductId,
                        SUM(DamageQuantity) as TotalDamaged
                    FROM DamageDetailTable
                    WHERE ProductId = " + productId + @"
                    GROUP BY ProductId
                ),
                ProductReturned AS (
                    SELECT 
                        ProductId,
                        SUM(ReturnQuantity) as TotalReturned
                    FROM ReturnDetailTable
                    WHERE ProductId = " + productId + @"
                    GROUP BY ProductId
                )
                SELECT 
                    (ISNULL(ps.TotalPurchased, 0) 
                     - ISNULL(psold.TotalSold, 0) 
                     - ISNULL(pd.TotalDamaged, 0) 
                     + ISNULL(pr.TotalReturned, 0)) AS CurrentAvailableQuantity
                FROM 
                    ProductTable p
                    LEFT JOIN ProductStock ps ON p.ProductId = ps.ProductId
                    LEFT JOIN ProductSold psold ON p.ProductId = psold.ProductId
                    LEFT JOIN ProductDamaged pd ON p.ProductId = pd.ProductId
                    LEFT JOIN ProductReturned pr ON p.ProductId = pr.ProductId
                WHERE p.ProductId = " + productId;

            try
            {
                var ds = db.ExecuteQuery(query);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                // For debugging - you can remove this MessageBox later
                MessageBox.Show("Error getting stock quantity: " + ex.Message + "\nQuery: " + query);
                return 0;
            }
        }


        // In DGVSaleCart_CellEndEdit, fix the CS0136 error by renaming the inner 'quantity' variable to 'qty' in the "Discount" and "SaleValue" column blocks.

        private void DGVSaleCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DGVSaleCart.Rows.Count)
            {
                var row = cartTable.Rows[e.RowIndex];
                int productId = Convert.ToInt32(row["ProductId"]);

                // Handle quantity change
                if (DGVSaleCart.Columns[e.ColumnIndex].Name == "SaleQuantity")
                {
                    int newQuantity;
                    if (!int.TryParse(row["SaleQuantity"].ToString(), out newQuantity) || newQuantity <= 0)
                    {
                        MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        row["SaleQuantity"] = 1; // Reset to 1
                        newQuantity = 1;
                    }

                    // Check stock availability
                    int currentAvailableQuantity = GetCurrentAvailableQuantity(productId);
                    if (newQuantity > currentAvailableQuantity)
                    {
                        MessageBox.Show($"Cannot set quantity to {newQuantity}. Available stock: {currentAvailableQuantity}",
                                       "Stock Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Reset to available quantity
                        row["SaleQuantity"] = currentAvailableQuantity;
                        newQuantity = currentAvailableQuantity;
                    }

                    // Recalculate values
                    decimal origPrice = Convert.ToDecimal(row["Price"]);
                    decimal salePrice = Convert.ToDecimal(row["SalePrice"]);
                    decimal discountPerUnit = origPrice - salePrice;
                    row["Discount"] = discountPerUnit * newQuantity;
                    row["SaleValue"] = salePrice * newQuantity;

                    UpdateCartTotal();
                }

                // Handle sale price change
                if (DGVSaleCart.Columns[e.ColumnIndex].Name == "SalePrice")
                {
                    decimal newSalePrice;
                    if (!decimal.TryParse(row["SalePrice"].ToString(), out newSalePrice) || newSalePrice < 0)
                    {
                        MessageBox.Show("Please enter a valid sale price.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Reset to original price
                        decimal origPrice = Convert.ToDecimal(row["Price"]);
                        row["SalePrice"] = origPrice;
                        newSalePrice = origPrice;
                    }

                    int qty = Convert.ToInt32(row["SaleQuantity"]);
                    decimal origPrice2 = Convert.ToDecimal(row["Price"]);

                    // Calculate discount
                    decimal discountPerUnit = origPrice2 - newSalePrice;
                    row["Discount"] = discountPerUnit * qty;

                    // Recalculate sale value
                    row["SaleValue"] = newSalePrice * qty;

                    UpdateCartTotal();
                }

                // Handle discount change
                if (DGVSaleCart.Columns[e.ColumnIndex].Name == "Discount")
                {
                    decimal newDiscount;
                    if (!decimal.TryParse(row["Discount"].ToString(), out newDiscount) || newDiscount < 0)
                    {
                        MessageBox.Show("Please enter a valid discount amount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Reset to original discount
                        decimal origPrice = Convert.ToDecimal(row["Price"]);
                        decimal salePrice = Convert.ToDecimal(row["SalePrice"]);
                        int qty = Convert.ToInt32(row["SaleQuantity"]);
                        decimal originalDiscount = (origPrice - salePrice) * qty;
                        row["Discount"] = originalDiscount;
                        newDiscount = originalDiscount;
                    }

                    int qty2 = Convert.ToInt32(row["SaleQuantity"]);
                    decimal origPrice2 = Convert.ToDecimal(row["Price"]);

                    // Calculate new sale price based on discount
                    decimal discountPerUnit = newDiscount / qty2;
                    decimal newSalePrice = origPrice2 - discountPerUnit;

                    // Update values
                    row["SalePrice"] = newSalePrice;
                    row["SaleValue"] = newSalePrice * qty2;

                    UpdateCartTotal();
                }

                // Handle total sale value change
                if (DGVSaleCart.Columns[e.ColumnIndex].Name == "SaleValue")
                {
                    decimal newSaleValue;
                    if (!decimal.TryParse(row["SaleValue"].ToString(), out newSaleValue) || newSaleValue < 0)
                    {
                        MessageBox.Show("Please enter a valid sale value.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Reset to current sale price * quantity
                        decimal salePrice = Convert.ToDecimal(row["SalePrice"]);
                        int qty = Convert.ToInt32(row["SaleQuantity"]);
                        row["SaleValue"] = salePrice * qty;
                        newSaleValue = salePrice * qty;
                    }

                    int qty2 = Convert.ToInt32(row["SaleQuantity"]);
                    if (qty2 == 0)
                    {
                        qty2 = 1;
                        row["SaleQuantity"] = qty2;
                    }

                    // Calculate new sale price per unit
                    decimal newSalePrice = newSaleValue / qty2;
                    row["SalePrice"] = newSalePrice;

                    // Calculate discount
                    decimal origPrice = Convert.ToDecimal(row["Price"]);
                    decimal discountPerUnit = origPrice - newSalePrice;
                    row["Discount"] = discountPerUnit * qty2;

                    UpdateCartTotal();
                }
            }
        }


        // Method to recalculate and update the total
        private void UpdateCartTotal()
        {
            decimal total = 0;

            foreach (DataRow row in cartTable.Rows)
            {
                if (row["SaleValue"] != DBNull.Value)
                {
                    total += Convert.ToDecimal(row["SaleValue"]);
                }
            }

            txtTotal.Text = total.ToString("F2"); // Format to 2 decimal places
        }



        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            ProcessCheckout();
        }


        private void ProcessCheckout()
        {
            decimal discountValue = 0;

            if (string.IsNullOrEmpty(txtPayment.Text) || !IsTextAnDecimal(txtPayment.Text))
            {
                MessageBox.Show("Please Enter Valid Payment Amount.");
                return;
            }
            if (string.IsNullOrEmpty(txtTotal.Text) || !IsTextAnDecimal(txtTotal.Text))
            {
                MessageBox.Show("Please Enter Valid Total Amount.");
                return;
            }
            if (cmbCustomerName.SelectedValue == null)
            {
                MessageBox.Show("Please Select Customer");
                return;
            }

            decimal txtPaymentValue = Convert.ToDecimal(txtPayment.Text.Trim());
            decimal lblTotalValue = Convert.ToDecimal(txtTotal.Text.Trim());

            int customerId = Convert.ToInt32(cmbCustomerName.SelectedValue);
            string customerName = cmbCustomerName.Text;
            bool isWalkInCustomer = (customerId == 1) || (cmbCustomerName.Text == "Walk-In Customer");

            // Your existing validations here...
            if (isWalkInCustomer && !chkbDiscountClick)
            {
                if (txtPaymentValue < lblTotalValue)
                {
                    MessageBox.Show("Walk-In Customer must pay the full amount or provide discount by checking the 'Discounted?' checkbox.",
                                   "Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (chkbDiscountClick)
            {
                if (txtPaymentValue < lblTotalValue)
                {
                    discountValue = lblTotalValue - txtPaymentValue;
                }
            }
            else
            {
                if (!isWalkInCustomer && txtPaymentValue < lblTotalValue)
                {
                    // This is allowed for regular customers
                }
                else if (txtPaymentValue < lblTotalValue)
                {
                    MessageBox.Show("Payment amount is less than total. Please check the discount option if this is intentional.",
                                   "Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var payAmount = cartTable.AsEnumerable().Sum(row => row.Field<decimal>("SaleValue"));
            var paidAmount = Convert.ToDecimal(txtPayment.Text.Trim());
            var dueAmount = payAmount - paidAmount - discountValue;

            string transactionType = "";
            if (cmbTransactionType.SelectedItem != null)
            {
                transactionType = cmbTransactionType.SelectedItem.ToString();
            }
            else
            {
                transactionType = "CASH";
            }

            lblChange.Text = (paidAmount - (payAmount - discountValue)).ToString();

            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var saleId = InsertSaleRecord(transaction, customerId, transactionType, discountValue, payAmount, paidAmount);

                    foreach (DataRow row in cartTable.Rows)
                    {
                        InsertSaleDetailRecord(transaction, saleId, row);
                    }

                    if (dueAmount > 0 && (!isWalkInCustomer || chkbDiscountClick))
                    {
                        if (!chkbDiscountClick)
                        {
                            InsertOrUpdateCustomerDueRecord(transaction, customerId, saleId, dueAmount);
                        }
                    }

                    transaction.Commit();

                    // Ask user for invoice type preference
                    DialogResult invoiceChoice = ShowInvoiceTypeDialog();

                    if (invoiceChoice != DialogResult.Cancel)
                    {
                        bool isPdfInvoice = (invoiceChoice == DialogResult.Yes);

                        if (isPdfInvoice)
                        {
                            GeneratePdfInvoice(saleId, customerId, customerName, payAmount, paidAmount, discountValue, transactionType);
                        }
                        else
                        {
                            PreviewPremiumTextInvoice(saleId, customerId, customerName, payAmount, paidAmount, discountValue, transactionType);
                        }
                    }

                    ClearCart();
                    PopulateGridViewSaleInventory();

                    chkbDiscountClick = false;
                    chkbDiscount.Checked = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred during checkout: " + ex.Message);
                }
            }
        }

        private int InsertSaleRecord(SqlTransaction transaction, int customerId, string transactionType,decimal discountValue, decimal payAmount, decimal paidAmount)
        {
            var query = @"
                INSERT INTO SaleTable (CustomerId,TransactionType, Discount, PayAmount, PaidAmount)
                OUTPUT INSERTED.SaleId
                VALUES (@CustomerId, @TransactionType, @Discount, @PayAmount, @PaidAmount)";

            var parameters = new[]
            {
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@TransactionType", transactionType),
                new SqlParameter("@Discount", discountValue),
                new SqlParameter("@PayAmount", payAmount),
                new SqlParameter("@PaidAmount", paidAmount)
            };

            return Convert.ToInt32(db.ExecuteScalarQuery(query, transaction, parameters));

        }

        private void InsertSaleDetailRecord(SqlTransaction transaction, int saleId, DataRow row)
        {

            var query = @"
                INSERT INTO SaleDetailTable (SaleId, ProductId, SaleQuantity, UnitPrice)
                VALUES (@SaleId, @ProductId, @SaleQuantity, @SalePrice)";


            var parameters = new[]
            {
                new SqlParameter("@SaleId", saleId),
                new SqlParameter("@ProductId", row["ProductId"]),
                new SqlParameter("@SaleQuantity", row["SaleQuantity"]),
                new SqlParameter("@SalePrice", row["SalePrice"])
            };


            db.ExecuteNonQuery(query, transaction, parameters);

        }

        public void InsertOrUpdateCustomerDueRecord(SqlTransaction transaction, int customerId, int saleId, decimal dueAmount)
        {
            string checkQuery = @"
        SELECT COUNT(*) FROM CustomerDueTable 
        WHERE CustomerId = @CustomerId";

            string insertQuery = @"
        INSERT INTO CustomerDueTable (CustomerId, SaleId, DueAmount) 
        VALUES (@CustomerId, @SaleId, @DueAmount)";

            string updateQuery = @"
        UPDATE CustomerDueTable 
        SET DueAmount = DueAmount + @DueAmount
        WHERE CustomerId = @CustomerId";

            using (SqlCommand checkCmd = new SqlCommand(checkQuery, transaction.Connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@CustomerId", customerId);
                checkCmd.Parameters.AddWithValue("@SaleId", saleId);

                int recordCount = (int)checkCmd.ExecuteScalar();

                if (recordCount > 0)
                {
                    // Record exists, update it
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, transaction.Connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@CustomerId", customerId);
                        updateCmd.Parameters.AddWithValue("@SaleId", saleId);
                        updateCmd.Parameters.AddWithValue("@DueAmount", dueAmount);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Record does not exist, insert it
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, transaction.Connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@CustomerId", customerId);
                        insertCmd.Parameters.AddWithValue("@SaleId", saleId);
                        insertCmd.Parameters.AddWithValue("@DueAmount", dueAmount);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        private void DGVSaleCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on a valid row and the Delete column
            if (e.RowIndex >= 0 && e.ColumnIndex == DGVSaleCart.Columns["Delete"].Index)
            {
                try
                {
                    RemoveProductFromCart(e.RowIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RemoveProductFromCart(int rowIndex)
        {
            // Check if rowIndex is valid
            if (rowIndex < 0 || rowIndex >= cartTable.Rows.Count)
            {
                return;
            }

            var row = cartTable.Rows[rowIndex];
            int currentQuantity = row.Field<int>("SaleQuantity");

            if (currentQuantity > 1)
            {
                // Decrement quantity and recalculate values
                row.SetField("SaleQuantity", currentQuantity - 1);
                int newQuantity = currentQuantity - 1;

                decimal originalPrice = row.Field<decimal>("Price");
                decimal salePrice = row.Field<decimal>("SalePrice");
                decimal discountPerUnit = originalPrice - salePrice;

                row.SetField("Discount", discountPerUnit * newQuantity);
                row.SetField("SaleValue", salePrice * newQuantity);
            }
            else
            {
                // Remove the entire row when quantity becomes 0
                cartTable.Rows.RemoveAt(rowIndex);
            }

            UpdateCartTotal();
        }



        private void AddProductToCartFromInventory()
        {
            if (DGVSaleInventory.SelectedRows.Count > 0)
            {
                var selectedRow = DGVSaleInventory.SelectedRows[0];
                AddProductToCart((int)selectedRow.Cells["ProductId"].Value,
                                 (string)selectedRow.Cells["ProductName"].Value,
                                 (decimal)selectedRow.Cells["SalePrice"].Value);
            }
        }


        //Checking is Text is Decimal or Integer
        public bool IsTextAnDecimal(string text)
        {
            decimal result;
            return decimal.TryParse(text, out result);
        }

        public bool IsTextAnInteger(string text)
        {
            int result;
            return int.TryParse(text, out result);
        }

        private void ClearCart()
        {
            cartTable.Rows.Clear();
            UpdateCartTotal();
            lblChange.Text = string.Empty;
            txtPayment.Clear();
        }

        private void txtSearchBar_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewSaleInventory(txtSearchBar.Text.Trim());
        }


        private void DGVSaleInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int productId = Convert.ToInt32(DGVSaleInventory.CurrentRow.Cells["ProductId"].Value);
            AddProductToCartFromSearch(productId);
        }

        private void txtSearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddProductToCartFromSearch();
            }
        }

        private void txtBarcodeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddProductToCartFromBarcode();
            }
        }

        private void chkbDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbDiscount.Checked)
            {
                chkbDiscountClick = true;
            }
        }

        private void cmbCustomerName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCustomerName.SelectedText == "Walk-In-Customer")
            {
                walkInCustomer = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            SalesmanMainDashBoard salesmanMainDashBoard = new SalesmanMainDashBoard();
            salesmanMainDashBoard.Show();
        }

        private void btnReturnAndDamage_Click(object sender, EventArgs e)
        {
            ReturnProducts returnProducts = new ReturnProducts();
            returnProducts.Show();
            this.Hide();
        }

        private void pcbxOpenCustomerForm_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.Show();
        }

        private void DGVSaleCart_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleCart.ClearSelection();
            //    DGVSaleCart.CurrentCell = null;
        }

        private void DGVSaleInventory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleInventory.ClearSelection();
            DGVSaleInventory.CurrentCell = null;
        }

        private void pcbxRelodeCustomer_Click(object sender, EventArgs e)
        {
            PopulateCustomerComboBox();
        }





























        // Printing functionality

        private DialogResult ShowInvoiceTypeDialog()
        {
            Form invoiceDialog = new Form();
            invoiceDialog.Text = "Invoice Type Selection";
            invoiceDialog.Size = new Size(400, 200);
            invoiceDialog.StartPosition = FormStartPosition.CenterParent;
            invoiceDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            invoiceDialog.MaximizeBox = false;
            invoiceDialog.MinimizeBox = false;

            Label lblQuestion = new Label();
            lblQuestion.Text = "Which type of invoice would you like to generate?";
            lblQuestion.Size = new Size(350, 40);
            lblQuestion.Location = new Point(25, 20);
            lblQuestion.TextAlign = ContentAlignment.MiddleCenter;
            lblQuestion.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);

            Button btnPdf = new Button();
            btnPdf.Text = "Premium PDF Invoice";
            btnPdf.Size = new Size(150, 40);
            btnPdf.Location = new Point(50, 80);
            btnPdf.BackColor = Color.FromArgb(52, 152, 219);
            btnPdf.ForeColor = Color.White;
            btnPdf.FlatStyle = FlatStyle.Flat;
            btnPdf.DialogResult = DialogResult.Yes;

            Button btnText = new Button();
            btnText.Text = "Premium Text Invoice";
            btnText.Size = new Size(150, 40);
            btnText.Location = new Point(210, 80);
            btnText.BackColor = Color.FromArgb(46, 204, 113);
            btnText.ForeColor = Color.White;
            btnText.FlatStyle = FlatStyle.Flat;
            btnText.DialogResult = DialogResult.No;

            Button btnCancel = new Button();
            btnCancel.Text = "Skip";
            btnCancel.Size = new Size(100, 25);
            btnCancel.Location = new Point(150, 135);
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.DialogResult = DialogResult.Cancel;

            invoiceDialog.Controls.Add(lblQuestion);
            invoiceDialog.Controls.Add(btnPdf);
            invoiceDialog.Controls.Add(btnText);
            invoiceDialog.Controls.Add(btnCancel);

            return invoiceDialog.ShowDialog();
        }

        // Enhanced Premium Text-based Invoice
        private string GeneratePremiumTextInvoice(int saleId, int customerId, string customerName,
     decimal totalAmount, decimal paidAmount, decimal discountValue, string transactionType)
        {
            StringBuilder invoice = new StringBuilder();

            // Get sale details
            var saleDetails = GetSaleDetailsForInvoice(saleId);
            var currentDate = DateTime.Now;

            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine("                        Stor's Name");
            invoice.AppendLine("                    INVOICE RECEIPT");
            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine();
            invoice.AppendLine($"Invoice#: {saleId.ToString().PadRight(20)} Date: {currentDate.ToString("dd/MM/yyyy")}");
            invoice.AppendLine($"Time: {currentDate.ToString("HH:mm:ss")}");
            invoice.AppendLine();
            invoice.AppendLine("Invoice to:");
            invoice.AppendLine($"{customerName}");
            invoice.AppendLine();
            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine("SL.  Item Description              Price    Sale Price  Qty.   Discount   Total");
            invoice.AppendLine("═══════════════════════════════════════════════════════════");

            int slNo = 1;
            foreach (DataRow row in saleDetails.Rows)
            {
                string itemName = row["ProductName"].ToString();
                if (itemName.Length > 20) itemName = itemName.Substring(0, 20);

                decimal originalPrice = Convert.ToDecimal(row["OriginalPrice"]);
                decimal salePrice = Convert.ToDecimal(row["UnitPrice"]);
                int qty = Convert.ToInt32(row["SaleQuantity"]);
                decimal discount = Convert.ToDecimal(row["DiscountAmount"]);
                decimal total = salePrice * qty;

                invoice.AppendLine($"{slNo.ToString().PadRight(4)} {itemName.PadRight(20)} ${originalPrice.ToString("F2").PadLeft(8)} ${salePrice.ToString("F2").PadLeft(10)} {qty.ToString().PadLeft(4)} ${discount.ToString("F2").PadLeft(8)} ${total.ToString("F2").PadLeft(8)}");
                slNo++;
            }

            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine();

            decimal subTotal = totalAmount + discountValue; // Add discount back to get original total

            invoice.AppendLine($"{"Sub Total:".PadLeft(45)} ${subTotal.ToString("F2").PadLeft(10)}");

            if (discountValue > 0)
            {
                invoice.AppendLine($"{"Discount:".PadLeft(45)} ${discountValue.ToString("F2").PadLeft(10)}");
            }

            invoice.AppendLine($"{"Tax:".PadLeft(45)} ${"0.00".PadLeft(10)}");
            invoice.AppendLine("────────────────────────────────────────────────────────────");
            invoice.AppendLine($"{"TOTAL:".PadLeft(45)} ${totalAmount.ToString("F2").PadLeft(10)}");
            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine();
            invoice.AppendLine($"Payment Method: {transactionType}");
            invoice.AppendLine($"Amount Paid: ${paidAmount.ToString("F2")}");

            decimal changeAmount = paidAmount - totalAmount;
            if (changeAmount > 0)
            {
                invoice.AppendLine($"Change: ${changeAmount.ToString("F2")}");
            }
            else if (changeAmount < 0)
            {
                invoice.AppendLine($"Due Amount: ${Math.Abs(changeAmount).ToString("F2")}");
            }

            invoice.AppendLine();
            invoice.AppendLine("Thank you for your business!");
            invoice.AppendLine();
            invoice.AppendLine("Terms & Conditions");
            invoice.AppendLine("All sales are final. Please check your items before leaving.");
            invoice.AppendLine();
            invoice.AppendLine("═══════════════════════════════════════════════════════════");
            invoice.AppendLine("Contact: [Your Phone] | [Your Address] | [Your Website]");
            invoice.AppendLine("═══════════════════════════════════════════════════════════");

            return invoice.ToString();
        }

        private DataTable GetSaleDetailsForInvoice(int saleId)
        {
            string query = @"
        SELECT 
            sd.SaleId,
            sd.ProductId,
            p.ProductName,
            sd.SaleQuantity,
            sd.UnitPrice,
            p.SalePrice as OriginalPrice,
            (sd.SaleQuantity * sd.UnitPrice) as TotalPrice,
            -- Calculate discount based on original price vs. sale price
            (p.SalePrice - sd.UnitPrice) * sd.SaleQuantity as DiscountAmount
        FROM SaleDetailTable sd
        INNER JOIN ProductTable p ON sd.ProductId = p.ProductId
        WHERE sd.SaleId = @SaleId
        ORDER BY sd.ProductId";
            var parameters = new[] { new SqlParameter("@SaleId", saleId) };
            var ds = db.ExecuteQuery(query, parameters);
            return ds.Tables[0];
        }




        // Method to preview premium text invoice
        private void PreviewPremiumTextInvoice(int saleId, int customerId, string customerName,
            decimal totalAmount, decimal paidAmount, decimal discountValue, string transactionType)
        {
            try
            {
                string invoiceContent = GeneratePremiumTextInvoice(saleId, customerId, customerName,
                    totalAmount, paidAmount, discountValue, transactionType);

                Form previewForm = new Form();
                previewForm.Text = "Premium Text Invoice Preview";
                previewForm.Size = new Size(500, 700);
                previewForm.StartPosition = FormStartPosition.CenterScreen;
                previewForm.BackColor = Color.FromArgb(240, 240, 240);

                TextBox textBox = new TextBox();
                textBox.Multiline = true;
                textBox.ScrollBars = ScrollBars.Both;
                textBox.Font = new System.Drawing.Font("Consolas", 9, FontStyle.Regular);
                textBox.Text = invoiceContent;
                textBox.ReadOnly = true;
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.Black;
                textBox.BorderStyle = BorderStyle.None;
                textBox.Margin = new Padding(10);
                textBox.Dock = DockStyle.Fill;

                Panel buttonPanel = new Panel();
                buttonPanel.Height = 60;
                buttonPanel.Dock = DockStyle.Bottom;
                buttonPanel.BackColor = Color.FromArgb(52, 73, 94);

                Button printButton = new Button();
                printButton.Text = "🖨️ Print Invoice";
                printButton.Size = new Size(120, 35);
                printButton.Location = new Point(80, 12);
                printButton.BackColor = Color.FromArgb(46, 204, 113);
                printButton.ForeColor = Color.White;
                printButton.FlatStyle = FlatStyle.Flat;
                printButton.Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold);
                printButton.Click += (s, e) => {
                    invoiceToPrint = invoiceContent;
                    PrintDialog printDialog = new PrintDialog();
                    printDialog.Document = printDocument;
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDocument.Print();
                    }
                };


                Button closeButton = new Button();
                closeButton.Text = "❌ Close";
                closeButton.Size = new Size(100, 35);
                closeButton.Location = new Point(275, 12);
                closeButton.BackColor = Color.FromArgb(231, 76, 60);
                closeButton.ForeColor = Color.White;
                closeButton.FlatStyle = FlatStyle.Flat;
                closeButton.Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold);
                closeButton.Click += (s, e) => previewForm.Close();

                buttonPanel.Controls.Add(printButton);
                buttonPanel.Controls.Add(closeButton);

                previewForm.Controls.Add(textBox);
                previewForm.Controls.Add(buttonPanel);

                previewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error previewing invoice: " + ex.Message);
            }
        }


        // Premium PDF Invoice Generation
        private void GeneratePdfInvoice(int saleId, int customerId, string customerName,
            decimal totalAmount, decimal paidAmount, decimal discountValue, string transactionType)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Invoice_{saleId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    CreatePdfInvoice(saveFileDialog.FileName, saleId, customerId, customerName,
                        totalAmount, paidAmount, discountValue, transactionType);

                    DialogResult result = MessageBox.Show("PDF Invoice generated successfully! Would you like to open it?",
                        "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF invoice: " + ex.Message);
            }
        }

        // Create PDF Invoice with premium design
        private void CreatePdfInvoice(string filePath, int saleId, int customerId, string customerName,
            decimal totalAmount, decimal paidAmount, decimal discountValue, string transactionType)
        {
            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Colors
            BaseColor primaryColor = new BaseColor(255, 193, 7); // Golden Yellow
            BaseColor secondaryColor = new BaseColor(52, 58, 64); // Dark Gray
            BaseColor accentColor = new BaseColor(40, 167, 69); // Green

            // Fonts
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 24, iTextSharp.text.Font.BOLD, secondaryColor);
            iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, secondaryColor);
            iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, secondaryColor);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, secondaryColor);

            // Header Section with colored background
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 70, 30 });

            // Company Info Cell
            PdfPCell companyCell = new PdfPCell();
            companyCell.Border = Rectangle.NO_BORDER;
            companyCell.BackgroundColor = primaryColor;
            companyCell.Padding = 20;

            Paragraph companyName = new Paragraph("SOTRE'S NAME", titleFont);
            companyName.Alignment = Element.ALIGN_LEFT;
            companyCell.AddElement(companyName);

            Paragraph tagline = new Paragraph("TAGLINE SPACE HERE", new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.ITALIC, secondaryColor));
            tagline.Alignment = Element.ALIGN_LEFT;
            companyCell.AddElement(tagline);

            // Invoice Label Cell
            PdfPCell invoiceCell = new PdfPCell();
            invoiceCell.Border = Rectangle.NO_BORDER;
            invoiceCell.BackgroundColor = primaryColor;
            invoiceCell.Padding = 20;
            invoiceCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            Paragraph invoiceLabel = new Paragraph("INVOICE", new iTextSharp.text.Font(baseFont, 25, iTextSharp.text.Font.BOLD, BaseColor.WHITE));
            invoiceLabel.Alignment = Element.ALIGN_RIGHT;
            invoiceCell.AddElement(invoiceLabel);

            headerTable.AddCell(companyCell);
            headerTable.AddCell(invoiceCell);
            document.Add(headerTable);

            // Invoice Details Section
            PdfPTable detailsTable = new PdfPTable(2);
            detailsTable.WidthPercentage = 100;
            detailsTable.SetWidths(new float[] { 60, 40 });
            detailsTable.SpacingBefore = 20;

            // Customer Info
            PdfPCell customerCell = new PdfPCell();
            customerCell.Border = Rectangle.NO_BORDER;
            customerCell.Padding = 10;

            Paragraph invoiceToLabel = new Paragraph("Invoice to:", headerFont);
            customerCell.AddElement(invoiceToLabel);

            Paragraph customerInfo = new Paragraph($"{customerName}\nCustomer ID: {customerId}", normalFont);
            customerCell.AddElement(customerInfo);

            // Invoice Info
            PdfPCell invoiceInfoCell = new PdfPCell();
            invoiceInfoCell.Border = Rectangle.NO_BORDER;
            invoiceInfoCell.Padding = 10;

            DateTime currentDate = DateTime.Now;
            Paragraph invoiceNumber = new Paragraph($"Invoice#: {saleId}", boldFont);
            invoiceNumber.Alignment = Element.ALIGN_RIGHT;
            invoiceInfoCell.AddElement(invoiceNumber);

            Paragraph invoiceDate = new Paragraph($"Date: {currentDate.ToString("dd/MM/yyyy")}", normalFont);
            invoiceDate.Alignment = Element.ALIGN_RIGHT;
            invoiceInfoCell.AddElement(invoiceDate);

            detailsTable.AddCell(customerCell);
            detailsTable.AddCell(invoiceInfoCell);
            document.Add(detailsTable);

            // Items Table
            PdfPTable itemsTable = new PdfPTable(7); // Changed from 6 to 7 to match the number of columns
            itemsTable.WidthPercentage = 100;
            itemsTable.SetWidths(new float[] { 8, 30, 12, 12, 10, 12, 16 }); // This is correct for 7 columns
            itemsTable.SpacingBefore = 30;

            // Table Headers
            string[] headers = { "SL.", "Item Description", "Price", "Sale Price", "Qty.", "Discount", "Total" }; // 7 headers
            foreach (string header in headers)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(header, new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE)));
                headerCell.BackgroundColor = secondaryColor;
                headerCell.Padding = 8;
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                itemsTable.AddCell(headerCell);
            }

            // Table Data
            var saleDetails = GetSaleDetailsForInvoice(saleId);
            int slNo = 1;
            foreach (DataRow row in saleDetails.Rows)
            {
                // Add alternating row colors
                BaseColor rowColor = (slNo % 2 == 0) ? new BaseColor(248, 249, 250) : BaseColor.WHITE;

                PdfPCell slCell = new PdfPCell(new Phrase(slNo.ToString(), normalFont));
                slCell.BackgroundColor = rowColor;
                slCell.Padding = 6;
                slCell.HorizontalAlignment = Element.ALIGN_CENTER;
                itemsTable.AddCell(slCell);

                PdfPCell nameCell = new PdfPCell(new Phrase(row["ProductName"].ToString(), normalFont));
                nameCell.BackgroundColor = rowColor;
                nameCell.Padding = 6;
                itemsTable.AddCell(nameCell);

                decimal originalPrice = Convert.ToDecimal(row["OriginalPrice"]);
                PdfPCell originalPriceCell = new PdfPCell(new Phrase($"৳{originalPrice:F2}", normalFont));
                originalPriceCell.BackgroundColor = rowColor;
                originalPriceCell.Padding = 6;
                originalPriceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                itemsTable.AddCell(originalPriceCell);

                decimal salePrice = Convert.ToDecimal(row["UnitPrice"]);
                PdfPCell salePriceCell = new PdfPCell(new Phrase($"৳{salePrice:F2}", normalFont));
                salePriceCell.BackgroundColor = rowColor;
                salePriceCell.Padding = 6;
                salePriceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                itemsTable.AddCell(salePriceCell);

                int qty = Convert.ToInt32(row["SaleQuantity"]);
                PdfPCell qtyCell = new PdfPCell(new Phrase(qty.ToString(), normalFont));
                qtyCell.BackgroundColor = rowColor;
                qtyCell.Padding = 6;
                qtyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                itemsTable.AddCell(qtyCell);

                decimal discount = Convert.ToDecimal(row["DiscountAmount"]);
                PdfPCell discountCell = new PdfPCell(new Phrase($"৳{discount:F2}", normalFont));
                discountCell.BackgroundColor = rowColor;
                discountCell.Padding = 6;
                discountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                itemsTable.AddCell(discountCell);

                decimal total = salePrice * qty;
                PdfPCell totalCell = new PdfPCell(new Phrase($"৳{total:F2}", normalFont));
                totalCell.BackgroundColor = rowColor;
                totalCell.Padding = 6;
                totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                itemsTable.AddCell(totalCell);

                slNo++;
            }

            document.Add(itemsTable);

            // Totals Section
            PdfPTable totalsTable = new PdfPTable(2);
            totalsTable.WidthPercentage = 100;
            totalsTable.SetWidths(new float[] { 70, 30 });
            totalsTable.SpacingBefore = 20;

            // Thank you message
            PdfPCell thankYouCell = new PdfPCell();
            thankYouCell.Border = Rectangle.NO_BORDER;
            thankYouCell.Padding = 10;

            Paragraph thankYou = new Paragraph("Thank you for your business", headerFont);
            thankYouCell.AddElement(thankYou);

            Paragraph terms = new Paragraph("Terms & Conditions\nAll sales are final. Please check your items before leaving.",
                new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, BaseColor.GRAY));
            thankYouCell.AddElement(terms);

            // Totals calculation
            PdfPCell totalsCell = new PdfPCell();
            totalsCell.Border = Rectangle.NO_BORDER;
            totalsCell.Padding = 10;

            decimal subTotal = totalAmount + discountValue;

            Paragraph subTotalPara = new Paragraph($"Sub Total: ৳{subTotal:F2}", normalFont);
            subTotalPara.Alignment = Element.ALIGN_RIGHT;
            totalsCell.AddElement(subTotalPara);

            if (discountValue > 0)
            {
                Paragraph discountPara = new Paragraph($"Discount: ৳{discountValue:F2}", normalFont);
                discountPara.Alignment = Element.ALIGN_RIGHT;
                totalsCell.AddElement(discountPara);
            }

            Paragraph taxPara = new Paragraph("Tax: 0.00%", normalFont);
            taxPara.Alignment = Element.ALIGN_RIGHT;
            totalsCell.AddElement(taxPara);

            // Grand Total with highlighting
            PdfPCell grandTotalCell = new PdfPCell();
            grandTotalCell.BackgroundColor = primaryColor;
            grandTotalCell.Padding = 10;
            grandTotalCell.Border = Rectangle.NO_BORDER;

            Paragraph grandTotal = new Paragraph($"Total: ৳{totalAmount:F2}",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD, BaseColor.WHITE));
            grandTotal.Alignment = Element.ALIGN_RIGHT;
            grandTotalCell.AddElement(grandTotal);

            totalsTable.AddCell(thankYouCell);
            totalsTable.AddCell(totalsCell);
            document.Add(totalsTable);

            // Add Grand Total row
            PdfPTable grandTotalTable = new PdfPTable(1);
            grandTotalTable.WidthPercentage = 30;
            grandTotalTable.HorizontalAlignment = Element.ALIGN_RIGHT;
            grandTotalTable.SpacingBefore = 5;
            grandTotalTable.AddCell(grandTotalCell);
            document.Add(grandTotalTable);

            // Payment Info
            PdfPTable paymentTable = new PdfPTable(1);
            paymentTable.WidthPercentage = 100;
            paymentTable.SpacingBefore = 20;

            PdfPCell paymentCell = new PdfPCell();
            paymentCell.Border = Rectangle.NO_BORDER;
            paymentCell.Padding = 15;
            paymentCell.BackgroundColor = new BaseColor(248, 249, 250);

            Paragraph paymentInfo = new Paragraph("Payment Info:", headerFont);
            paymentCell.AddElement(paymentInfo);

            Paragraph paymentDetails = new Paragraph($"Payment Method: {transactionType}\nAmount Paid: ৳{paidAmount:F2}", normalFont);
            paymentCell.AddElement(paymentDetails);

            decimal changeAmount = paidAmount - totalAmount;
            if (changeAmount > 0)
            {
                Paragraph changePara = new Paragraph($"Change: ৳{changeAmount:F2}", normalFont);
                paymentCell.AddElement(changePara);
            }
            else if (changeAmount < 0)
            {
                Paragraph duePara = new Paragraph($"Due Amount: ৳{Math.Abs(changeAmount):F2}",
                    new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, BaseColor.RED));
                paymentCell.AddElement(duePara);
            }

            paymentTable.AddCell(paymentCell);
            document.Add(paymentTable);

            // Footer
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.WidthPercentage = 100;
            footerTable.SpacingBefore = 30;

            PdfPCell footerCell = new PdfPCell();
            footerCell.Border = Rectangle.TOP_BORDER;
            footerCell.BorderColor = primaryColor;
            footerCell.BorderWidth = 2;
            footerCell.Padding = 15;

            Paragraph contact = new Paragraph("Phone # | Address | Website",
                new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, BaseColor.GRAY));
            contact.Alignment = Element.ALIGN_CENTER;
            footerCell.AddElement(contact);

            Paragraph signature = new Paragraph("Authorised Sign",
                new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, BaseColor.GRAY));
            signature.Alignment = Element.ALIGN_RIGHT;
            signature.SpacingBefore = 20;
            footerCell.AddElement(signature);

            footerTable.AddCell(footerCell);
            document.Add(footerTable);

            document.Close();
        }

        // Enhanced Print Document event handler for text invoice
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Set up font and brush for premium text invoice
            System.Drawing.Font font = new System.Drawing.Font("Consolas", 8);
            SolidBrush brush = new SolidBrush(Color.Black);

            // Calculate the area within the page margins
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float lineHeight = font.GetHeight(e.Graphics);

            // Split the invoice content into lines
            string[] lines = invoiceToPrint.Split('\n');

            float yPos = topMargin;

            // Print each line
            foreach (string line in lines)
            {
                if (yPos + lineHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    break;
                }

                e.Graphics.DrawString(line, font, brush, leftMargin, yPos);
                yPos += lineHeight;
            }

            font.Dispose();
            brush.Dispose();
        }
        private DialogResult ShowEnhancedInvoiceDialog()
        {
            Form invoiceDialog = new Form();
            invoiceDialog.Text = "Select Invoice Type";
            invoiceDialog.Size = new Size(500, 300);
            invoiceDialog.StartPosition = FormStartPosition.CenterParent;
            invoiceDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            invoiceDialog.MaximizeBox = false;
            invoiceDialog.MinimizeBox = false;
            invoiceDialog.BackColor = Color.FromArgb(248, 249, 250);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "🧾 Choose Your Invoice Style";
            lblTitle.Size = new Size(460, 50);
            lblTitle.Location = new Point(20, 20);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 58, 64);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Select the type of invoice you'd like to generate for your customer";
            lblSubtitle.Size = new Size(460, 30);
            lblSubtitle.Location = new Point(20, 60);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.FromArgb(108, 117, 125);

            // PDF Button
            Button btnPdf = new Button();
            btnPdf.Text = "📄 Premium PDF Invoice\n• Professional Layout\n• Easy to Share\n• Print Ready";
            btnPdf.Size = new Size(180, 100);
            btnPdf.Location = new Point(60, 110);
            btnPdf.BackColor = Color.FromArgb(52, 152, 219);
            btnPdf.ForeColor = Color.White;
            btnPdf.FlatStyle = FlatStyle.Flat;
            btnPdf.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            btnPdf.TextAlign = ContentAlignment.MiddleCenter;
            btnPdf.FlatAppearance.BorderSize = 0;
            btnPdf.DialogResult = DialogResult.Yes;

            // Text Button  
            Button btnText = new Button();
            btnText.Text = "📝 Premium Text Invoice\n• Classic Format\n• Quick Print\n• Thermal Printer Ready";
            btnText.Size = new Size(180, 100);
            btnText.Location = new Point(260, 110);
            btnText.BackColor = Color.FromArgb(40, 167, 69);
            btnText.ForeColor = Color.White;
            btnText.FlatStyle = FlatStyle.Flat;
            btnText.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            btnText.TextAlign = ContentAlignment.MiddleCenter;
            btnText.FlatAppearance.BorderSize = 0;
            btnText.DialogResult = DialogResult.No;

            // Skip Button
            Button btnSkip = new Button();
            btnSkip.Text = "❌ Skip Invoice";
            btnSkip.Size = new Size(120, 35);
            btnSkip.Location = new Point(190, 230);
            btnSkip.BackColor = Color.FromArgb(108, 117, 125);
            btnSkip.ForeColor = Color.White;
            btnSkip.FlatStyle = FlatStyle.Flat;
            btnSkip.Font = new System.Drawing.Font("Segoe UI", 9);
            btnSkip.FlatAppearance.BorderSize = 0;
            btnSkip.DialogResult = DialogResult.Cancel;

            // Add hover effects
            btnPdf.MouseEnter += (s, e) => btnPdf.BackColor = Color.FromArgb(41, 128, 185);
            btnPdf.MouseLeave += (s, e) => btnPdf.BackColor = Color.FromArgb(52, 152, 219);

            btnText.MouseEnter += (s, e) => btnText.BackColor = Color.FromArgb(34, 139, 58);
            btnText.MouseLeave += (s, e) => btnText.BackColor = Color.FromArgb(40, 167, 69);

            invoiceDialog.Controls.Add(lblTitle);
            invoiceDialog.Controls.Add(lblSubtitle);
            invoiceDialog.Controls.Add(btnPdf);
            invoiceDialog.Controls.Add(btnText);
            invoiceDialog.Controls.Add(btnSkip);

            return invoiceDialog.ShowDialog();
        }

        // Utility method to add company information (you can customize this)
        private void AddCompanyInfo(PdfPTable table)
        {
            PdfPCell companyCell = new PdfPCell();
            companyCell.Border = Rectangle.NO_BORDER;
            companyCell.Padding = 10;

            // Add your company logo here if you have one
             iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("C:\\Users\\ASUS\\Documents\\Visual Studio CSharp 2022\\InventoryManagementSystem\\InventoryManagementSystem\\Resources\\logocolor.png");
             logo.ScaleToFit(80, 60);
             companyCell.AddElement(logo);

            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font companyFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);

            Paragraph companyName = new Paragraph("SK TIRES", companyFont);
            Paragraph address = new Paragraph("Your Business Address\nCity, State, ZIP\nPhone: +880-XXXXXXXX\nEmail: info@sktires.com",
                new iTextSharp.text.Font(baseFont, 9));

            companyCell.AddElement(companyName);
            companyCell.AddElement(address);

            table.AddCell(companyCell);
        }

        // Method for batch invoice generation (bonus feature)
        private void GenerateBatchInvoices(List<int> saleIds, bool isPdf = true)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "Select folder to save invoices";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderDialog.SelectedPath;
                    int successCount = 0;

                    foreach (int saleId in saleIds)
                    {
                        try
                        {
                            // Get sale information
                            var saleInfo = GetSaleInformation(saleId);
                            if (saleInfo != null)
                            {
                                string fileName = isPdf ?
                                    $"Invoice_{saleId}_{DateTime.Now:yyyyMMdd}.pdf" :
                                    $"Invoice_{saleId}_{DateTime.Now:yyyyMMdd}.txt";

                                string filePath = Path.Combine(folderPath, fileName);

                                if (isPdf)
                                {
                                    CreatePdfInvoice(filePath, saleId,
                                        saleInfo.CustomerId, saleInfo.CustomerName,
                                        saleInfo.TotalAmount, saleInfo.PaidAmount,
                                        saleInfo.DiscountValue, saleInfo.TransactionType);
                                }
                                else
                                {
                                    string textInvoice = GeneratePremiumTextInvoice(saleId,
                                        saleInfo.CustomerId, saleInfo.CustomerName,
                                        saleInfo.TotalAmount, saleInfo.PaidAmount,
                                        saleInfo.DiscountValue, saleInfo.TransactionType);

                                    System.IO.File.WriteAllText(filePath, textInvoice);
                                }

                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log individual failures but continue with batch
                            Console.WriteLine($"Failed to generate invoice for Sale ID {saleId}: {ex.Message}");
                        }
                    }

                    MessageBox.Show($"Successfully generated {successCount} out of {saleIds.Count} invoices!",
                        "Batch Generation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in batch generation: " + ex.Message);
            }
        }

        // Helper method to get sale information
        private dynamic GetSaleInformation(int saleId)
        {
            try
            {
                string query = @"
                SELECT 
                    s.SaleId, s.CustomerId, s.TransactionType, s.Discount, s.PayAmount, s.PaidAmount,
                    c.CustomerName
                FROM SaleTable s
                INNER JOIN CustomerTable c ON s.CustomerId = c.CustomerId
                WHERE s.SaleId = @SaleId";

                var parameters = new[] { new SqlParameter("@SaleId", saleId) };
                var ds = db.ExecuteQuery(query, parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    return new
                    {
                        SaleId = Convert.ToInt32(row["SaleId"]),
                        CustomerId = Convert.ToInt32(row["CustomerId"]),
                        CustomerName = row["CustomerName"].ToString(),
                        TransactionType = row["TransactionType"].ToString(),
                        DiscountValue = Convert.ToDecimal(row["Discount"]),
                        TotalAmount = Convert.ToDecimal(row["PayAmount"]),
                        PaidAmount = Convert.ToDecimal(row["PaidAmount"])
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
