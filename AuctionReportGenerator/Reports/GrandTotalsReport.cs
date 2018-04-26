using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuctionReportGenerator.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace AuctionReportGenerator.Reports
{
    public class GrandTotalsReport : IAuctionReport
    {
        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("GrandTotalReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);


            var title = new Paragraph();
            title.SetFontSize(30)
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                 .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                 .Add(new Text("Grand Total Report"));

            var subtitle = new Paragraph().SetFontSize(15)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                        .Add(new Text("Report Time: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

            decimal gtotal = context.Bid.Sum(o => o.TotalCost);

            var grandTotal = new Paragraph().SetFontSize(15)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .Add(new Text("Grand Total Bids: " + gtotal.ToString("C2")));

            decimal pTotal = context.Bidder.Sum(o => o.AmountPaid);

            var paidTotal = new Paragraph().SetFontSize(15)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .Add(new Text("Grand Total Paid Bids: " + pTotal.ToString("C2")));

            document.Add(title);
            document.Add(subtitle);
            document.Add(grandTotal);
            document.Add(paidTotal);
            document.Close();
        }
    }
}
