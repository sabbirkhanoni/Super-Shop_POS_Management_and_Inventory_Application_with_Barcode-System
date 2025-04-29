using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    
    public partial class AdminCustomersPurchaseDetails : Form
    {
        DataAccess db;
        private int pdfCounter = 0;
        public AdminCustomersPurchaseDetails()
        {
            db = new DataAccess();
            InitializeComponent();
            ExtraDesign();
            PopulateCustomerDueData();
            PopulateCustomerComboBox();


        }


        public void ExtraDesign()
        {
            DGVSaleDetails.RowTemplate.Height = 35; // Example height in pixels

            DGVSaleDetails.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVSaleDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVSaleDetails.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVSaleDetails.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }

        private void PopulateCustomerComboBox()
        {
            try
            {
                var ds = this.db.ExecuteQuery("SELECT CustomerId, CustomerName FROM CustomerTable");
                cmbCustomerName.DataSource = ds.Tables[0];
                cmbCustomerName.DisplayMember = "CustomerName";
                cmbCustomerName.ValueMember = "CustomerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the customer combo box: " + ex.Message);
            }
        }

        private void PopulateCustomerDueData(string query = @"
            SELECT  
                c.CustomerId AS 'ID',
                c.CustomerName AS 'Custo. Name',
                pt.ProductName AS 'Prod. Name',
                ct.CategoryName AS 'Category',
                bt.BrandName AS 'Brand',
                pt.PurchaseCost AS 'Purchase Price',
                pt.SalePrice AS 'Sale Price',
                sdt.SaleQuantity AS 'Sold Quantity',
                (sdt.SaleQuantity * sdt.UnitPrice) AS 'Sold Price',
                st.SaleDate AS 'Date'
            FROM 
                SaleDetailTable sdt
            INNER JOIN 
                SaleTable st ON sdt.SaleId = st.SaleId
            INNER JOIN 
                CustomerTable c ON st.CustomerId = c.CustomerId
            INNER JOIN 
                ProductTable pt ON sdt.ProductId = pt.ProductId
            LEFT JOIN 
                CategoryTable ct ON pt.CategoryId = ct.CategoryId
            LEFT JOIN 
                BrandTable bt ON pt.BrandId = bt.BrandId
            LEFT JOIN 
                CustomerDueTable cd ON c.CustomerId = cd.CustomerId;
            ")
        {
            try
            {
                if (db != null)
                {
                    var ds = this.db.ExecuteQuery(query);
                    this.DGVSaleDetails.AutoGenerateColumns = true;
                    this.DGVSaleDetails.DataSource = ds.Tables[0];
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminMainDashBoard adminMainDashBoard = new AdminMainDashBoard();
            adminMainDashBoard.Show();
            this.Hide();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    Title = "Save PDF File",
                    FileName = $"Customer_Purchase_Details_{pdfCounter}_{DateTime.Now:dd-MM-yyyy}.pdf"
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;

                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        Document doc = new Document(PageSize.A4);
                        PdfWriter.GetInstance(doc, stream);

                        doc.Open();

                        // Title Font
                        var titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL);

                        // Add a title to the document
                        Paragraph title = new Paragraph("More Details", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 5
                        };
                        doc.Add(title);

                        // Create a table
                        PdfPTable pdfTable = new PdfPTable(DGVSaleDetails.ColumnCount)
                        {
                            WidthPercentage = 110,
                            SpacingBefore = 5,
                            SpacingAfter = 5
                        };

                        // Header Font
                        var headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);

                        foreach (DataGridViewColumn column in DGVSaleDetails.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, headerFont))
                            {
                                BackgroundColor = BaseColor.DARK_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5,
                                FixedHeight = 25 // Set fixed height for the header row
                            };
                            pdfTable.AddCell(headerCell);
                        }

                        // Row Font
                        var rowFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);

                        for (int i = 0; i < DGVSaleDetails.Rows.Count; i++)
                        {
                            DataGridViewRow row = DGVSaleDetails.Rows[i];
                            BaseColor rowColor = (i % 2 == 0) ? new BaseColor(173, 216, 230) : new BaseColor(255, 228, 196);

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty, rowFont))
                                {
                                    BackgroundColor = rowColor,
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    FixedHeight = 25 // Set fixed height for the row cells
                                };
                                pdfTable.AddCell(pdfCell);
                            }
                        }

                        doc.Add(pdfTable);
                        doc.Close();
                    }

                    MessageBox.Show("PDF file generated successfully!");
                    pdfCounter = pdfCounter + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void DGVSaleDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleDetails.ClearSelection();
            DGVSaleDetails.CurrentCell = null;
        }
    }
}
