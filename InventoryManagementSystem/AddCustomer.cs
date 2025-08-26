using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class AddCustomer : Form
    {
        private DataAccess db;

        public AddCustomer()
        {
            InitializeComponent();
            db = new DataAccess(); // Initialize DataAccess class
            PopulateDataGridView();
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVCustomer.RowTemplate.Height = 35; // Example height in pixels

            DGVCustomer.DefaultCellStyle.SelectionBackColor = Color.FromArgb(25, 25, 112); // Gray color
            DGVCustomer.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Default row color
            DGVCustomer.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray; // Alternate row color
            DGVCustomer.DefaultCellStyle.ForeColor = Color.White;
        }

        private void PopulateDataGridView()
        {
            try
            {
                string query = "SELECT CustomerId, CustomerName, ContactInfo, CustomerEmail, CustomerAddress FROM CustomerTable";
                var ds = db.ExecuteQuery(query);

                DGVCustomer.DataSource = ds.Tables[0];
                DGVCustomer.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Customers: " + ex.Message);
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtCustomerName.Text.Trim();
                string contactInfo = txtCustomerContactNo.Text.Trim();
                string customerEmail = txtCustomerEmail.Text.Trim();
                string customerAddress = txtCustomerAddress.Text.Trim();

                if (string.IsNullOrEmpty(customerName))
                {
                    MessageBox.Show("Customer name is required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "INSERT INTO CustomerTable (CustomerName, ContactInfo, CustomerEmail, CustomerAddress) " +
                               "VALUES (@CustomerName, @ContactInfo, @CustomerEmail, @CustomerAddress)";

                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerName", customerName),
                    new SqlParameter("@ContactInfo", contactInfo),
                    new SqlParameter("@CustomerEmail", customerEmail),
                    new SqlParameter("@CustomerAddress", customerAddress)
                };

                int result = db.ExecuteDMLQuery(query, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to add Customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding Customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVCustomer.CurrentRow == null)
                {
                    MessageBox.Show("Please select a Customer to update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int customerId = Convert.ToInt32(DGVCustomer.CurrentRow.Cells["CustomerId"].Value);
                string customerName = txtCustomerName.Text.Trim();
                string contactInfo = txtCustomerContactNo.Text.Trim();
                string customerEmail = txtCustomerEmail.Text.Trim();
                string customerAddress = txtCustomerAddress.Text.Trim();

                if (string.IsNullOrEmpty(customerName))
                {
                    MessageBox.Show("Customer name is required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "UPDATE CustomerTable SET CustomerName = @CustomerName, ContactInfo = @ContactInfo, " +
                               "CustomerEmail = @CustomerEmail, CustomerAddress = @CustomerAddress WHERE CustomerId = @CustomerId";

                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerId", customerId),
                    new SqlParameter("@CustomerName", customerName),
                    new SqlParameter("@ContactInfo", contactInfo),
                    new SqlParameter("@CustomerEmail", customerEmail),
                    new SqlParameter("@CustomerAddress", customerAddress)
                };

                int result = db.ExecuteDMLQuery(query, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to update Customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVCustomer.CurrentRow == null)
                {
                    MessageBox.Show("Please select a Customer to delete.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int customerId = Convert.ToInt32(DGVCustomer.CurrentRow.Cells["CustomerId"].Value);

                string query = "DELETE FROM CustomerTable WHERE CustomerId = @CustomerId";

                SqlParameter parameter = new SqlParameter("@CustomerId", customerId);

                int result = db.ExecuteDMLQuery(query, new SqlParameter[] { parameter });

                if (result > 0)
                {
                    MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to delete Customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting Customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtCustomerName.Text = "";
            txtCustomerContactNo.Text = "";
            txtCustomerEmail.Text = "";
            txtCustomerAddress.Text = "";
        }

        private void DGVCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVCustomer.Rows[e.RowIndex];
                txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
                txtCustomerContactNo.Text = row.Cells["ContactInfo"].Value.ToString();
                txtCustomerEmail.Text = row.Cells["CustomerEmail"].Value.ToString();
                txtCustomerAddress.Text = row.Cells["CustomerAddress"].Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }

        private void DGVCustomer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVCustomer.ClearSelection();
            DGVCustomer.CurrentCell = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
