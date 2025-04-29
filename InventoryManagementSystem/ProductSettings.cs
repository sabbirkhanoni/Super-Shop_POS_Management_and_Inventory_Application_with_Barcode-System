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
    public partial class ProductSettings : Form
    {
        private DataAccess db;
        public ProductSettings()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateGridViewCategory();
            PopulateGridViewBrand();

            ExtraDesign();
        }




        public void ExtraDesign()
        {
            DGVCategory.RowTemplate.Height = 30; // Example height in pixels
            DGVBrand.RowTemplate.Height = 30; // Example height in pixels

            DGVCategory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVCategory.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVCategory.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVCategory.DefaultCellStyle.ForeColor = Color.White;

            DGVBrand.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVBrand.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVBrand.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVBrand.DefaultCellStyle.ForeColor = Color.White;

        }


        private void PopulateGridViewCategory(string filter = "")
        {
            try
            {
                string query = "SELECT * FROM CategoryTable";

                // Apply filter if provided
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += $" WHERE CategoryName LIKE '%{filter}%'";
                }

                var ds = db.ExecuteQuery(query);
                this.DGVCategory.AutoGenerateColumns = false;
                this.DGVCategory.DataSource = ds.Tables[0];

                // Deselect the initially selected row
                if (DGVCategory.Rows.Count > 0)
                {
                    DGVCategory.ClearSelection();
                    DGVCategory.CurrentCell = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the categories: " + ex.Message);
            }
        }


        private void PopulateGridViewBrand(string filter = "")
        {
            try
            {
                string query = "SELECT * FROM BrandTable";

                // Apply filter if provided
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += $" WHERE BrandName LIKE '%{filter}%'";
                }

                var ds = db.ExecuteQuery(query);
                this.DGVBrand.AutoGenerateColumns = false;
                this.DGVBrand.DataSource = ds.Tables[0];

                // Deselect the initially selected row
                if (DGVBrand.Rows.Count > 0)
                {
                    DGVBrand.ClearSelection();
                    DGVBrand.CurrentCell = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the brands: " + ex.Message);
            }
        }


        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category name cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Query to insert the category
                string sql = $"INSERT INTO CategoryTable (CategoryName) VALUES ('{categoryName}')";

                int rowsAffected = db.ExecuteDMLQuery(sql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCategoryName.Clear();
                    PopulateGridViewCategory();
                }
                else
                {
                    MessageBox.Show("Failed to add category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddBrand_Click(object sender, EventArgs e)
        {
            string brandName = txtBrandName.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(brandName))
            {
                MessageBox.Show("Brand name cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Query to insert the brand
                string sql = $"INSERT INTO BrandTable (BrandName) VALUES ('{brandName}')";

                int rowsAffected = db.ExecuteDMLQuery(sql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Brand added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBrandName.Clear();
                    PopulateGridViewBrand();
                }
                else
                {
                    MessageBox.Show("Failed to add brand.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding brand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category name cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int categoryId = Convert.ToInt32(DGVCategory.CurrentRow.Cells["CategoryId"].Value);
                string sql = $"UPDATE CategoryTable SET CategoryName = '{categoryName}' WHERE CategoryId = {categoryId}";
                int rowsAffected = db.ExecuteDMLQuery(sql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Category updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCategoryName.Clear();
                    PopulateGridViewCategory();
                }
                else
                {
                    MessageBox.Show("Failed to update category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateBrand_Click(object sender, EventArgs e)
        {
            string brandName = txtBrandName.Text.Trim();

            if (string.IsNullOrEmpty(brandName))
            {
                MessageBox.Show("Brand name cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int brandId = Convert.ToInt32(DGVBrand.CurrentRow.Cells["BrandId"].Value);
                string sql = $"UPDATE BrandTable SET BrandName = '{brandName}' WHERE BrandId = {brandId}";
                int rowsAffected = db.ExecuteDMLQuery(sql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Brand updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBrandName.Clear();
                    PopulateGridViewBrand();
                }
                else
                {
                    MessageBox.Show("Failed to update brand.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating brand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVCategory.CurrentRow == null)
                {
                    MessageBox.Show("Please select a row to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int categoryId = Convert.ToInt32(DGVCategory.CurrentRow.Cells["CategoryId"].Value);

                // Check for dependent records in ProductTable
                string checkSql = $"SELECT COUNT(*) FROM ProductTable WHERE CategoryId = {categoryId}";
                int dependentCount = Convert.ToInt32(db.ExecuteScalarQuery(checkSql));

                if (dependentCount > 0)
                {
                    MessageBox.Show("Cannot delete category because it is used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Proceed to delete the category
                string deleteSql = $"DELETE FROM CategoryTable WHERE CategoryId = {categoryId}";
                int rowsAffected = db.ExecuteDMLQuery(deleteSql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCategoryName.Clear();
                    PopulateGridViewCategory();
                }
                else
                {
                    MessageBox.Show("Failed to delete category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteBrand_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVBrand.CurrentRow == null)
                {
                    MessageBox.Show("Please select a row to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int brandId = Convert.ToInt32(DGVBrand.CurrentRow.Cells["BrandId"].Value);

                // Check for dependent records
                string checkSql = $"SELECT COUNT(*) FROM ProductTable WHERE BrandId = {brandId}";
                int dependentCount = Convert.ToInt32(db.ExecuteScalarQuery(checkSql));

                if (dependentCount > 0)
                {
                    MessageBox.Show("Cannot delete brand because it is used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Proceed to delete
                string deleteSql = $"DELETE FROM BrandTable WHERE BrandId = {brandId}";
                int rowsAffected = db.ExecuteDMLQuery(deleteSql);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Brand deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBrandName.Clear();
                    PopulateGridViewBrand();
                }
                else
                {
                    MessageBox.Show("Failed to delete brand.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting brand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DGVCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.txtCategoryName.Text = this.DGVCategory.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString();
            }
        }

        private void DGVBrand_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.txtBrandName.Text = this.DGVBrand.Rows[e.RowIndex].Cells["BrandName"].Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore salesmanInventoryStore = new SalesmanInventoryStore();
            salesmanInventoryStore.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtSearchCategory_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewCategory(txtSearchCategory.Text.Trim());
        }

        private void txtSearchBrand_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewBrand(txtSearchBrand.Text.Trim());
        }

        private void DGVCategory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVCategory.ClearSelection();
            DGVCategory.CurrentCell = null;
        }

        private void DGVBrand_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVBrand.ClearSelection();
            DGVBrand.CurrentCell = null;
        }
    }
}
