using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventoryManagementSystem
{
    public partial class AdminAllSaleDetails : Form
    {
        private int pdfCounter = 1; // Counter to track the number of generated PDFs
        int excelCounter = 1;
        private int currentRowIndex = 0; // Add this class-level variable to track current row position


        DataAccess db;
        public AdminAllSaleDetails()
        {
            InitializeComponent();
            ExtraDesign();
            db = new DataAccess();
            PopulateAllSaleDetailData();
            ApplySaleDetailsLayout();
        }



        private void PopulateAllSaleDetailData(string query = @"
               SELECT 
                -- Identification
                s.SaleId,

                -- Combined Customer Info
                CAST(c.CustomerId AS NVARCHAR(50)) + ' - ' + c.CustomerName AS CustomerInfo,

                -- Combined Product Info
                CAST(p.ProductId AS NVARCHAR(50)) + ' - ' + 
                p.ProductName + ' - ' + 
                cat.CategoryName + ' - ' + 
                b.BrandName AS ProductInfo,

                -- Pricing & Cost
                p.PurchaseCost,
                p.SalePrice,
                sd.UnitPrice AS ActualSoldPrice,
                sd.SaleQuantity,
                (p.SalePrice * sd.SaleQuantity) AS SaleValue,
                CAST(ROUND(( (p.SalePrice - sd.UnitPrice) * sd.SaleQuantity ), 2) AS DECIMAL(18,2)) AS EachDiscount,  -- FIXED
                (sd.UnitPrice * sd.SaleQuantity) AS EachSoldPrice,

                -- Payment & Discount (per product line)
                CAST(ROUND((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(gross.SaleGross, 0), 3) AS DECIMAL(18,3)) AS EachSoldPaidAmount,

                -- Each Sold Due (EachSoldPrice - EachSoldPaidAmount)
                CAST(ROUND((sd.UnitPrice * sd.SaleQuantity) - ((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(gross.SaleGross, 0)), 3) AS DECIMAL(18,3)) AS EachSoldDueAmount,
    
                -- Sale summary
                s.PayAmount AS SaleTotalAmount,
                s.PaidAmount AS SaleTotalPaidAmount,
                ISNULL(cd.DueAmount, 0) AS SaleDueAmount,  
                ISNULL(total_due.TotalDueAmount, 0) AS TotalDueAmount,

                -- Profit
                ((sd.UnitPrice * sd.SaleQuantity) - (sd.SaleQuantity * p.PurchaseCost)) AS WouldBeProfit,
                ROUND(
                    ((sd.UnitPrice * sd.SaleQuantity * s.PaidAmount) / NULLIF(gross.SaleGross, 0))
                    - ((sd.SaleQuantity * p.PurchaseCost * s.PaidAmount) / NULLIF(gross.SaleGross, 0)),
                2) AS CurrentProfit,

                -- Transaction
                s.TransactionType,
                s.SaleDate

            FROM SaleTable s
            INNER JOIN CustomerTable c ON s.CustomerId = c.CustomerId
            INNER JOIN SaleDetailTable sd ON s.SaleId = sd.SaleId
            INNER JOIN ProductTable p ON sd.ProductId = p.ProductId
            INNER JOIN CategoryTable cat ON p.CategoryId = cat.CategoryId
            INNER JOIN BrandTable b ON p.BrandId = b.BrandId

            -- Per-sale gross to allocate PaidAmount proportionally
            CROSS APPLY (
                SELECT SUM(sd2.UnitPrice * sd2.SaleQuantity) AS SaleGross
                FROM SaleDetailTable sd2
                WHERE sd2.SaleId = s.SaleId
            ) gross

            -- Specific sale due amount
            LEFT JOIN CustomerDueTable cd 
                ON s.SaleId = cd.SaleId 
                AND s.CustomerId = cd.CustomerId

            -- Total due for the customer
            LEFT JOIN (
                SELECT CustomerId, SUM(DueAmount) AS TotalDueAmount
                FROM CustomerDueTable
                GROUP BY CustomerId
            ) total_due 
                ON s.CustomerId = total_due.CustomerId

            ORDER BY s.SaleDate DESC, s.SaleId, ProductInfo;

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


        // Call this AFTER you set DataSource, or in DataBindingComplete
        void ApplySaleDetailsLayout()
        {
            DGVSaleDetails.ScrollBars = ScrollBars.Both;
            DGVSaleDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var widths = new Dictionary<string, int>
            {
                ["SaleId"] = 70,
                ["CustomerInfo"] = 170,
                ["ProductInfo"] = 450,


                ["PurchaseCost"] = 120,
                ["SalePrice"] = 120,
                ["ActualSoldPrice"] = 120,
                ["SaleQuantity"] = 120,
                ["SaleValue"] = 120,
                ["EachDiscount"] = 120,
                
                ["EachSoldPrice"] = 130,
                ["EachSoldPaidAmount"] = 120,
                ["EachSoldDueAmount"] = 120,


                ["SaleTotalAmount"] = 130,
                ["SaleTotalPaidAmount"] = 130,
                ["SaleDueAmount"] = 130,
                ["TotalDueAmount"] = 130,
                

                ["WouldBeProfit"] = 130,
                ["CurrentProfit"] = 130,

                ["TransactionType"] = 120,
                ["SaleDate"] = 160

                
            };

            foreach (var kv in widths)
                if (DGVSaleDetails.Columns.Contains(kv.Key))
                {
                    var col = DGVSaleDetails.Columns[kv.Key];
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = kv.Value;
                }

            // Optional precise formats you asked for earlier:
            if (DGVSaleDetails.Columns.Contains("LinePaidAmount"))
                DGVSaleDetails.Columns["LinePaidAmount"].DefaultCellStyle.Format = "0.000";
            if (DGVSaleDetails.Columns.Contains("LineDiscount"))
                DGVSaleDetails.Columns["LineDiscount"].DefaultCellStyle.Format = "0.00";
        }



        public void ExtraDesign()
        {
            DGVSaleDetails.RowTemplate.Height = 35; // Example height in pixels

            DGVSaleDetails.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue; // Default row color
            DGVSaleDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Alternate row color

            DGVSaleDetails.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue; // Background color for selected row
            DGVSaleDetails.RowsDefaultCellStyle.SelectionForeColor = Color.White;   // Foreground color for selected row

            DGVSaleDetails.SuspendLayout();


            DGVSaleDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        }




        private void btnBack_Click(object sender, EventArgs e)
        {
            FormManager.OpenForm(this, typeof(AdminMainDashBoard));
        }


        private void DGVSaleDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVSaleDetails.ClearSelection();
            DGVSaleDetails.CurrentCell = null;
        }




        private void btnPdf_Click(object sender, EventArgs e)
        {

            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    Title = "Save PDF File",
                    FileName = $"All Sale_Details_{pdfCounter}_{DateTime.Now:dd-MM-yyyy_hh_mm_tt}.pdf"
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        // Landscape Tabloid (11" x 17")
                        Document doc = new Document(PageSize.TABLOID.Rotate());
                        doc.SetMargins(18f, 18f, 18f, 18f); // 0.25 inch margins (~18f)
                        PdfWriter writer = PdfWriter.GetInstance(doc, stream);

                        // Create page event helper to handle header repetition
                        PdfPageEventHelper pageEventHelper = new PdfPageEventHelper();
                        writer.PageEvent = pageEventHelper;

                        doc.Open();

                        // Title (only on first page)
                        var titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
                        Paragraph title = new Paragraph("All Sale Details", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 10
                        };
                        doc.Add(title);

                        // Create table
                        PdfPTable pdfTable = new PdfPTable(DGVSaleDetails.ColumnCount)
                        {
                            WidthPercentage = 100, // Fill page width
                            SpacingBefore = 5,
                            SpacingAfter = 5,
                            HeaderRows = 1 // This makes the first row repeat on every page
                        };

                        // Set custom column widths (relative scale)
                        float[] columnWidths = new float[]
                        {
                            4f, 10f, 4f, 12f, 8f, 8f,
                            8f, 8f,8f, 8f,8f, 8f,
                            8f, 8f,8f, 8f,8f, 8f,
                            8f, 8f,8f, 8f,8f, 8f
                        };

                        if (columnWidths.Length == DGVSaleDetails.ColumnCount)
                        {
                            pdfTable.SetWidths(columnWidths);
                        }
                        else
                        {
                            throw new Exception("Column width count does not match the number of DataGridView columns.");
                        }

                        // Header font
                        var headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);

                        // Add header cells
                        foreach (DataGridViewColumn column in DGVSaleDetails.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, headerFont))
                            {
                                BackgroundColor = BaseColor.DARK_GRAY,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5,
                                FixedHeight = 30
                            };
                            pdfTable.AddCell(headerCell);
                        }

                        // Row font
                        var rowFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);

                        // Add data rows
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
                                    FixedHeight = 30
                                };
                                pdfTable.AddCell(pdfCell);
                            }
                        }

                        // Add table to document
                        doc.Add(pdfTable);
                        doc.Close();
                    }

                    MessageBox.Show("PDF file generated successfully!");
                    pdfCounter++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVSaleDetails.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.");
                    return;
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    Title = "Save Excel File",
                    FileName = $"All_Sale_Details_{excelCounter}_{DateTime.Now:yyyy-MM-dd_hh-mm-tt}.xlsx"
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;

                    Excel.Application excelApp = new Excel.Application();
                    excelApp.Workbooks.Add(Type.Missing);

                    Excel.Worksheet sheet = (Excel.Worksheet)excelApp.ActiveSheet;

                    // Title
                    sheet.Cells[1, 1] = "All Sale Details";
                    Excel.Range titleRange = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, DGVSaleDetails.Columns.Count]];
                    titleRange.Merge();
                    titleRange.Font.Bold = true;
                    titleRange.Font.Size = 12;
                    titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    titleRange.RowHeight = 30;

                    // Headers
                    for (int i = 0; i < DGVSaleDetails.Columns.Count; i++)
                    {
                        sheet.Cells[2, i + 1] = DGVSaleDetails.Columns[i].HeaderText;
                        Excel.Range headerCell = (Excel.Range)sheet.Cells[2, i + 1];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Size = 12;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGray);
                        headerCell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        headerCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        headerCell.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    }

                    // Data rows
                    for (int i = 0; i < DGVSaleDetails.Rows.Count; i++)
                    {
                        for (int j = 0; j < DGVSaleDetails.Columns.Count; j++)
                        {
                            sheet.Cells[i + 3, j + 1] = DGVSaleDetails.Rows[i].Cells[j].Value?.ToString() ?? "";
                            Excel.Range dataCell = (Excel.Range)sheet.Cells[i + 3, j + 1];
                            dataCell.Font.Size = 15;
                            dataCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            dataCell.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                            if (i % 2 == 0)
                                dataCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                            else
                                dataCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Bisque);
                        }
                    }

                    // Row height for all
                    Excel.Range allRows = sheet.Range[
                        sheet.Cells[1, 1],
                        sheet.Cells[DGVSaleDetails.Rows.Count + 2, DGVSaleDetails.Columns.Count]
                    ];
                    allRows.RowHeight = 25;

                    // ✅ Set specific column widths for 14 columns
                    double[] colWidths = { 10, 25, 15, 35, 12, 12, 11, 11, 11, 15, 15, 15, 15, 15, 12, 12, 11, 11, 11, 15, 20, 15, 15, 15 };
                    for (int i = 0; i < colWidths.Length && i < DGVSaleDetails.Columns.Count; i++)
                    {
                        ((Excel.Range)sheet.Columns[i + 1]).ColumnWidth = colWidths[i];
                    }

                    // Save file
                    sheet.SaveAs(fileName);
                    excelApp.Quit();

                    excelCounter++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();
                // Reset row index at the start of printing
                currentRowIndex = 0;

                // Set page settings to match your PDF format
                printDoc.DefaultPageSettings.Landscape = true;
                printDoc.DefaultPageSettings.Margins = new Margins(25, 25, 25, 25); // 0.25 inches in hundredths

                // Set paper size to Tabloid (11x17 inches)
                bool tabloidFound = false;
                foreach (PaperSize paperSize in printDoc.PrinterSettings.PaperSizes)
                {
                    if (paperSize.PaperName == "Tabloid")
                    {
                        printDoc.DefaultPageSettings.PaperSize = paperSize;
                        tabloidFound = true;
                        break;
                    }
                }

                // If Tabloid not found, create custom paper size
                if (!tabloidFound)
                {
                    printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 1100, 1700);
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
                PrintDialog printDialog = new PrintDialog
                {
                    Document = printDoc
                };

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Calculate printable area (in hundredths of inches)
            float printableWidth = e.MarginBounds.Width;
            float printableHeight = e.MarginBounds.Height;

            // Define heights in hundredths of inches (converting from points)
            float titleHeight = (40f * 100f) / 72f; // 40 points to hundredths of inches
            float rowHeight = (40f * 100f) / 72f;   // 40 points to hundredths of inches

            // Calculate available space for rows (subtract title and header)
            float availableSpaceForRows = printableHeight - titleHeight - rowHeight;

            // Calculate how many rows can fit on this page
            int rowsPerPage = (int)(availableSpaceForRows / rowHeight);
            if (rowsPerPage < 1) rowsPerPage = 1; // Ensure at least one row per page

            // Determine the last row to print on this page
            int lastRow = Math.Min(currentRowIndex + rowsPerPage, DGVSaleDetails.Rows.Count);

            // Create fonts to match PDF
            System.Drawing.Font titleFont = new System.Drawing.Font("Helvetica", 8, FontStyle.Regular);
            System.Drawing.Font headerFont = new System.Drawing.Font("Helvetica", 8, FontStyle.Regular);
            System.Drawing.Font rowFont = new System.Drawing.Font("Helvetica", 10, FontStyle.Regular);

            // Create brushes with exact colors to match PDF
            Brush darkGrayBrush = new SolidBrush(Color.FromArgb(64, 64, 64)); // PDF DARK_GRAY
            Brush lightBlueBrush = new SolidBrush(Color.FromArgb(173, 216, 230)); // PDF light blue
            Brush bisqueBrush = new SolidBrush(Color.FromArgb(255, 228, 196)); // PDF bisque
            Brush whiteBrush = Brushes.White;
            Brush blackBrush = Brushes.Black;
            Pen blackPen = Pens.Black;

            // Draw title only on the first page
            if (currentRowIndex == 0)
            {
                StringFormat titleFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                RectangleF titleRect = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, printableWidth, titleHeight);
                e.Graphics.DrawString("All Sale Details", titleFont, blackBrush, titleRect, titleFormat);
            }

            // Calculate column widths (same as before)
            float[] columnWidthPercentages = new float[DGVSaleDetails.ColumnCount];
            // Column width configuration for 14 columns
            if (DGVSaleDetails.ColumnCount >= 1) columnWidthPercentages[0] = 3f;   // 5% for column 1
            if (DGVSaleDetails.ColumnCount >= 2) columnWidthPercentages[1] = 7f;  // 10% for column 2
            if (DGVSaleDetails.ColumnCount >= 3) columnWidthPercentages[2] = 3f;  // 10% for column 3
            if (DGVSaleDetails.ColumnCount >= 4) columnWidthPercentages[3] = 9f;   // 8% for column 4
            if (DGVSaleDetails.ColumnCount >= 5) columnWidthPercentages[4] = 4f;   // 8% for column 5
            if (DGVSaleDetails.ColumnCount >= 6) columnWidthPercentages[5] = 4f;   // 7% for column 6
            if (DGVSaleDetails.ColumnCount >= 7) columnWidthPercentages[6] = 3f;   // 7% for column 7
            if (DGVSaleDetails.ColumnCount >= 8) columnWidthPercentages[7] = 3f;   // 7% for column 8
            if (DGVSaleDetails.ColumnCount >= 9) columnWidthPercentages[8] = 3f;   // 6% for column 9
            if (DGVSaleDetails.ColumnCount >= 10) columnWidthPercentages[9] = 3f;  // 6% for column 10
            if (DGVSaleDetails.ColumnCount >= 11) columnWidthPercentages[10] = 4f; // 5% for column 11
            if (DGVSaleDetails.ColumnCount >= 12) columnWidthPercentages[11] = 4f; // 5% for column 12
            if (DGVSaleDetails.ColumnCount >= 13) columnWidthPercentages[12] = 4f; // 5% for column 13
            if (DGVSaleDetails.ColumnCount >= 14) columnWidthPercentages[13] = 4f; // 5% for column 14
            if (DGVSaleDetails.ColumnCount >= 15) columnWidthPercentages[14] = 4f;   // 8% for column 5
            if (DGVSaleDetails.ColumnCount >= 16) columnWidthPercentages[15] = 4f;   // 7% for column 6
            if (DGVSaleDetails.ColumnCount >= 17) columnWidthPercentages[16] = 4f;   // 7% for column 7
            if (DGVSaleDetails.ColumnCount >= 18) columnWidthPercentages[17] = 4f;   // 7% for column 8
            if (DGVSaleDetails.ColumnCount >= 19) columnWidthPercentages[18] = 4f;   // 6% for column 9
            if (DGVSaleDetails.ColumnCount >= 20) columnWidthPercentages[19] = 4f;  // 6% for column 10
            if (DGVSaleDetails.ColumnCount >= 21) columnWidthPercentages[20] = 6f; // 5% for column 11
            if (DGVSaleDetails.ColumnCount >= 22) columnWidthPercentages[21] = 4f; // 5% for column 12
            if (DGVSaleDetails.ColumnCount >= 23) columnWidthPercentages[22] = 4f; // 5% for column 13
            if (DGVSaleDetails.ColumnCount >= 24) columnWidthPercentages[23] = 4f; // 5% for column 14

            // Make sure the total adds up to 100% (currently sums to 100%)
            float totalPercentage = columnWidthPercentages.Sum();
            if (totalPercentage != 100f)
            {
                for (int i = 0; i < columnWidthPercentages.Length; i++)
                {
                    columnWidthPercentages[i] = (columnWidthPercentages[i] / totalPercentage) * 100f;
                }
            }

            // Calculate actual column widths in points
            float[] columnWidths = new float[DGVSaleDetails.ColumnCount];
            for (int i = 0; i < DGVSaleDetails.ColumnCount; i++)
            {
                columnWidths[i] = (columnWidthPercentages[i] / 100f) * printableWidth;
            }

            // Set starting Y position
            float currentY = e.MarginBounds.Top;

            // Add title height if on first page
            if (currentRowIndex == 0)
            {
                currentY += titleHeight;
            }

            // Draw table headers on every page
            float currentX = e.MarginBounds.Left;
            for (int i = 0; i < DGVSaleDetails.ColumnCount; i++)
            {
                RectangleF cellRect = new RectangleF(currentX, currentY, columnWidths[i], rowHeight);

                // Fill background for header
                e.Graphics.FillRectangle(darkGrayBrush, cellRect);

                // Draw text
                StringFormat cellFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(DGVSaleDetails.Columns[i].HeaderText, headerFont, whiteBrush, cellRect, cellFormat);

                // Draw border
                e.Graphics.DrawRectangle(blackPen, cellRect.Left, cellRect.Top, cellRect.Width, cellRect.Height);
                currentX += columnWidths[i];
            }

            // Move to first row position
            currentY += rowHeight;

            // Draw rows for the current page
            for (int i = currentRowIndex; i < lastRow; i++)
            {
                currentX = e.MarginBounds.Left; // Reset X position for each row

                for (int j = 0; j < DGVSaleDetails.ColumnCount; j++)
                {
                    RectangleF cellRect = new RectangleF(currentX, currentY, columnWidths[j], rowHeight);

                    // Alternate row colors
                    if (i % 2 == 0)
                        e.Graphics.FillRectangle(lightBlueBrush, cellRect);
                    else
                        e.Graphics.FillRectangle(bisqueBrush, cellRect);

                    // Draw text
                    StringFormat cellFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(DGVSaleDetails.Rows[i].Cells[j].Value?.ToString() ?? "", rowFont, blackBrush, cellRect, cellFormat);

                    // Draw border
                    e.Graphics.DrawRectangle(blackPen, cellRect.Left, cellRect.Top, cellRect.Width, cellRect.Height);
                    currentX += columnWidths[j];
                }
                currentY += rowHeight;
            }

            // Update current row index for next page
            currentRowIndex = lastRow;

            // Check if there are more rows to print
            if (currentRowIndex < DGVSaleDetails.Rows.Count)
            {
                e.HasMorePages = true; // There are more rows to print
            }
            else
            {
                e.HasMorePages = false; // All rows have been printed
                currentRowIndex = 0; // Reset for next print job
            }

            // Dispose custom resources
            darkGrayBrush.Dispose();
            lightBlueBrush.Dispose();
            bisqueBrush.Dispose();
            titleFont.Dispose();
            headerFont.Dispose();
            rowFont.Dispose();
        }

    }
}
