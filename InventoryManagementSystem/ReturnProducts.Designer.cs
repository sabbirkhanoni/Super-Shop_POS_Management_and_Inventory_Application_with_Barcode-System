namespace InventoryManagementSystem
{
    partial class ReturnProducts
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.DGVReturnDamageProducts = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSearchBar = new System.Windows.Forms.TextBox();
            this.btnDamageProduct = new System.Windows.Forms.Button();
            this.btnReturnProduct = new System.Windows.Forms.Button();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCustomerName = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtCurrentDueOfSelectedCustomerDue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDueOfSelectedCustomer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReturnMoney = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVReturnDamageProducts)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(7, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(453, 38);
            this.label13.TabIndex = 23;
            this.label13.Text = "Return And Damage Products";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1530, 61);
            this.panel1.TabIndex = 24;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBack.Location = new System.Drawing.Point(1401, 8);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(111, 48);
            this.btnBack.TabIndex = 29;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // DGVReturnDamageProducts
            // 
            this.DGVReturnDamageProducts.AllowUserToAddRows = false;
            this.DGVReturnDamageProducts.AllowUserToDeleteRows = false;
            this.DGVReturnDamageProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVReturnDamageProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVReturnDamageProducts.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVReturnDamageProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVReturnDamageProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVReturnDamageProducts.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGVReturnDamageProducts.Location = new System.Drawing.Point(0, 359);
            this.DGVReturnDamageProducts.Name = "DGVReturnDamageProducts";
            this.DGVReturnDamageProducts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVReturnDamageProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGVReturnDamageProducts.RowHeadersWidth = 51;
            this.DGVReturnDamageProducts.RowTemplate.Height = 35;
            this.DGVReturnDamageProducts.Size = new System.Drawing.Size(1530, 483);
            this.DGVReturnDamageProducts.TabIndex = 27;
            this.DGVReturnDamageProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVReturnDamageProducts_CellClick);
            this.DGVReturnDamageProducts.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGVReturnDamageProducts_DataBindingComplete);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtSearchBar);
            this.panel2.Controls.Add(this.btnDamageProduct);
            this.panel2.Controls.Add(this.btnReturnProduct);
            this.panel2.Controls.Add(this.txtQuantity);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbCustomerName);
            this.panel2.Location = new System.Drawing.Point(10, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(795, 286);
            this.panel2.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(163, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 25);
            this.label5.TabIndex = 39;
            this.label5.Text = "Search Here";
            // 
            // txtSearchBar
            // 
            this.txtSearchBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchBar.Location = new System.Drawing.Point(346, 25);
            this.txtSearchBar.Name = "txtSearchBar";
            this.txtSearchBar.Size = new System.Drawing.Size(284, 38);
            this.txtSearchBar.TabIndex = 38;
            // 
            // btnDamageProduct
            // 
            this.btnDamageProduct.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnDamageProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDamageProduct.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDamageProduct.Location = new System.Drawing.Point(517, 225);
            this.btnDamageProduct.Name = "btnDamageProduct";
            this.btnDamageProduct.Size = new System.Drawing.Size(113, 48);
            this.btnDamageProduct.TabIndex = 37;
            this.btnDamageProduct.Text = "Damage";
            this.btnDamageProduct.UseVisualStyleBackColor = false;
            this.btnDamageProduct.Click += new System.EventHandler(this.btnDamageProduct_Click);
            // 
            // btnReturnProduct
            // 
            this.btnReturnProduct.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnReturnProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturnProduct.ForeColor = System.Drawing.SystemColors.Control;
            this.btnReturnProduct.Location = new System.Drawing.Point(346, 225);
            this.btnReturnProduct.Name = "btnReturnProduct";
            this.btnReturnProduct.Size = new System.Drawing.Size(113, 48);
            this.btnReturnProduct.TabIndex = 36;
            this.btnReturnProduct.Text = "Return";
            this.btnReturnProduct.UseVisualStyleBackColor = false;
            this.btnReturnProduct.Click += new System.EventHandler(this.btnReturnProduct_Click);
            // 
            // txtQuantity
            // 
            this.txtQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(346, 170);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(284, 38);
            this.txtQuantity.TabIndex = 35;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(167, 173);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 25);
            this.label9.TabIndex = 34;
            this.label9.Text = "Quantity";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(166, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 25);
            this.label6.TabIndex = 33;
            this.label6.Text = "Customer Name";
            // 
            // cmbCustomerName
            // 
            this.cmbCustomerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCustomerName.FormattingEnabled = true;
            this.cmbCustomerName.Location = new System.Drawing.Point(346, 97);
            this.cmbCustomerName.Name = "cmbCustomerName";
            this.cmbCustomerName.Size = new System.Drawing.Size(284, 37);
            this.cmbCustomerName.TabIndex = 32;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel3.Controls.Add(this.txtCurrentDueOfSelectedCustomerDue);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtDueOfSelectedCustomer);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtReturnMoney);
            this.panel3.Location = new System.Drawing.Point(811, 67);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(719, 286);
            this.panel3.TabIndex = 29;
            // 
            // txtCurrentDueOfSelectedCustomerDue
            // 
            this.txtCurrentDueOfSelectedCustomerDue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentDueOfSelectedCustomerDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentDueOfSelectedCustomerDue.Location = new System.Drawing.Point(239, 217);
            this.txtCurrentDueOfSelectedCustomerDue.Name = "txtCurrentDueOfSelectedCustomerDue";
            this.txtCurrentDueOfSelectedCustomerDue.ReadOnly = true;
            this.txtCurrentDueOfSelectedCustomerDue.Size = new System.Drawing.Size(284, 38);
            this.txtCurrentDueOfSelectedCustomerDue.TabIndex = 48;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 25);
            this.label3.TabIndex = 47;
            this.label3.Text = "Current Due";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(53, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 46;
            this.label2.Text = "Due";
            // 
            // txtDueOfSelectedCustomer
            // 
            this.txtDueOfSelectedCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDueOfSelectedCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDueOfSelectedCustomer.Location = new System.Drawing.Point(239, 38);
            this.txtDueOfSelectedCustomer.Name = "txtDueOfSelectedCustomer";
            this.txtDueOfSelectedCustomer.ReadOnly = true;
            this.txtDueOfSelectedCustomer.Size = new System.Drawing.Size(284, 38);
            this.txtDueOfSelectedCustomer.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 43;
            this.label1.Text = "Return Money";
            // 
            // txtReturnMoney
            // 
            this.txtReturnMoney.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReturnMoney.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReturnMoney.Location = new System.Drawing.Point(239, 124);
            this.txtReturnMoney.Name = "txtReturnMoney";
            this.txtReturnMoney.ReadOnly = true;
            this.txtReturnMoney.Size = new System.Drawing.Size(284, 38);
            this.txtReturnMoney.TabIndex = 44;
            // 
            // ReturnProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1532, 843);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.DGVReturnDamageProducts);
            this.Controls.Add(this.panel1);
            this.Name = "ReturnProducts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReturnProducts";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVReturnDamageProducts)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView DGVReturnDamageProducts;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDamageProduct;
        private System.Windows.Forms.Button btnReturnProduct;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCustomerName;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSearchBar;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDueOfSelectedCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReturnMoney;
        private System.Windows.Forms.TextBox txtCurrentDueOfSelectedCustomerDue;
        private System.Windows.Forms.Label label3;
    }
}