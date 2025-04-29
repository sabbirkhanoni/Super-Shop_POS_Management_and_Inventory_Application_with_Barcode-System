using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using ZXing.OneD;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace InventoryManagementSystem
{
    public partial class AdminSaleDetails : Form
    {
        DataAccess db;
        private int pdfCounter = 1; // Counter to track the number of generated PDFs

        public AdminSaleDetails()
        {
            db = new DataAccess();
            InitializeComponent();
            ExtraDesign();
            PopulateCustomerDueData(); // Load all data initially
        }

        public void ExtraDesign()
        {
            DGVSaleDetails.RowTemplate.Height = 35; // Example height in pixels

            DGVSaleDetails.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVSaleDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVSaleDetails.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVSaleDetails.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row
        }

        private void PopulateCustomerDueData(string query = @"
               SELECT  
                pt.ProductId AS 'ID',
                pt.ProductName AS 'Name',
                ct.CategoryName AS 'Category',
                bt.BrandName AS 'Brand',
                pt.PurchaseCost AS 'Purch. Cost',
                pt.SalePrice AS 'Sale Price',
                ISNULL(SUM(sdt.SaleQuantity), 0) AS 'Sold Quant.',
                ISNULL(SUM(sdt.SaleQuantity * sdt.UnitPrice), 0) AS 'Sold Price',
                ISNULL(MAX(pdt.Quantity), 0) AS 'Available'
            FROM 
                ProductTable pt
            LEFT JOIN 
                CategoryTable ct ON pt.CategoryId = ct.CategoryId
            LEFT JOIN 
                BrandTable bt ON pt.BrandId = bt.BrandId
            LEFT JOIN 
                PurchaseDetailTable pdt ON pt.ProductId = pdt.ProductId
            LEFT JOIN 
                SaleDetailTable sdt ON pt.ProductId = sdt.ProductId
            GROUP BY 
                pt.ProductId, 
                pt.ProductName, 
                ct.CategoryName, 
                bt.BrandName, 
                pt.PurchaseCost, 
                pt.SalePrice
            ORDER BY 
                pt.ProductId;

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Fetch filter values
            string search = txtSearchBar.Text.Trim();
            DateTime? startDate = dtpStartDate.Value.Date;
            DateTime? endDate = dtpEndDate.Value.Date;

            // Load filtered data
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminMainDashBoard adminMainDashBoard = new AdminMainDashBoard();
            adminMainDashBoard.Show();
            this.Hide();
        }

        private void DGVSaleDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleDetails.ClearSelection();
            DGVSaleDetails.CurrentCell = null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    Title = "Save PDF File",
                    FileName = $"Sale_Details_{pdfCounter}_{DateTime.Now:dd-MM-yyyy}.pdf"
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
                        Paragraph title = new Paragraph("Sale Details", titleFont)
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

        private void btnMoreDetails_Click(object sender, EventArgs e)
        {
            AdminMoreSaleDetails adminMoreSaleDetails = new AdminMoreSaleDetails();
            adminMoreSaleDetails.Show();
            this.Hide();
        }
    }
}
