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
    public partial class ForgetPasswordDialog : Form
    {

        private DataAccess db;
        public ForgetPasswordDialog()
        {
            InitializeComponent();
            db = new DataAccess(); // Initialize DataAccess class
        }

        private void submitButtonAction()
        {
            string userId = txtUserId.Text.Trim();
            string userName = txtUserName.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please enter User ID and Username.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Query to fetch password
                string sql = $"SELECT Password FROM UserTable WHERE UserId = '{userId}' AND UserName = '{userName}'";
                var ds = db.ExecuteQueryTable(sql);

                if (ds.Rows.Count > 0)
                {
                    string password = ds.Rows[0]["Password"].ToString();
                    MessageBox.Show($"Username: {userName}\nPassword: {password}", "Password Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("No matching records found.", "Password Retrieval Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtUserName.Clear();
            txtUserId.Clear();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            submitButtonAction();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
