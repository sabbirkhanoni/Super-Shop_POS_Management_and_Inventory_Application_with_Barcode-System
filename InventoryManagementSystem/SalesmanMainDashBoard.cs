using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class SalesmanMainDashBoard : Form
    {
        public static string SessionNameForGoInventory { get; set; }

        private DataAccess db;

        private DataTable cartTable;
        private bool chkbDiscountClick = false;
        private bool walkInCustomer = false;

        public SalesmanMainDashBoard()
        {
            InitializeComponent();
            db = new DataAccess();

            InitializeCartTable();
            PopulateGridViewSaleInventory();
            PopulateCustomerComboBox();

            ExtraDesign();

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
        }

        private void InitializeCartTable()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductId", typeof(int));
            cartTable.Columns.Add("ProductName", typeof(string));
            cartTable.Columns.Add("SalePrice", typeof(decimal));
            cartTable.Columns.Add("SaleQuantity", typeof(int));
            cartTable.Columns.Add("SaleValue", typeof(decimal));

           
            DGVSaleCart.DataSource = cartTable;

        }

        private void PopulateGridViewSaleInventory(string query = @"
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
                COALESCE(pd.Quantity, 0) > 0
            GROUP BY 
                p.ProductId,
                p.ProductName,
                c.CategoryName,
                b.BrandName,
                p.SalePrice,
                p.PurchaseCost;
           ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
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


        private void AddProductToCart(int productId, string productName, decimal salePrice)
        {
            var existingRow = cartTable.AsEnumerable().FirstOrDefault(row => row.Field<int>("ProductId") == productId);

            if (existingRow != null)
            {
                existingRow.SetField("SaleQuantity", existingRow.Field<int>("SaleQuantity") + 1);
                existingRow.SetField("SaleValue", existingRow.Field<decimal>("SalePrice") * existingRow.Field<int>("SaleQuantity"));
            }
            else
            {
                int quantity = 1;
                decimal saleValue = salePrice * quantity;
                cartTable.Rows.Add(productId, productName, salePrice, quantity, saleValue);
            }

            UpdateCartTotal();
        }


        private void DGVSaleCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGVSaleCart.Columns[e.ColumnIndex].Name == "SalePriceCart" || DGVSaleCart.Columns[e.ColumnIndex].Name == "Quantity")
            {
                var row = cartTable.Rows[e.RowIndex];
                decimal salePrice = Convert.ToDecimal(row["SalePrice"]);
                int quantity = Convert.ToInt32(row["SaleQuantity"]);
                row["SaleValue"] = salePrice * quantity;

                // Update the total label
                UpdateCartTotal();
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

            lblTotal.Text = total.ToString("F2"); // Format to 2 decimal places
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
            if (string.IsNullOrEmpty(lblTotal.Text) || !IsTextAnDecimal(lblTotal.Text))
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
            decimal lblTotalValue = Convert.ToDecimal(lblTotal.Text.Trim());

            if (chkbDiscountClick)
            {
                if (txtPaymentValue < lblTotalValue)
                {
                    discountValue = lblTotalValue - txtPaymentValue;
                }
            }

            if (cmbCustomerName.Text == "Walk-In-Customer" && chkbDiscountClick==false)
            {

                if (txtPaymentValue < lblTotalValue)
                {
                    MessageBox.Show("Walk-In-Customer Must Have To Pay Full Payment Or Give Discount");
                    walkInCustomer = false;
                    return;
                }
            }

            var customerId = (int)cmbCustomerName.SelectedValue;
            var payAmount = cartTable.AsEnumerable().Sum(row => row.Field<decimal>("SaleValue"));
            var paidAmount = Convert.ToDecimal(txtPayment.Text.Trim());
            var dueAmount = payAmount - paidAmount;
            string transactionType = "";

            if (cmbTransactionType.SelectedItem != null)
            {
                transactionType = cmbTransactionType.SelectedItem.ToString();
            }
            else
            {
                transactionType = "CASH";
            }


            lblChange.Text = (paidAmount - payAmount).ToString();

            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var saleId = InsertSaleRecord(transaction, customerId, transactionType, discountValue, payAmount, paidAmount);

                    foreach (DataRow row in cartTable.Rows)
                    {
                        InsertSaleDetailRecord(transaction, saleId, row);
                        UpdateProductStock(transaction, row);
                    }

                    if (dueAmount > 0)
                    {
                        InsertOrUpdateCustomerDueRecord(transaction, customerId, saleId, dueAmount);
                    }

                    transaction.Commit();
                    MessageBox.Show("Checkout successful!");

                    ClearCart();
                    PopulateGridViewSaleInventory();
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

        private void UpdateProductStock(SqlTransaction transaction, DataRow row)
        {
            var query = @"
                UPDATE PurchaseDetailTable
                SET Quantity = Quantity - @SaleQuantity
                WHERE ProductId = @ProductId";

            var parameters = new[]
            {
                new SqlParameter("@SaleQuantity", row["SaleQuantity"]),
                new SqlParameter("@ProductId", row["ProductId"])
            };

            //MessageBox.Show("hello111");
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
            if (e.ColumnIndex == DGVSaleCart.Columns["Delete"].Index)
            {
                RemoveProductFromCart(e.RowIndex);
            }
        }

        private void RemoveProductFromCart(int rowIndex)
        {
            var row = cartTable.Rows[rowIndex];

            if (row.Field<int>("SaleQuantity") > 1)
            {
                row.SetField("SaleQuantity", row.Field<int>("SaleQuantity") - 1);
                row.SetField("SaleValue", row.Field<decimal>("SalePrice") * row.Field<int>("SaleQuantity"));
            }
            else
            {
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
            var query = $@"
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
            (p.ProductName LIKE '{txtSearchBar.Text}%' OR p.ProductId LIKE '{txtSearchBar.Text}%')
        GROUP BY 
            p.ProductId,
            p.ProductName,
            c.CategoryName,
            b.BrandName,
            p.SalePrice,
            p.PurchaseCost";

            PopulateGridViewSaleInventory(query);
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
            SalesmanInventoryStore salesmanInventoryStore = new SalesmanInventoryStore();
            salesmanInventoryStore.Show();
            this.Hide();
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
            this.Show();
        }

        private void DGVSaleCart_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleCart.ClearSelection();
            DGVSaleCart.CurrentCell = null;
        }

        private void DGVSaleInventory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleInventory.ClearSelection();
            DGVSaleInventory.CurrentCell = null;
        }
    }
}
