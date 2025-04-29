namespace InventoryManagementSystem
{
    partial class SalesmanMainDashBoard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.lblSale = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBarcodeInput = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchBar = new System.Windows.Forms.TextBox();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.DGVSaleInventory = new System.Windows.Forms.DataGridView();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cmbTransactionType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.chkbDiscount = new System.Windows.Forms.CheckBox();
            this.lblTotal = new System.Windows.Forms.TextBox();
            this.txtPayment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCustomerName = new System.Windows.Forms.ComboBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pcbxOpenCustomerForm = new System.Windows.Forms.PictureBox();
            this.DGVSaleCart = new System.Windows.Forms.DataGridView();
            this.ProductIdCart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductNameCart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalePriceCart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSaleInventory)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbxOpenCustomerForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSaleCart)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.lblSale);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1532, 54);
            this.panel1.TabIndex = 0;
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCopy.Location = new System.Drawing.Point(1341, 5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(91, 46);
            this.btnCopy.TabIndex = 24;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // lblSale
            // 
            this.lblSale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSale.AutoSize = true;
            this.lblSale.BackColor = System.Drawing.Color.MidnightBlue;
            this.lblSale.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSale.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSale.Location = new System.Drawing.Point(13, 9);
            this.lblSale.Name = "lblSale";
            this.lblSale.Size = new System.Drawing.Size(86, 38);
            this.lblSale.TabIndex = 3;
            this.lblSale.Text = "Sale";
            this.lblSale.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBack.Location = new System.Drawing.Point(1438, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(83, 48);
            this.btnBack.TabIndex = 23;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtBarcodeInput);
            this.panel2.Location = new System.Drawing.Point(7, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(305, 80);
            this.panel2.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Barcode";
            // 
            // txtBarcodeInput
            // 
            this.txtBarcodeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcodeInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcodeInput.Location = new System.Drawing.Point(12, 32);
            this.txtBarcodeInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBarcodeInput.Name = "txtBarcodeInput";
            this.txtBarcodeInput.Size = new System.Drawing.Size(285, 38);
            this.txtBarcodeInput.TabIndex = 14;
            this.txtBarcodeInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcodeInput_KeyDown);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtSearchBar);
            this.panel3.Controls.Add(this.btnAddToCart);
            this.panel3.Location = new System.Drawing.Point(7, 91);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(305, 158);
            this.panel3.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 25);
            this.label1.TabIndex = 15;
            this.label1.Text = "ID/Name";
            // 
            // txtSearchBar
            // 
            this.txtSearchBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchBar.Location = new System.Drawing.Point(12, 44);
            this.txtSearchBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearchBar.Name = "txtSearchBar";
            this.txtSearchBar.Size = new System.Drawing.Size(285, 38);
            this.txtSearchBar.TabIndex = 14;
            this.txtSearchBar.TextChanged += new System.EventHandler(this.txtSearchBar_TextChanged);
            this.txtSearchBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchBar_KeyDown);
            // 
            // btnAddToCart
            // 
            this.btnAddToCart.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnAddToCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddToCart.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddToCart.Location = new System.Drawing.Point(91, 95);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(137, 48);
            this.btnAddToCart.TabIndex = 37;
            this.btnAddToCart.Text = "Add To Cart";
            this.btnAddToCart.UseVisualStyleBackColor = false;
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);
            // 
            // DGVSaleInventory
            // 
            this.DGVSaleInventory.AllowUserToAddRows = false;
            this.DGVSaleInventory.AllowUserToDeleteRows = false;
            this.DGVSaleInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVSaleInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVSaleInventory.BackgroundColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVSaleInventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVSaleInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVSaleInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductId,
            this.ProductName,
            this.CategoryName,
            this.BrandName,
            this.PurchasePrice,
            this.SalePrice,
            this.AvailableQuantity});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVSaleInventory.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGVSaleInventory.Location = new System.Drawing.Point(0, 678);
            this.DGVSaleInventory.Margin = new System.Windows.Forms.Padding(4);
            this.DGVSaleInventory.Name = "DGVSaleInventory";
            this.DGVSaleInventory.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVSaleInventory.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGVSaleInventory.RowHeadersWidth = 51;
            this.DGVSaleInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVSaleInventory.Size = new System.Drawing.Size(1207, 163);
            this.DGVSaleInventory.TabIndex = 32;
            this.DGVSaleInventory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVSaleInventory_CellDoubleClick);
            this.DGVSaleInventory.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGVSaleInventory_DataBindingComplete);
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "ProductId";
            this.ProductId.HeaderText = "ID";
            this.ProductId.MinimumWidth = 6;
            this.ProductId.Name = "ProductId";
            this.ProductId.ReadOnly = true;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "Name";
            this.ProductName.MinimumWidth = 6;
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // CategoryName
            // 
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.HeaderText = "Category";
            this.CategoryName.MinimumWidth = 6;
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // BrandName
            // 
            this.BrandName.DataPropertyName = "BrandName";
            this.BrandName.HeaderText = "Brand";
            this.BrandName.MinimumWidth = 6;
            this.BrandName.Name = "BrandName";
            this.BrandName.ReadOnly = true;
            // 
            // PurchasePrice
            // 
            this.PurchasePrice.DataPropertyName = "PurchaseCost";
            this.PurchasePrice.HeaderText = "Purch. Price";
            this.PurchasePrice.MinimumWidth = 6;
            this.PurchasePrice.Name = "PurchasePrice";
            this.PurchasePrice.ReadOnly = true;
            // 
            // SalePrice
            // 
            this.SalePrice.DataPropertyName = "SalePrice";
            this.SalePrice.HeaderText = "Sale Price";
            this.SalePrice.MinimumWidth = 6;
            this.SalePrice.Name = "SalePrice";
            this.SalePrice.ReadOnly = true;
            // 
            // AvailableQuantity
            // 
            this.AvailableQuantity.DataPropertyName = "Quantity";
            this.AvailableQuantity.HeaderText = "Quantity";
            this.AvailableQuantity.MinimumWidth = 6;
            this.AvailableQuantity.Name = "AvailableQuantity";
            this.AvailableQuantity.ReadOnly = true;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Controls.Add(this.cmbTransactionType);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.lblChange);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.btnCheckOut);
            this.panel6.Controls.Add(this.chkbDiscount);
            this.panel6.Controls.Add(this.lblTotal);
            this.panel6.Controls.Add(this.txtPayment);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Location = new System.Drawing.Point(1214, 420);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(318, 421);
            this.panel6.TabIndex = 33;
            // 
            // cmbTransactionType
            // 
            this.cmbTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTransactionType.FormattingEnabled = true;
            this.cmbTransactionType.Items.AddRange(new object[] {
            "BKASH",
            "NAGAD",
            "UPAY",
            "OTHERS"});
            this.cmbTransactionType.Location = new System.Drawing.Point(161, 196);
            this.cmbTransactionType.Name = "cmbTransactionType";
            this.cmbTransactionType.Size = new System.Drawing.Size(136, 37);
            this.cmbTransactionType.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(30, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 25);
            this.label8.TabIndex = 42;
            this.label8.Text = "Payment In";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(30, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 25);
            this.label9.TabIndex = 39;
            this.label9.Text = "Change";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChange.ForeColor = System.Drawing.SystemColors.Control;
            this.lblChange.Location = new System.Drawing.Point(189, 156);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(101, 25);
            this.lblChange.TabIndex = 40;
            this.lblChange.Text = "lblChange";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(30, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 25);
            this.label4.TabIndex = 38;
            this.label4.Text = "Pay";
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnCheckOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckOut.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCheckOut.Location = new System.Drawing.Point(115, 264);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(113, 48);
            this.btnCheckOut.TabIndex = 34;
            this.btnCheckOut.Text = "Checkout";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // chkbDiscount
            // 
            this.chkbDiscount.AutoSize = true;
            this.chkbDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbDiscount.ForeColor = System.Drawing.SystemColors.Control;
            this.chkbDiscount.Location = new System.Drawing.Point(108, 67);
            this.chkbDiscount.Name = "chkbDiscount";
            this.chkbDiscount.Size = new System.Drawing.Size(143, 29);
            this.chkbDiscount.TabIndex = 41;
            this.chkbDiscount.Text = "Discounted?";
            this.chkbDiscount.UseVisualStyleBackColor = true;
            this.chkbDiscount.CheckedChanged += new System.EventHandler(this.chkbDiscount_CheckedChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(108, 13);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.ReadOnly = true;
            this.lblTotal.Size = new System.Drawing.Size(192, 38);
            this.lblTotal.TabIndex = 43;
            // 
            // txtPayment
            // 
            this.txtPayment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayment.Location = new System.Drawing.Point(108, 101);
            this.txtPayment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPayment.Name = "txtPayment";
            this.txtPayment.Size = new System.Drawing.Size(192, 38);
            this.txtPayment.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(30, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 25);
            this.label6.TabIndex = 35;
            this.label6.Text = "Total";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Customer Name";
            // 
            // cmbCustomerName
            // 
            this.cmbCustomerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCustomerName.FormattingEnabled = true;
            this.cmbCustomerName.Location = new System.Drawing.Point(8, 42);
            this.cmbCustomerName.Name = "cmbCustomerName";
            this.cmbCustomerName.Size = new System.Drawing.Size(285, 37);
            this.cmbCustomerName.TabIndex = 37;
            this.cmbCustomerName.SelectedValueChanged += new System.EventHandler(this.cmbCustomerName_SelectedValueChanged);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.pcbxOpenCustomerForm);
            this.panel5.Controls.Add(this.cmbCustomerName);
            this.panel5.Location = new System.Drawing.Point(7, 255);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(305, 103);
            this.panel5.TabIndex = 31;
            // 
            // pcbxOpenCustomerForm
            // 
            this.pcbxOpenCustomerForm.Image = global::InventoryManagementSystem.Properties.Resources.plus;
            this.pcbxOpenCustomerForm.Location = new System.Drawing.Point(260, 11);
            this.pcbxOpenCustomerForm.Name = "pcbxOpenCustomerForm";
            this.pcbxOpenCustomerForm.Size = new System.Drawing.Size(22, 25);
            this.pcbxOpenCustomerForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbxOpenCustomerForm.TabIndex = 39;
            this.pcbxOpenCustomerForm.TabStop = false;
            this.pcbxOpenCustomerForm.Click += new System.EventHandler(this.pcbxOpenCustomerForm_Click);
            // 
            // DGVSaleCart
            // 
            this.DGVSaleCart.AllowUserToAddRows = false;
            this.DGVSaleCart.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(1);
            this.DGVSaleCart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.DGVSaleCart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVSaleCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVSaleCart.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVSaleCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.DGVSaleCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVSaleCart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductIdCart,
            this.ProductNameCart,
            this.SalePriceCart,
            this.Quantity,
            this.SaleValue,
            this.Delete});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVSaleCart.DefaultCellStyle = dataGridViewCellStyle7;
            this.DGVSaleCart.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DGVSaleCart.Location = new System.Drawing.Point(0, 58);
            this.DGVSaleCart.Margin = new System.Windows.Forms.Padding(4);
            this.DGVSaleCart.MultiSelect = false;
            this.DGVSaleCart.Name = "DGVSaleCart";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVSaleCart.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DGVSaleCart.RowHeadersWidth = 51;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DGVSaleCart.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.DGVSaleCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVSaleCart.Size = new System.Drawing.Size(1207, 612);
            this.DGVSaleCart.TabIndex = 41;
            this.DGVSaleCart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVSaleCart_CellContentClick);
            this.DGVSaleCart.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVSaleCart_CellEndEdit);
            this.DGVSaleCart.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGVSaleCart_DataBindingComplete);
            // 
            // ProductIdCart
            // 
            this.ProductIdCart.DataPropertyName = "ProductId";
            this.ProductIdCart.FillWeight = 84.74184F;
            this.ProductIdCart.HeaderText = "ID";
            this.ProductIdCart.MinimumWidth = 7;
            this.ProductIdCart.Name = "ProductIdCart";
            // 
            // ProductNameCart
            // 
            this.ProductNameCart.DataPropertyName = "ProductName";
            this.ProductNameCart.FillWeight = 115.4059F;
            this.ProductNameCart.HeaderText = "Name";
            this.ProductNameCart.MinimumWidth = 6;
            this.ProductNameCart.Name = "ProductNameCart";
            // 
            // SalePriceCart
            // 
            this.SalePriceCart.DataPropertyName = "SalePrice";
            this.SalePriceCart.FillWeight = 70.30508F;
            this.SalePriceCart.HeaderText = "Price";
            this.SalePriceCart.MinimumWidth = 6;
            this.SalePriceCart.Name = "SalePriceCart";
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "SaleQuantity";
            this.Quantity.FillWeight = 66.5034F;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.MinimumWidth = 6;
            this.Quantity.Name = "Quantity";
            // 
            // SaleValue
            // 
            this.SaleValue.DataPropertyName = "SaleValue";
            this.SaleValue.FillWeight = 116.0537F;
            this.SaleValue.HeaderText = "Sale Price";
            this.SaleValue.MinimumWidth = 6;
            this.SaleValue.Name = "SaleValue";
            // 
            // Delete
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete.DefaultCellStyle = dataGridViewCellStyle6;
            this.Delete.FillWeight = 86.56239F;
            this.Delete.HeaderText = "Delete";
            this.Delete.MinimumWidth = 6;
            this.Delete.Name = "Delete";
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Location = new System.Drawing.Point(1214, 57);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(318, 363);
            this.panel4.TabIndex = 42;
            // 
            // SalesmanMainDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1532, 843);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.DGVSaleCart);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.DGVSaleInventory);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SalesmanMainDashBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SalesmanMainDashBoard";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSaleInventory)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbxOpenCustomerForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSaleCart)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSale;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBarcodeInput;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchBar;
        private System.Windows.Forms.DataGridView DGVSaleInventory;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddToCart;
        private System.Windows.Forms.PictureBox pcbxOpenCustomerForm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbCustomerName;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPayment;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox lblTotal;
        private System.Windows.Forms.CheckBox chkbDiscount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbTransactionType;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataGridView DGVSaleCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchasePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableQuantity;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductIdCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductNameCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalePriceCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleValue;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.Panel panel4;
    }
}