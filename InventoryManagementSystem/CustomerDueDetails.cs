using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class CustomerDueDetails : Form
    {
        private DataAccess db;
        int selectedCustomerId = -1;
        public CustomerDueDetails()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateCustomerDueData();
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVCustomerDueDetails.RowTemplate.Height = 35; // Example height in pixels

            DGVCustomerDueDetails.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVCustomerDueDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVCustomerDueDetails.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVCustomerDueDetails.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }



        private void PopulateCustomerDueData(string query = @"
            SELECT 
                c.CustomerId,
                c.CustomerName,
                c.ContactInfo,
                c.CustomerEmail,
                cd.DueAmount,
                cd.DueDate
            FROM 
                CustomerTable c
            JOIN 
                CustomerDueTable cd ON c.CustomerId = cd.CustomerId
            WHERE 
                cd.DueAmount > 0;
           ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVCustomerDueDetails.AutoGenerateColumns = false;
                    this.DGVCustomerDueDetails.DataSource = ds.Tables[0];
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

        private void DGVCustomerDueDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int customerId = Convert.ToInt32(DGVCustomerDueDetails.Rows[e.RowIndex].Cells[0].Value);
            decimal dueAmount = Convert.ToDecimal(DGVCustomerDueDetails.Rows[e.RowIndex].Cells[5].Value);

            selectedCustomerId = customerId;
            txtCurrentDueSelectedCustomer.Text = dueAmount.ToString();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == -1)
            {
                MessageBox.Show("Please select a row..", "Selection Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtPaymentAmount.Text, out int payAmount) || payAmount <= 0)
            {
                MessageBox.Show("Please enter a valid amount greater than 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string updateQuery = @"UPDATE CustomerDueTable SET DueAmount = DueAmount - @DueAmount WHERE CustomerId = @CustomerId";

            var parameters = new[]
            {
                 new SqlParameter("@DueAmount", payAmount),
                 new SqlParameter("@CustomerId", selectedCustomerId)
            };

            try
            {
                int rowsAffected = db.ExecuteDMLQuery(updateQuery, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Payment Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateCustomerDueData();
                    txtPaymentAmount.Clear();
                    txtCurrentDueSelectedCustomer.Clear();
                }
                else
                {
                    MessageBox.Show("Payment Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }

        private void DGVCustomerDueDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVCustomerDueDetails.ClearSelection();
            DGVCustomerDueDetails.CurrentCell = null;
        }
    }
}
