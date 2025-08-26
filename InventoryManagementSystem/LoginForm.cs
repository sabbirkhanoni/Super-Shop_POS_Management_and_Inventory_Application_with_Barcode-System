using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class LoginForm : Form
    {
        private DataAccess db;
        public static string SessionNameForAdmin { get; set; }
        public static string SessionNameForSalesman { get; set; }

        public static string SessionName { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            db = new DataAccess(); // Initialize DataAccess class
        }

       
        private void btnLogin_Click(object sender, EventArgs e)
        {
            loginButtonAction();
        }

        private void loginButtonAction()
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text;

            // Validate username and password
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and Password fields must be filled.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Query to fetch role based on username and password
                string sql = $"SELECT Role FROM UserTable WHERE UserName = '{username}' AND Password = '{password}'";
                var ds = this.db.ExecuteQueryTable(sql);

                if (ds.Rows.Count > 0)
                {
                    string usertype = ds.Rows[0]["Role"].ToString();

                    if (usertype == "Admin")
                    {
                        SessionNameForAdmin = usertype;
                        SessionName = username;
                        FormManager.OpenForm(this, typeof(AdminMainDashBoard));
                    }
                    else if (usertype == "Salesman")
                    {
                        SessionNameForSalesman = usertype;
                        SessionName = username;
                        FormManager.OpenForm(this, typeof(AdminMainDashBoard));
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during login: " + ex.Message);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close application when Cancel button is clicked
        }

        private void ShowPass_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            // Hide the password visibility image
            ShowPass.Visible = false;
            hidePass.Visible = true;
        }

        private void hidePass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            ShowPass.Visible = true;
            hidePass.Visible = false;
        }

        private void forgetPass_Click(object sender, EventArgs e)
        {
            ForgetPasswordDialog forgetPasswordDialog = new ForgetPasswordDialog();
            forgetPasswordDialog.Show();
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.LightGray;
            btnLogin.ForeColor = Color.Black;
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(0, 102, 204);
            btnLogin.ForeColor = Color.White;
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.LightGray;
            btnCancel.ForeColor = Color.Black;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(0, 102, 204);
            btnCancel.ForeColor = Color.White;
        }

        private void forgetPass_MouseEnter(object sender, EventArgs e)
        {
            forgetPass.ForeColor = Color.Black;
        }

        private void forgetPass_MouseLeave(object sender, EventArgs e)
        {
            forgetPass.ForeColor = Color.FromArgb(0, 102, 204);
        }

        private void pnlLeft_MouseEnter(object sender, EventArgs e)
        {
            pnlLeft.BackColor = Color.White;
            pnlRight.BackColor = Color.FromArgb(0, 102, 204);
            forgetPass.BackColor = Color.FromArgb(0,102, 204);
            forgetPass.ForeColor = Color.White;
            lblProjectName.ForeColor = Color.Black;
            lblVersion.ForeColor = Color.Black;
            btnBackUp.BackColor = Color.LightGray;
            btnBackUp.ForeColor = Color.Black;
            btnImport.ForeColor = Color.Black;
            btnImport.BackColor = Color.LightGray;
        }

        private void pnlLeft_MouseLeave(object sender, EventArgs e)
        {
            pnlRight.BackColor = Color.White;
            pnlLeft.BackColor = Color.FromArgb(0, 102, 204);
            forgetPass.BackColor = Color.White;
            forgetPass.ForeColor = Color.FromArgb(0, 102, 204);
            lblProjectName.ForeColor = Color.White;
            lblVersion.ForeColor = Color.White;
            btnBackUp.BackColor = Color.FromArgb(0, 102, 204);
            btnBackUp.ForeColor = Color.White;
            btnImport.BackColor = Color.FromArgb(0, 102, 204);
            btnImport.ForeColor = Color.White;

        }
    }
}
