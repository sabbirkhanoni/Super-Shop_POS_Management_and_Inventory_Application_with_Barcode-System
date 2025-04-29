using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class AddSupplier : Form
    {
        private DataAccess db;

        public AddSupplier()
        {
            InitializeComponent();
            db = new DataAccess(); // Initialize DataAccess class
            PopulateDataGridView();

            ExtraDesign();


        }

        public void ExtraDesign()
        {
            DGVSupplier.RowTemplate.Height = 35; // Example height in pixels

            DGVSupplier.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVSupplier.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVSupplier.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVSupplier.DefaultCellStyle.ForeColor = Color.White;
        }

        private void PopulateDataGridView()
        {
            try
            {
                string query = "SELECT SupplierId, SupplierName, ContactInfo, EmailAddress, AddressInfo FROM SupplierTable";
                var ds = db.ExecuteQuery(query);

                DGVSupplier.DataSource = ds.Tables[0];
                DGVSupplier.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message);
            }
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                string supplierName = txtSupplierName.Text.Trim();
                string contactInfo = txtSupplierContactNo.Text.Trim();
                string emailAddress = txtSupplierEmail.Text.Trim();
                string addressInfo = txtSupplierAddress.Text.Trim();

                if (string.IsNullOrEmpty(supplierName))
                {
                    MessageBox.Show("Supplier name is required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "INSERT INTO SupplierTable (SupplierName, ContactInfo, EmailAddress, AddressInfo) " +
                               "VALUES (@SupplierName, @ContactInfo, @EmailAddress, @AddressInfo)";

                SqlParameter[] parameters = {
                    new SqlParameter("@SupplierName", supplierName),
                    new SqlParameter("@ContactInfo", contactInfo),
                    new SqlParameter("@EmailAddress", emailAddress),
                    new SqlParameter("@AddressInfo", addressInfo)
                };

                int result = db.ExecuteDMLQuery(query, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Supplier added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to add supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVSupplier.CurrentRow == null)
                {
                    MessageBox.Show("Please select a supplier to update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int supplierId = Convert.ToInt32(DGVSupplier.CurrentRow.Cells["SupplierId"].Value);
                string supplierName = txtSupplierName.Text.Trim();
                string contactInfo = txtSupplierContactNo.Text.Trim();
                string emailAddress = txtSupplierEmail.Text.Trim();
                string addressInfo = txtSupplierAddress.Text.Trim();

                if (string.IsNullOrEmpty(supplierName))
                {
                    MessageBox.Show("Supplier name is required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "UPDATE SupplierTable SET SupplierName = @SupplierName, ContactInfo = @ContactInfo, " +
                               "EmailAddress = @EmailAddress, AddressInfo = @AddressInfo WHERE SupplierId = @SupplierId";

                SqlParameter[] parameters = {
                    new SqlParameter("@SupplierId", supplierId),
                    new SqlParameter("@SupplierName", supplierName),
                    new SqlParameter("@ContactInfo", contactInfo),
                    new SqlParameter("@EmailAddress", emailAddress),
                    new SqlParameter("@AddressInfo", addressInfo)
                };

                int result = db.ExecuteDMLQuery(query, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Supplier updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to update supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVSupplier.CurrentRow == null)
                {
                    MessageBox.Show("Please select a supplier to delete.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int supplierId = Convert.ToInt32(DGVSupplier.CurrentRow.Cells["SupplierId"].Value);

                string query = "DELETE FROM SupplierTable WHERE SupplierId = @SupplierId";

                SqlParameter parameter = new SqlParameter("@SupplierId", supplierId);

                int result = db.ExecuteDMLQuery(query, new SqlParameter[] { parameter });

                if (result > 0)
                {
                    MessageBox.Show("Supplier deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to delete supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtSupplierName.Text = "";
            txtSupplierContactNo.Text = "";
            txtSupplierEmail.Text = "";
            txtSupplierAddress.Text = "";
        }

        private void DGVSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVSupplier.Rows[e.RowIndex];
                txtSupplierName.Text = row.Cells["SupplierName"].Value.ToString();
                txtSupplierContactNo.Text = row.Cells["ContactInfo"].Value.ToString();
                txtSupplierEmail.Text = row.Cells["EmailAddress"].Value.ToString();
                txtSupplierAddress.Text = row.Cells["AddressInfo"].Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SalesmanInventoryStore store = new SalesmanInventoryStore();
            store.Show();
            this.Hide();
        }

        private void DGVSupplier_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSupplier.ClearSelection();
            DGVSupplier.CurrentCell = null;
        }
    }
}
