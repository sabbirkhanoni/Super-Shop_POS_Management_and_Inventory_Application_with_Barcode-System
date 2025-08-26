namespace InventoryManagementSystem
{
    partial class CustomerDueDetails
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.DGVCustomerDueDetails = new System.Windows.Forms.DataGridView();
            this.CustomerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContactInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DueAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSearchBar = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPaymentAmount = new System.Windows.Forms.TextBox();
            this.txtCurrentDueSelectedCustomer = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnAllPrint = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSendMessageToAll = new System.Windows.Forms.Button();
            this.btnPay = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCustomerDueDetails)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1523, 66);
            this.panel2.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(13, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(354, 38);
            this.label13.TabIndex = 25;
            this.label13.Text = "Customers Due Details";
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBack.Location = new System.Drawing.Point(1410, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(113, 60);
            this.btnBack.TabIndex = 22;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // DGVCustomerDueDetails
            // 
            this.DGVCustomerDueDetails.AllowUserToAddRows = false;
            this.DGVCustomerDueDetails.AllowUserToDeleteRows = false;
            this.DGVCustomerDueDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVCustomerDueDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVCustomerDueDetails.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVCustomerDueDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVCustomerDueDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVCustomerDueDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CustomerId,
            this.CustomerName,
            this.ContactInfo,
            this.CustomerEmail,
            this.DueDate,
            this.DueAmount});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVCustomerDueDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGVCustomerDueDetails.Location = new System.Drawing.Point(4, 262);
            this.DGVCustomerDueDetails.Name = "DGVCustomerDueDetails";
            this.DGVCustomerDueDetails.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVCustomerDueDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGVCustomerDueDetails.RowHeadersWidth = 51;
            this.DGVCustomerDueDetails.RowTemplate.Height = 35;
            this.DGVCustomerDueDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVCustomerDueDetails.Size = new System.Drawing.Size(1523, 575);
            this.DGVCustomerDueDetails.TabIndex = 27;
            this.DGVCustomerDueDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCustomerDueDetails_CellClick);
            this.DGVCustomerDueDetails.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGVCustomerDueDetails_DataBindingComplete);
            // 
            // CustomerId
            // 
            this.CustomerId.DataPropertyName = "CustomerId";
            this.CustomerId.HeaderText = "ID";
            this.CustomerId.MinimumWidth = 6;
            this.CustomerId.Name = "CustomerId";
            this.CustomerId.ReadOnly = true;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "Name";
            this.CustomerName.MinimumWidth = 6;
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // ContactInfo
            // 
            this.ContactInfo.DataPropertyName = "ContactInfo";
            this.ContactInfo.HeaderText = "Contact No";
            this.ContactInfo.MinimumWidth = 6;
            this.ContactInfo.Name = "ContactInfo";
            this.ContactInfo.ReadOnly = true;
            // 
            // CustomerEmail
            // 
            this.CustomerEmail.DataPropertyName = "CustomerEmail";
            this.CustomerEmail.HeaderText = "Email";
            this.CustomerEmail.MinimumWidth = 6;
            this.CustomerEmail.Name = "CustomerEmail";
            this.CustomerEmail.ReadOnly = true;
            // 
            // DueDate
            // 
            this.DueDate.DataPropertyName = "DueDate";
            this.DueDate.HeaderText = "Date";
            this.DueDate.MinimumWidth = 6;
            this.DueDate.Name = "DueDate";
            this.DueDate.ReadOnly = true;
            // 
            // DueAmount
            // 
            this.DueAmount.DataPropertyName = "DueAmount";
            this.DueAmount.HeaderText = "Due";
            this.DueAmount.MinimumWidth = 6;
            this.DueAmount.Name = "DueAmount";
            this.DueAmount.ReadOnly = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.txtSearchBar);
            this.panel4.Location = new System.Drawing.Point(12, 73);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(492, 71);
            this.panel4.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Search Here";
            // 
            // txtSearchBar
            // 
            this.txtSearchBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchBar.Location = new System.Drawing.Point(141, 21);
            this.txtSearchBar.Name = "txtSearchBar";
            this.txtSearchBar.Size = new System.Drawing.Size(284, 38);
            this.txtSearchBar.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtPaymentAmount);
            this.panel1.Controls.Add(this.txtCurrentDueSelectedCustomer);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Location = new System.Drawing.Point(821, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 183);
            this.panel1.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(411, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 25);
            this.label2.TabIndex = 35;
            this.label2.Text = "Payment Amount";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "Current Due";
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaymentAmount.Location = new System.Drawing.Point(408, 44);
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(284, 38);
            this.txtPaymentAmount.TabIndex = 34;
            // 
            // txtCurrentDueSelectedCustomer
            // 
            this.txtCurrentDueSelectedCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentDueSelectedCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentDueSelectedCustomer.Location = new System.Drawing.Point(16, 44);
            this.txtCurrentDueSelectedCustomer.Name = "txtCurrentDueSelectedCustomer";
            this.txtCurrentDueSelectedCustomer.ReadOnly = true;
            this.txtCurrentDueSelectedCustomer.Size = new System.Drawing.Size(284, 38);
            this.txtCurrentDueSelectedCustomer.TabIndex = 32;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.btnAllPrint);
            this.panel5.Controls.Add(this.btnPrint);
            this.panel5.Controls.Add(this.btnSendMessageToAll);
            this.panel5.Controls.Add(this.btnPay);
            this.panel5.Location = new System.Drawing.Point(12, 99);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(684, 71);
            this.panel5.TabIndex = 33;
            // 
            // btnAllPrint
            // 
            this.btnAllPrint.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnAllPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllPrint.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAllPrint.Location = new System.Drawing.Point(358, 12);
            this.btnAllPrint.Name = "btnAllPrint";
            this.btnAllPrint.Size = new System.Drawing.Size(113, 48);
            this.btnAllPrint.TabIndex = 9;
            this.btnAllPrint.Text = "All Print";
            this.btnAllPrint.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.SystemColors.Control;
            this.btnPrint.Location = new System.Drawing.Point(204, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(113, 48);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnSendMessageToAll
            // 
            this.btnSendMessageToAll.BackColor = System.Drawing.Color.Brown;
            this.btnSendMessageToAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendMessageToAll.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSendMessageToAll.Location = new System.Drawing.Point(508, 12);
            this.btnSendMessageToAll.Name = "btnSendMessageToAll";
            this.btnSendMessageToAll.Size = new System.Drawing.Size(165, 48);
            this.btnSendMessageToAll.TabIndex = 8;
            this.btnSendMessageToAll.Text = "Send Message";
            this.btnSendMessageToAll.UseVisualStyleBackColor = false;
            // 
            // btnPay
            // 
            this.btnPay.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.ForeColor = System.Drawing.SystemColors.Control;
            this.btnPay.Location = new System.Drawing.Point(44, 12);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(113, 48);
            this.btnPay.TabIndex = 6;
            this.btnPay.Text = "Pay";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // CustomerDueDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1532, 843);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.DGVCustomerDueDetails);
            this.Controls.Add(this.panel2);
            this.Name = "CustomerDueDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerDueDetails";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCustomerDueDetails)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataGridView DGVCustomerDueDetails;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSearchBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.TextBox txtCurrentDueSelectedCustomer;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnAllPrint;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSendMessageToAll;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContactInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn DueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DueAmount;
        private System.Windows.Forms.Label label13;
    }
}