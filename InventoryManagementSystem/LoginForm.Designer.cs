namespace InventoryManagementSystem
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.hidePass = new System.Windows.Forms.PictureBox();
            this.ShowPass = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.forgetPass = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblImgName = new System.Windows.Forms.Panel();
            this.pcbxLogo = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.btnBackUp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hidePass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShowPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.lblImgName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlRight);
            this.panel1.Controls.Add(this.pnlLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1532, 843);
            this.panel1.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.panel3);
            this.pnlRight.Location = new System.Drawing.Point(736, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(796, 843);
            this.pnlRight.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.hidePass);
            this.panel3.Controls.Add(this.ShowPass);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.forgetPass);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnLogin);
            this.panel3.Controls.Add(this.txtPassword);
            this.panel3.Controls.Add(this.txtUserName);
            this.panel3.Location = new System.Drawing.Point(155, 214);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(470, 478);
            this.panel3.TabIndex = 0;
            // 
            // hidePass
            // 
            this.hidePass.Image = global::InventoryManagementSystem.Properties.Resources.hide;
            this.hidePass.Location = new System.Drawing.Point(396, 196);
            this.hidePass.Name = "hidePass";
            this.hidePass.Size = new System.Drawing.Size(25, 35);
            this.hidePass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.hidePass.TabIndex = 6;
            this.hidePass.TabStop = false;
            this.hidePass.Click += new System.EventHandler(this.hidePass_Click);
            // 
            // ShowPass
            // 
            this.ShowPass.Image = global::InventoryManagementSystem.Properties.Resources.view;
            this.ShowPass.Location = new System.Drawing.Point(395, 196);
            this.ShowPass.Name = "ShowPass";
            this.ShowPass.Size = new System.Drawing.Size(25, 35);
            this.ShowPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ShowPass.TabIndex = 5;
            this.ShowPass.TabStop = false;
            this.ShowPass.Click += new System.EventHandler(this.ShowPass_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::InventoryManagementSystem.Properties.Resources.letter_i;
            this.pictureBox1.Location = new System.Drawing.Point(395, 111);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // forgetPass
            // 
            this.forgetPass.AutoSize = true;
            this.forgetPass.BackColor = System.Drawing.Color.White;
            this.forgetPass.Font = new System.Drawing.Font("Times New Roman", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgetPass.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.forgetPass.Location = new System.Drawing.Point(160, 360);
            this.forgetPass.Name = "forgetPass";
            this.forgetPass.Size = new System.Drawing.Size(173, 25);
            this.forgetPass.TabIndex = 2;
            this.forgetPass.Text = "Foget Password?";
            this.forgetPass.Click += new System.EventHandler(this.forgetPass_Click);
            this.forgetPass.MouseEnter += new System.EventHandler(this.forgetPass_MouseEnter);
            this.forgetPass.MouseLeave += new System.EventHandler(this.forgetPass_MouseLeave);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Location = new System.Drawing.Point(253, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 45);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.SystemColors.Control;
            this.btnLogin.Location = new System.Drawing.Point(87, 286);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(122, 45);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.MouseEnter += new System.EventHandler(this.btnLogin_MouseEnter);
            this.btnLogin.MouseLeave += new System.EventHandler(this.btnLogin_MouseLeave);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(78, 196);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(297, 38);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(78, 111);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(297, 39);
            this.txtUserName.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLeft.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pnlLeft.Controls.Add(this.btnImport);
            this.pnlLeft.Controls.Add(this.lblImgName);
            this.pnlLeft.Controls.Add(this.btnBackUp);
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(730, 843);
            this.pnlLeft.TabIndex = 2;
            this.pnlLeft.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.pnlLeft.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnImport.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.SystemColors.Control;
            this.btnImport.Location = new System.Drawing.Point(168, 786);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(122, 45);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.btnImport.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // lblImgName
            // 
            this.lblImgName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImgName.Controls.Add(this.pcbxLogo);
            this.lblImgName.Controls.Add(this.lblVersion);
            this.lblImgName.Controls.Add(this.lblProjectName);
            this.lblImgName.Location = new System.Drawing.Point(69, 281);
            this.lblImgName.Name = "lblImgName";
            this.lblImgName.Size = new System.Drawing.Size(542, 302);
            this.lblImgName.TabIndex = 4;
            this.lblImgName.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.lblImgName.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // pcbxLogo
            // 
            this.pcbxLogo.Image = global::InventoryManagementSystem.Properties.Resources.logocolor;
            this.pcbxLogo.Location = new System.Drawing.Point(149, 18);
            this.pcbxLogo.Name = "pcbxLogo";
            this.pcbxLogo.Size = new System.Drawing.Size(248, 183);
            this.pcbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbxLogo.TabIndex = 3;
            this.pcbxLogo.TabStop = false;
            this.pcbxLogo.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.pcbxLogo.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.Control;
            this.lblVersion.Location = new System.Drawing.Point(190, 268);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(146, 32);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version 1.0";
            this.lblVersion.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.lblVersion.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.ForeColor = System.Drawing.SystemColors.Control;
            this.lblProjectName.Location = new System.Drawing.Point(43, 220);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(458, 37);
            this.lblProjectName.TabIndex = 2;
            this.lblProjectName.Text = "Inventory Management System";
            this.lblProjectName.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.lblProjectName.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // btnBackUp
            // 
            this.btnBackUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBackUp.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnBackUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackUp.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackUp.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBackUp.Location = new System.Drawing.Point(12, 786);
            this.btnBackUp.Name = "btnBackUp";
            this.btnBackUp.Size = new System.Drawing.Size(122, 45);
            this.btnBackUp.TabIndex = 7;
            this.btnBackUp.Text = "Backup";
            this.btnBackUp.UseVisualStyleBackColor = false;
            this.btnBackUp.MouseEnter += new System.EventHandler(this.pnlLeft_MouseEnter);
            this.btnBackUp.MouseLeave += new System.EventHandler(this.pnlLeft_MouseLeave);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 843);
            this.Controls.Add(this.panel1);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hidePass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShowPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.lblImgName.ResumeLayout(false);
            this.lblImgName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox ShowPass;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label forgetPass;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.PictureBox pcbxLogo;
        private System.Windows.Forms.PictureBox hidePass;
        private System.Windows.Forms.Panel lblImgName;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBackUp;
    }
}

