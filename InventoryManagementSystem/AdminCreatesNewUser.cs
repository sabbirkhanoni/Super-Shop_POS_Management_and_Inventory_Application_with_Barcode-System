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
    public partial class AdminCreatesNewUser : Form
    {
        DataAccess db;
        public AdminCreatesNewUser()
        {
            InitializeComponent();
            db = new DataAccess();
            PopulateUserTableDataGirdView();
            ExtraDesign();
        }

        public void ExtraDesign()
        {
            DGVUser.RowTemplate.Height = 35; // Example height in pixels

            DGVUser.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVUser.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVUser.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVUser.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }

        private void PopulateUserTableDataGirdView(string query = @"SELECT * FROM UserTable;")
        {
            try
            {
                if(db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    DGVUser.AutoGenerateColumns = true;
                    DGVUser.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Database context is not initialized While Populate UserTable");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Database Error is Occered While Populate UserTable" + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminMainDashBoard adminMainDashBoard = new AdminMainDashBoard();
            adminMainDashBoard.Show();
            this.Hide();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddNewUser();
        }

        private void AddNewUser()
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string role = "Admin";
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please Select Role For User");
                return;
            }
            else
            {
                role = cmbRole.SelectedItem.ToString();
            }
            
            try
            {
                string query = $"INSERT INTO UserTable (UserName,Password,Role) VALUES ('{username}','{password}','{role}')";
                int rowAffected = db.ExecuteDMLQuery(query);
                if (rowAffected > 0)
                {
                    MessageBox.Show("New User Added Successfully");
                    txtPassword.Clear();
                    txtUserName.Clear();
                    cmbRole.SelectedItem = null;
                    PopulateUserTableDataGirdView();
                }
                else
                {
                    MessageBox.Show("Failed To Add New User", "Failed Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error While Add New User", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            UpdateUser();
        }

        private void UpdateUser()
        {
            int userId = Convert.ToInt32(DGVUser.CurrentRow.Cells["UserId"].Value);
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem.ToString();

            if(userId==1)
            {
                if(role == "Salesman")
                {
                    MessageBox.Show("You Can not Change Your Role","Validation",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
            }

            try
            {

                string query = $"UPDATE UserTable SET UserName = '{username}',Password = '{password}', Role = '{role}' WHERE UserId = {userId}";
                int rowAffected = db.ExecuteDMLQuery(query);
                if (rowAffected > 0)
                {
                    MessageBox.Show("User Details Updated Successfull");
                    txtUserName.Clear();
                    txtPassword.Clear();
                    cmbRole.SelectedItem = null;
                    PopulateUserTableDataGirdView();
                }
                else
                {
                    MessageBox.Show("Failed to update User.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error For Updating User. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DGVUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.txtUserName.Text = DGVUser.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                this.txtPassword.Text = DGVUser.Rows[e.RowIndex].Cells["Password"].Value.ToString();
                this.cmbRole.SelectedItem = DGVUser.Rows[e.RowIndex].Cells["Role"].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            cmbRole.SelectedItem = null;
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        private void DeleteUser()
        {
            int userId = Convert.ToInt32(DGVUser.CurrentRow.Cells["UserId"].Value);
            if (userId == 1 || userId == 2)
            {
                MessageBox.Show("UserId 1 and 2 You Can not Delete","Validation",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string query = $"DELETE FROM UserTable WHERE UserId = {userId}";
                int rowAffected = db.ExecuteDMLQuery(query);
                if (rowAffected > 0)
                {
                    MessageBox.Show("User Deleted Successfully");
                    txtUserName.Clear();
                    txtPassword.Clear();
                    cmbRole.SelectedItem = null;
                    PopulateUserTableDataGirdView();
                }
                else
                {
                    MessageBox.Show("Failed To Delete User", "Failed Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Datbase Error while Delete User","Database Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
