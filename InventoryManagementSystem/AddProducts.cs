using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace InventoryManagementSystem
{
    public partial class AddProducts : Form
    {
        private DataAccess db;
        private int selectedProductId = -1; // Store the selected product ID for updating
        private decimal selectedProductPurchaseCost = 0;
        private bool isExistingProductSelected = false;

        public AddProducts()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateCategoryComboBox();
            PopulateBrandComboBox();
            PopulateSupplierComboBox();
            PopulateGridViewSaleInventory();
            txtPurchaseCost.TextChanged += txtPurchaseCost_TextChanged;
            txtQuantity1.TextChanged += txtQuantity1_TextChanged;
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVInventory.RowTemplate.Height = 35;
            DGVInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112);
            DGVInventory.DefaultCellStyle.SelectionForeColor = Color.White;
            DGVInventory.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            DGVInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray;
            DGVInventory.DefaultCellStyle.ForeColor = Color.White;
            DGVInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void PopulateGridViewSaleInventory(string filter = "")
        {
            try
            {
                string query = @"
                -- Inventory Display Query - Current Stock Status
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
                    p.ProductId DESC";

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += $" AND p.ProductName LIKE '{filter}%'";
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
                MessageBox.Show("Error loading inventory: " + ex.Message);
            }
        }

        private void PopulateCategoryComboBox()
        {
            try
            {
                string query = "SELECT CategoryId, CategoryName FROM CategoryTable";
                var ds = db.ExecuteQuery(query);
                cbCategoryName.DataSource = ds.Tables[0];
                cbCategoryName.DisplayMember = "CategoryName";
                cbCategoryName.ValueMember = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void PopulateBrandComboBox()
        {
            try
            {
                string query = "SELECT BrandId, BrandName FROM BrandTable";
                var ds = db.ExecuteQuery(query);
                cbBrandName.DataSource = ds.Tables[0];
                cbBrandName.DisplayMember = "BrandName";
                cbBrandName.ValueMember = "BrandId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading brands: " + ex.Message);
            }
        }

        private void PopulateSupplierComboBox()
        {
            try
            {
                string query = "SELECT SupplierID, SupplierName FROM SupplierTable";
                var ds = db.ExecuteQuery(query);
                cmbSupplierName.DataSource = ds.Tables[0];
                cmbSupplierName.DisplayMember = "SupplierName";
                cmbSupplierName.ValueMember = "SupplierID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Supplier: " + ex.Message);
            }
        }

        private int GetCategoryIdByName(string categoryName)
        {
            string query = "SELECT CategoryId FROM CategoryTable WHERE CategoryName = @CategoryName";
            SqlParameter parameter = new SqlParameter("@CategoryName", categoryName);
            object result = db.ExecuteScalarQuery(query, new SqlParameter[] { parameter });
            return result != null ? Convert.ToInt32(result) : -1;
        }

        private int GetBrandIdByName(string brandName)
        {
            string query = "SELECT BrandId FROM BrandTable WHERE BrandName = @BrandName";
            SqlParameter parameter = new SqlParameter("@BrandName", brandName);
            object result = db.ExecuteScalarQuery(query, new SqlParameter[] { parameter });
            return result != null ? Convert.ToInt32(result) : -1;
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                    Title = "Select an Image File"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFileName = openFileDialog.FileName;
                    pcbxProductBox.Image = Image.FromFile(selectedFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private byte[] GenerateBarcodeImage(string barcodeText)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 300
                }
            };
            using (Bitmap bitmap = barcodeWriter.Write(barcodeText))
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void ClearFields()
        {
            txtProductName.Clear();
            txtProductName.ReadOnly = false;
            cbCategoryName.SelectedIndex = -1;
            cbCategoryName.Enabled = true;
            cbBrandName.SelectedIndex = -1;
            cbBrandName.Enabled = true;
            txtSalePrice.Clear();
            txtSalePrice.ReadOnly = false;
            txtPurchaseCost.Clear();
            txtPurchaseCost.ReadOnly = false;
            pcbxProductBox.Image = null;
            pcbxBarcodeBox.Image = null;
            selectedProductId = -1;
            selectedProductPurchaseCost = 0;
            isExistingProductSelected = false;
            btnAddImage.Enabled = true;


            //Extra
            txtBarcodeInput.Clear();
        }

        private void ClearPurchaseFields()
        {
            txtQuantity1.Clear();
            txtTotalAmount.Clear();
            txtPaidAmount.Clear();
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string productName = txtProductName.Text.Trim();
                int categoryId = Convert.ToInt32(cbCategoryName.SelectedValue);
                int brandId = Convert.ToInt32(cbBrandName.SelectedValue);
                decimal salePrice = Convert.ToDecimal(txtSalePrice.Text);
                decimal purchaseCost = Convert.ToDecimal(txtPurchaseCost.Text);

                if (string.IsNullOrEmpty(productName) || salePrice < 0 || purchaseCost < 0)
                {
                    MessageBox.Show("Please fill all the fields with valid data.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Handle product image (can be null)
                byte[] productImageData = null;
                if (pcbxProductBox.Image != null)
                {
                    productImageData = ImageToByteArray(pcbxProductBox.Image);
                }

                // Build the SQL query based on whether we have an image or not
                string sql;
                SqlParameter[] parameters;

                if (productImageData != null)
                {
                    sql = @"UPDATE ProductTable 
                   SET ProductName = @ProductName, 
                       CategoryId = @CategoryId, 
                       BrandId = @BrandId, 
                       SalePrice = @SalePrice, 
                       PurchaseCost = @PurchaseCost, 
                       ProductImage = @ProductImage 
                   WHERE ProductId = @ProductId";

                    parameters = new SqlParameter[] {
                new SqlParameter("@ProductName", productName),
                new SqlParameter("@CategoryId", categoryId),
                new SqlParameter("@BrandId", brandId),
                new SqlParameter("@SalePrice", salePrice),
                new SqlParameter("@PurchaseCost", purchaseCost),
                new SqlParameter("@ProductImage", productImageData),
                new SqlParameter("@ProductId", selectedProductId)
            };
                }
                else
                {
                    sql = @"UPDATE ProductTable 
                   SET ProductName = @ProductName, 
                       CategoryId = @CategoryId, 
                       BrandId = @BrandId, 
                       SalePrice = @SalePrice, 
                       PurchaseCost = @PurchaseCost 
                   WHERE ProductId = @ProductId";

                    parameters = new SqlParameter[] {
                new SqlParameter("@ProductName", productName),
                new SqlParameter("@CategoryId", categoryId),
                new SqlParameter("@BrandId", brandId),
                new SqlParameter("@SalePrice", salePrice),
                new SqlParameter("@PurchaseCost", purchaseCost),
                new SqlParameter("@ProductId", selectedProductId)
            };
                }

                db.ExecuteQuery(sql, parameters);
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateGridViewSaleInventory();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));

        }

        private void btnAddPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Walk-In Supplier is selected (SupplierID = 1)
                if (cmbSupplierName.SelectedValue != null && Convert.ToInt32(cmbSupplierName.SelectedValue) == 1)
                {
                    // Validate that total amount equals paid amount for Walk-In Supplier
                    if (!decimal.TryParse(txtTotalAmount.Text, out decimal totalAmount) ||
                        !decimal.TryParse(txtPaidAmount.Text, out decimal paidAmount) ||
                        paidAmount != totalAmount)
                    {
                        MessageBox.Show("Walk-In Supplier must pay the full amount. No due allowed.",
                                      "Payment Restriction",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (IsNewProductPurchase())
                {
                    PurchaseNewProduct();
                }
                else
                {
                    PurchaseSelectedProduct();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing purchase: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsNewProductPurchase()
        {
            return (selectedProductId == -1) &&
                   !string.IsNullOrWhiteSpace(txtProductName.Text) &&
                   cbCategoryName.SelectedValue != null &&
                   cbBrandName.SelectedValue != null &&
                   !string.IsNullOrWhiteSpace(txtSalePrice.Text) &&
                   !string.IsNullOrWhiteSpace(txtPurchaseCost.Text);
        }

        private void PurchaseNewProduct()
        {
            try
            {
                if (!ValidateNewProductFields() || !ValidatePurchaseFields())
                    return;

                string productName = txtProductName.Text.Trim();
                int categoryId = Convert.ToInt32(cbCategoryName.SelectedValue);
                int brandId = Convert.ToInt32(cbBrandName.SelectedValue);
                decimal salePrice = Convert.ToDecimal(txtSalePrice.Text);
                decimal purchaseCost = Convert.ToDecimal(txtPurchaseCost.Text);
                bool generateBarcode = !cbDontGenerateBarcode.Checked;

                int supplierId = Convert.ToInt32(cmbSupplierName.SelectedValue);
                decimal quantity = Convert.ToDecimal(txtQuantity1.Text);
                decimal totalAmount = quantity * purchaseCost;
                decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);

                int productId = AddNewProduct(productName, categoryId, brandId, salePrice,
                                            purchaseCost, generateBarcode);

                if (productId != -1)
                {
                    int purchaseId = InsertPurchaseTableMethod(supplierId, totalAmount, paidAmount);

                    if (purchaseId != -1)
                    {
                        bool success = InsertPurchaseDetail(purchaseId, productId, quantity, purchaseCost);

                        if (success)
                        {
                            CreateSupplierDueIfNeeded(purchaseId, supplierId, totalAmount, paidAmount);

                            MessageBox.Show("Product added and purchased successfully!", "Success",
                                           MessageBoxButtons.OK, MessageBoxIcon.Information);

                            PopulateGridViewSaleInventory();
                            ClearAllFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in new product purchase: {ex.Message}");
            }
        }

        private void ClearAllFields()
        {
            ClearFields();
            ClearPurchaseFields();
            txtBarcodeInput.Clear();
        }

        private int AddNewProduct(string productName, int categoryId, int brandId,
                                 decimal salePrice, decimal purchaseCost, bool generateBarcode)
        {
            try
            {
                Image productImage = pcbxProductBox.Image;
                byte[] productImageData = ImageToByteArray(productImage) ?? new byte[0];

                string sql = @"INSERT INTO ProductTable (ProductName, CategoryId, BrandId, SalePrice, PurchaseCost, ProductImage) 
                               VALUES (@ProductName, @CategoryId, @BrandId, @SalePrice, @PurchaseCost, @ProductImage); 
                               SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@BrandId", brandId),
                    new SqlParameter("@SalePrice", salePrice),
                    new SqlParameter("@PurchaseCost", purchaseCost),
                    new SqlParameter("@ProductImage", productImageData)
                };

                object result = db.ExecuteScalarQuery(sql, parameters);

                if (result != null)
                {
                    int productId = Convert.ToInt32(result);

                    if (generateBarcode)
                    {
                        byte[] barcodeImageData = GenerateBarcodeImage(productId.ToString());
                        string updateSql = "UPDATE ProductTable SET BarcodeImage = @BarcodeImage WHERE ProductId = @ProductId";
                        SqlParameter[] updateParams = {
                            new SqlParameter("@BarcodeImage", barcodeImageData),
                            new SqlParameter("@ProductId", productId)
                        };

                        db.ExecuteQuery(updateSql, updateParams);
                    }

                    return productId;
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private bool InsertPurchaseDetail(int purchaseId, int productId, decimal quantity, decimal unitPrice)
        {
            try
            {
                string insertQuery = @"INSERT INTO PurchaseDetailTable (PurchaseId, ProductId, Quantity, UnitPrice) 
                                      VALUES (@PurchaseId, @ProductId, @Quantity, @UnitPrice)";

                SqlParameter[] insertParams = {
                    new SqlParameter("@PurchaseId", purchaseId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@UnitPrice", unitPrice)
                };

                db.ExecuteNonQuery(insertQuery, insertParams);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting purchase detail: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CreateSupplierDueIfNeeded(int purchaseId, int supplierId, decimal totalAmount, decimal paidAmount)
        {
            try
            {
                decimal dueAmount = totalAmount - paidAmount;

                if (dueAmount > 0)
                {
                    string dueQuery = @"INSERT INTO SupplierDueTable (PurchaseId, SupplierId, DueAmount) 
                                       VALUES (@PurchaseId, @SupplierId, @DueAmount)";

                    SqlParameter[] dueParams = {
                        new SqlParameter("@PurchaseId", purchaseId),
                        new SqlParameter("@SupplierId", supplierId),
                        new SqlParameter("@DueAmount", dueAmount)
                    };

                    db.ExecuteNonQuery(dueQuery, dueParams);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating supplier due: {ex.Message}");
            }
        }

        private bool ValidateNewProductFields()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                cbCategoryName.SelectedValue == null ||
                cbBrandName.SelectedValue == null ||
                !decimal.TryParse(txtSalePrice.Text, out decimal salePrice) || salePrice < 0 ||
                !decimal.TryParse(txtPurchaseCost.Text, out decimal purchaseCost) || purchaseCost < 0)
            {
                MessageBox.Show("Please fill all product fields with valid data.", "Input Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidatePurchaseFields()
        {
            if (cmbSupplierName.SelectedValue == null ||
                !decimal.TryParse(txtQuantity1.Text, out decimal quantity) || quantity <= 0 ||
                !decimal.TryParse(txtPaidAmount.Text, out decimal paidAmount) || paidAmount < 0)
            {
                MessageBox.Show("Please fill all purchase fields with valid data.", "Input Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private int InsertPurchaseTableMethod(int supplierId, decimal totalAmount, decimal paidAmount)
        {
            try
            {
                string purchaseQuery = @"INSERT INTO PurchaseTable (SupplierId, TotalAmount, PaidAmount) 
                                        VALUES (@SupplierId, @TotalAmount, @PaidAmount); 
                                        SELECT SCOPE_IDENTITY();";
                SqlParameter[] purchaseParams = new SqlParameter[]
                {
                    new SqlParameter("@SupplierId", supplierId),
                    new SqlParameter("@TotalAmount", totalAmount),
                    new SqlParameter("@PaidAmount", paidAmount)
                };

                object result = db.ExecuteScalarQuery(purchaseQuery, purchaseParams);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting into PurchaseTable: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private async void DGVInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVInventory.Rows[e.RowIndex];
                selectedProductId = Convert.ToInt32(row.Cells["ProductId"].Value);
                selectedProductPurchaseCost = Convert.ToDecimal(row.Cells["PurchaseCost"].Value);
                isExistingProductSelected = true;

                // Set the product fields
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtPurchaseCost.Text = row.Cells["PurchaseCost"].Value.ToString();
                txtSalePrice.Text = row.Cells["SalePrice"].Value.ToString();

                // Set category and brand by value
                cbCategoryName.SelectedValue = GetCategoryIdByName(row.Cells["CategoryName"].Value.ToString());
                cbBrandName.SelectedValue = GetBrandIdByName(row.Cells["BrandName"].Value.ToString());
                await FetchProductDataAsync(selectedProductId);
                // Enable all fields for editing during purchase
                txtProductName.ReadOnly = false;
                cbCategoryName.Enabled = true;
                cbBrandName.Enabled = true;
                txtSalePrice.ReadOnly = false;
                txtPurchaseCost.ReadOnly = false;
                btnAddImage.Enabled = true;
            }
        }



        private async Task FetchProductDataAsync(int productId)
        {
            try
            {
                string query = "SELECT p.ProductId, p.ProductName, p.SalePrice, p.PurchaseCost, " +
                               "c.CategoryId, b.BrandId, p.ProductImage, p.BarcodeImage " +
                               "FROM ProductTable p " +
                               "INNER JOIN CategoryTable c ON p.CategoryId = c.CategoryId " +
                               "INNER JOIN BrandTable b ON p.BrandId = b.BrandId " +
                               "WHERE p.ProductId = @ProductId";

                SqlParameter parameter = new SqlParameter("@ProductId", productId);
                var ds = await Task.Run(() => db.ExecuteQuery(query, new SqlParameter[] { parameter }));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    // Populate the fields with the selected row data
                    txtProductName.Text = dr["ProductName"].ToString();
                    cbCategoryName.SelectedValue = dr["CategoryId"];
                    cbBrandName.SelectedValue = dr["BrandId"];
                    txtSalePrice.Text = dr["SalePrice"].ToString();
                    txtPurchaseCost.Text = dr["PurchaseCost"].ToString();

                    // Load and display the product image if pcbxProductBox is not null
                    if (pcbxProductBox != null)
                    {
                        if (dr["ProductImage"] != DBNull.Value)
                        {
                            byte[] productImageBytes = (byte[])dr["ProductImage"];
                            pcbxProductBox.Image = ByteArrayToImage(productImageBytes) ?? null; // Use null if conversion fails
                        }
                        else
                        {
                            pcbxProductBox.Image = null; // Clear the image if no product image found
                        }
                    }

                    // Load and display the barcode image if pcbxBarcodeBox is not null
                    if (pcbxBarcodeBox != null)
                    {
                        if (dr["BarcodeImage"] != DBNull.Value)
                        {
                            byte[] barcodeImageBytes = (byte[])dr["BarcodeImage"];
                            pcbxBarcodeBox.Image = ByteArrayToImage(barcodeImageBytes) ?? null; // Use null if conversion fails
                        }
                        else
                        {
                            pcbxBarcodeBox.Image = null; // Clear the image if no barcode image found
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching product data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0)
                return null;

            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Invalid image data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }





        private void PurchaseSelectedProduct()
        {
            try
            {
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Please select a product from inventory.", "Warning",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidatePurchaseFields())
                    return;

                // Update product details if they were modified
                if (isExistingProductSelected)
                {
                    string productName = txtProductName.Text.Trim();
                    int categoryId = Convert.ToInt32(cbCategoryName.SelectedValue);
                    int brandId = Convert.ToInt32(cbBrandName.SelectedValue);
                    decimal salePrice = Convert.ToDecimal(txtSalePrice.Text);
                    decimal purchaseCost = Convert.ToDecimal(txtPurchaseCost.Text);

                    string updateSql = @"UPDATE ProductTable 
                                       SET ProductName = @ProductName, 
                                           CategoryId = @CategoryId, 
                                           BrandId = @BrandId, 
                                           SalePrice = @SalePrice, 
                                           PurchaseCost = @PurchaseCost 
                                       WHERE ProductId = @ProductId";

                    SqlParameter[] updateParams = {
                        new SqlParameter("@ProductName", productName),
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@BrandId", brandId),
                        new SqlParameter("@SalePrice", salePrice),
                        new SqlParameter("@PurchaseCost", purchaseCost),
                        new SqlParameter("@ProductId", selectedProductId)
                    };

                    db.ExecuteQuery(updateSql, updateParams);
                }

                // Process the purchase
                int supplierId = Convert.ToInt32(cmbSupplierName.SelectedValue);
                decimal quantity = Convert.ToDecimal(txtQuantity1.Text);
                decimal unitPrice = Convert.ToDecimal(txtPurchaseCost.Text);
                decimal totalAmount = quantity * unitPrice;
                decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);

                int purchaseId = InsertPurchaseTableMethod(supplierId, totalAmount, paidAmount);

                if (purchaseId != -1)
                {
                    bool success = InsertPurchaseDetail(purchaseId, selectedProductId, quantity, unitPrice);

                    if (success)
                    {
                        CreateSupplierDueIfNeeded(purchaseId, supplierId, totalAmount, paidAmount);

                        MessageBox.Show("Purchase added successfully!", "Success",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PopulateGridViewSaleInventory();
                        ClearAllFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in selected product purchase: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtQuantity1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }

        private void txtPurchaseCost_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            if (decimal.TryParse(txtQuantity1.Text, out decimal quantity) &&
                decimal.TryParse(txtPurchaseCost.Text, out decimal purchaseCost))
            {
                decimal totalAmount = purchaseCost * quantity;
                txtTotalAmount.Text = totalAmount.ToString("F2");
            }
            else
            {
                txtTotalAmount.Text = "0.00";
            }
        }

        private void pcbxOpenProductSettings2_Click(object sender, EventArgs e)
        {
            ProductSettings productSettings = new ProductSettings();
            productSettings.Show();
        }

        private void pcbxOpenSupplier_Click(object sender, EventArgs e)
        {
            AddSupplier addSupplier = new AddSupplier();
            addSupplier.Show();
        }

        private void btnAddPersonalBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string customBarcode = txtBarcodeInput.Text.Trim();
                if (int.TryParse(customBarcode, out int barcodeInt))
                {
                    if (barcodeInt <= 99999)
                    {
                        MessageBox.Show("The barcode value must be greater than 99999.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!IsBarcodeUnique(customBarcode))
                    {
                        MessageBox.Show("The custom barcode is already in use. Please choose a different barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                byte[] customBarcodeImage = GenerateBarcodeImage(customBarcode);
                string sql = "UPDATE ProductTable SET BarcodeImage = @BarcodeImage WHERE ProductId = @ProductId";
                SqlParameter[] parameters = {
                    new SqlParameter("@BarcodeImage", customBarcodeImage),
                    new SqlParameter("@Barcode", customBarcode),
                    new SqlParameter("@ProductId", selectedProductId)
                };

                db.ExecuteQuery(sql, parameters);
                MessageBox.Show("Custom barcode added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                PopulateGridViewSaleInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding custom barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsBarcodeUnique(string barcode)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM ProductTable WHERE ProductId = @ProductId";
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", barcode)
                };

                object result = db.ExecuteScalarQuery(query, parameters);
                int count = Convert.ToInt32(result);
                return count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking barcode uniqueness: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Please select a product to delete.", "Warning",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion with user
                var confirmResult = MessageBox.Show("Are you sure you want to delete this product and all its purchase records?",
                                                 "Confirm Delete",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }

                // First check if there are any sales for this product
                string checkSalesSql = "SELECT COUNT(*) FROM SaleDetailTable WHERE ProductId = @ProductId";
                int salesCount = Convert.ToInt32(db.ExecuteScalarQuery(checkSalesSql,
                    new SqlParameter[] { new SqlParameter("@ProductId", selectedProductId) }));

                if (salesCount > 0)
                {
                    MessageBox.Show("Cannot delete product because it has existing sales records.",
                                  "Deletion Restricted",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                    return;
                }

                // Begin transaction
                using (SqlTransaction transaction = db.BeginTransaction())
                {
                    try
                    {
                        // Delete from SupplierDueTable first
                        string deleteSupplierDuesSql = @"DELETE FROM SupplierDueTable 
                                              WHERE PurchaseId IN (
                                                  SELECT PurchaseId FROM PurchaseDetailTable 
                                                  WHERE ProductId = @ProductId
                                              )";
                        db.ExecuteNonQuery(deleteSupplierDuesSql, transaction,
                            new SqlParameter[] { new SqlParameter("@ProductId", selectedProductId) });

                        // Delete from PurchaseDetailTable
                        string deletePurchaseDetailsSql = "DELETE FROM PurchaseDetailTable WHERE ProductId = @ProductId";
                        db.ExecuteNonQuery(deletePurchaseDetailsSql, transaction,
                            new SqlParameter[] { new SqlParameter("@ProductId", selectedProductId) });

                        // Delete from PurchaseTable
                        string deletePurchasesSql = @"DELETE FROM PurchaseTable 
                                           WHERE PurchaseId IN (
                                               SELECT PurchaseId FROM PurchaseDetailTable 
                                               WHERE ProductId = @ProductId
                                           )";
                        db.ExecuteNonQuery(deletePurchasesSql, transaction,
                            new SqlParameter[] { new SqlParameter("@ProductId", selectedProductId) });

                        // Finally delete the product
                        string deleteProductSql = "DELETE FROM ProductTable WHERE ProductId = @ProductId";
                        db.ExecuteNonQuery(deleteProductSql, transaction,
                            new SqlParameter[] { new SqlParameter("@ProductId", selectedProductId) });

                        transaction.Commit();

                        MessageBox.Show("Product and all related purchase records deleted successfully!",
                                      "Success",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        // Refresh UI
                        PopulateGridViewSaleInventory();
                        ClearAllFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error deleting product: {ex.Message}",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void btnRelode_Click(object sender, EventArgs e)
        {
            PopulateCategoryComboBox();
            PopulateBrandComboBox();
            PopulateSupplierComboBox();
            PopulateGridViewSaleInventory(); 
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(SalesmanMainDashBoard));
        }

        private void pcbxRelode_Click(object sender, EventArgs e)
        {
            PopulateSupplierComboBox();
        }

        private void pcbxRelodeSupplier_Click(object sender, EventArgs e)
        {
            PopulateSupplierComboBox();
        }
    }
}