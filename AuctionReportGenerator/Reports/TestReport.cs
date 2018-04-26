using System;
using System.Collections.Generic;
using System.Text;
using AuctionReportGenerator.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace AuctionReportGenerator.Reports
{
    public class TestReport : IAuctionReport
    {
        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("TestReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);


            var title = new Paragraph();
            title.SetFontSize(30)
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                 .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                 .Add(new Text("Outstanding Balance Report"));

            var subtitle = new Paragraph().SetFontSize(20)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                        .Add(new Text("Report Time: " + DateTime.Now.ToShortDateString()));

            var table = new Table(4);
            
            table.AddHeaderCell(new Cell().Add(new Paragraph("Name").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Bidding Total").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Paid").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Outstanding").SetFont(font).SetPadding(1)));

            table.AddCell(new Cell().Add(new Paragraph("Test Person").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$20.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$10.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$10.00").SetFont(font).SetPadding(1)));

            table.AddCell(new Cell().Add(new Paragraph("Test Person 2").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$40.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$30.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$10.00").SetFont(font).SetPadding(1)));

            table.AddCell(new Cell().Add(new Paragraph("Other Person").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$20.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$0.00").SetFont(font).SetPadding(1)));
            table.AddCell(new Cell().Add(new Paragraph("$20.00").SetFont(font).SetPadding(1)));

            table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            document.Add(title);
            document.Add(subtitle);
            document.Add(table);
            document.Add(new Paragraph("Hello World!"));
            document.Close();
        }
    }
}
