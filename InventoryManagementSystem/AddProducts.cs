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
        private decimal selectedProductIdpurchaseCost = 0;

        public AddProducts()
        {
            InitializeComponent();
            db = new DataAccess(); // Initialize DataAccess class
            PopulateDataGirdView();
            PopulateCategoryComboBox();
            PopulateBrandComboBox();
            PopulateSupplierComboBox();
            DGVProducts.CellClick += DGVProducts_CellClick; // Add event handler

            ExtraDesign();
        }


        public void ExtraDesign()
        {
            DGVProducts.RowTemplate.Height = 35; // Example height in pixels

            DGVProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVProducts.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVProducts.DefaultCellStyle.ForeColor = Color.White;
        }


        private void DGVInventory_SelectionChanged(object sender, EventArgs e)
        {
            DGVProducts.ClearSelection();
            DGVProducts.CurrentCell = null;
        }

        public class ProductDetail
        {
            public int ProductId { get; set; }
            public decimal Quantity { get; set; }
        }




        private void PopulateDataGirdView()
        {
            try
            {
                string query = "SELECT p.ProductId, p.ProductName, p.SalePrice, p.PurchaseCost, " +
                               "c.CategoryName, b.BrandName " +
                               "FROM ProductTable p " +
                               "INNER JOIN CategoryTable c ON p.CategoryId = c.CategoryId " +
                               "INNER JOIN BrandTable b ON p.BrandId = b.BrandId";

                var ds = db.ExecuteQuery(query);

                // Bind the data to the DataGridView
                DGVProducts.DataSource = ds.Tables[0];

                // Optionally, resize columns based on content
                DGVProducts.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
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

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string productName = txtProductName.Text.Trim();
                int categoryId = Convert.ToInt32(cbCategoryName.SelectedValue);
                int brandId = Convert.ToInt32(cbBrandName.SelectedValue);
                decimal salePrice = Convert.ToDecimal(txtSalePrice.Text);
                decimal purchaseCost = Convert.ToDecimal(txtPurchaseCost.Text);
                bool generateBarcode = !cbDontGenerateBarcode.Checked; // Determine if barcode should be generated

                if (string.IsNullOrEmpty(productName) || salePrice < 0 || purchaseCost < 0)
                {
                    MessageBox.Show("Please fill all the fields with valid data.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Image productImage = pcbxProductBox.Image;
                byte[] productImageData = ImageToByteArray(productImage) ?? new byte[0]; // Handle no image scenario

                string sql = "INSERT INTO ProductTable (ProductName, CategoryId, BrandId, SalePrice, PurchaseCost, ProductImage) " +
                             "VALUES (@ProductName, @CategoryId, @BrandId, @SalePrice, @PurchaseCost, @ProductImage); " +
                             "SELECT SCOPE_IDENTITY();";

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
                    byte[] barcodeImageData = null;

                    if (generateBarcode)
                    {
                        // Generate barcode using productId
                        string barcode = productId.ToString();
                        barcodeImageData = GenerateBarcodeImage(barcode);

                        // Update the product with barcode image
                        string updateSql = "UPDATE ProductTable SET BarcodeImage = @BarcodeImage WHERE ProductId = @ProductId";
                        SqlParameter[] updateParams = {
                    new SqlParameter("@BarcodeImage", barcodeImageData),
                    new SqlParameter("@ProductId", productId)
                };

                        db.ExecuteQuery(updateSql, updateParams);
                    }

                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGirdView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to add product. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void DGVProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int productId = Convert.ToInt32(DGVProducts.Rows[e.RowIndex].Cells["ProductId"].Value);
                int purchaseCost = Convert.ToInt32(DGVProducts.Rows[e.RowIndex].Cells["SalePrice"].Value);
                await FetchProductDataAsync(productId);
                selectedProductId = productId; // Store the selected product ID for updating
                selectedProductIdpurchaseCost = purchaseCost; // Store the selected product ID for updating
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



        private void ClearFields()
        {
            txtProductName.Clear();
            cbCategoryName.SelectedIndex = -1;
            cbBrandName.SelectedIndex = -1;

            txtSalePrice.Clear();
            txtPurchaseCost.Clear();
            pcbxProductBox.Image = null;
            pcbxBarcodeBox.Image = null;
            selectedProductId = -1; // Reset the selected product ID

            cmbSupplierName.SelectedIndex = -1;
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

                Image productImage = pcbxProductBox.Image;
                byte[] productImageData = ImageToByteArray(productImage);

                string sql = "UPDATE ProductTable " +
                             "SET ProductName = @ProductName, CategoryId = @CategoryId, BrandId = @BrandId, " +
                             "SalePrice = @SalePrice, PurchaseCost = @PurchaseCost, ProductImage = @ProductImage " +
                             "WHERE ProductId = @ProductId";

                SqlParameter[] parameters = {
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@BrandId", brandId),
                    new SqlParameter("@SalePrice", salePrice),
                    new SqlParameter("@PurchaseCost", purchaseCost),
                    new SqlParameter("@ProductImage", productImageData),
                    new SqlParameter("@ProductId", selectedProductId)
                };

                db.ExecuteQuery(sql, parameters);

                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateDataGirdView();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Please select a product to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = "DELETE FROM ProductTable WHERE ProductId = @ProductId";

                SqlParameter parameter = new SqlParameter("@ProductId", selectedProductId);

                db.ExecuteQuery(sql, new SqlParameter[] { parameter });

                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateDataGirdView();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore salesmanInventoryStore = new SalesmanInventoryStore();
            salesmanInventoryStore.Show();
            this.Hide();
        }

        private void btnAddPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                int supplierId = Convert.ToInt32(cmbSupplierName.SelectedValue);
                decimal totalAmount = Convert.ToDecimal(txtQuantity1.Text) * Convert.ToDecimal(txtPurchaseCost.Text);
                decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);

                if (supplierId <= 0 || totalAmount < 0 || paidAmount < 0)
                {
                    MessageBox.Show("Please fill all the fields with valid data.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert into PurchaseTable
                int purchaseId = InsertPurchaseTableMethod(supplierId, totalAmount, paidAmount);

                if (purchaseId != -1)
                {
                    // Insert into PurchaseDetailTable
                    InsertOrUpdatePurchaseDetailTable(purchaseId);

                    MessageBox.Show("Purchase added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to add purchase. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding purchase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void InsertOrUpdatePurchaseDetailTable(int purchaseId)
        {
            try
            {
                if (selectedProductId != -1)
                {
                    int productId = selectedProductId;
                    decimal quantity = Convert.ToDecimal(txtQuantity1.Text);
                    decimal unitPrice = Convert.ToDecimal(txtPurchaseCost.Text);

                    string checkQuery = @"SELECT COUNT(*) FROM PurchaseDetailTable 
                                  WHERE ProductId = @ProductId";
                    SqlParameter[] checkParams = new SqlParameter[]
                    {
                new SqlParameter("@ProductId", productId),
                    };

                    object result = db.ExecuteScalarQuery(checkQuery, checkParams);
                    int count = Convert.ToInt32(result);

                    if (count > 0)
                    {
                        // Product already exists in the PurchaseDetailTable for this purchase, update the quantity
                        string updateQuery = @"UPDATE PurchaseDetailTable 
                                       SET Quantity = Quantity + @Quantity, 
                                           UnitPrice = @UnitPrice 
                                       WHERE ProductId = @ProductId";
                        SqlParameter[] updateParams = new SqlParameter[]
                        {
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@UnitPrice", unitPrice),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@PurchaseId", purchaseId)
                        };

                        db.ExecuteNonQuery(updateQuery, updateParams);
                    }
                    else
                    {
                        // Product doesn't exist in the PurchaseDetailTable for this purchase, insert new record
                        string insertQuery = @"INSERT INTO PurchaseDetailTable (PurchaseId, ProductId, Quantity, UnitPrice)
                                       VALUES (@PurchaseId, @ProductId, @Quantity, @UnitPrice)";
                        SqlParameter[] insertParams = new SqlParameter[]
                        {
                    new SqlParameter("@PurchaseId", purchaseId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@UnitPrice", unitPrice)
                        };

                        db.ExecuteNonQuery(insertQuery, insertParams);
                    }
                }
                else
                {
                    MessageBox.Show($"Invalid data in row. Please ensure ProductId is filled correctly.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting or updating PurchaseDetailTable: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pcbxOpenProductSettings1_Click(object sender, EventArgs e)
        {
            ProductSettings productSettings = new ProductSettings();
            productSettings.Show();
            
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

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Event handler for quantity text change
        private void txtQuantity1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }

        // Calculate the total amount based on the purchase cost and entered quantity
        private void CalculateTotalAmount()
        {
            if (decimal.TryParse(txtQuantity1.Text, out decimal quantity))
            {
                decimal totalAmount = selectedProductIdpurchaseCost * quantity;
                txtTotalAmount.Text = totalAmount.ToString("F2"); // Format to 2 decimal places
            }
            else
            {
                txtTotalAmount.Text = "0.00";
            }
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

                // Validate if the barcode input is an integer and greater than 9999999
                if (int.TryParse(customBarcode, out int barcodeInt))
                {
                    if (barcodeInt <= 99999)
                    {
                        MessageBox.Show("The barcode value must be greater than 99999.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Check if custom barcode is unique for integer barcodes
                    if (!IsBarcodeUnique(customBarcode))
                    {
                        MessageBox.Show("The custom barcode is already in use. Please choose a different barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Generate barcode and update the product with the custom barcode image
                byte[] customBarcodeImage = GenerateBarcodeImage(customBarcode);
                string sql = "UPDATE ProductTable SET BarcodeImage = @BarcodeImage WHERE ProductId = @ProductId";
                SqlParameter[] parameters = {
            new SqlParameter("@BarcodeImage", customBarcodeImage),
            new SqlParameter("@Barcode", customBarcode),
            new SqlParameter("@ProductId", selectedProductId)
        };

                db.ExecuteQuery(sql, parameters);

                MessageBox.Show("Custom barcode added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateDataGirdView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding custom barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Method to check if a barcode is unique
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
                return count == 0; // Return true if no matching ProductId found
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking barcode uniqueness: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


    }
}
