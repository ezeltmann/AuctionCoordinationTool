using System;
using System.Collections.Generic;
using System.IO;
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
        private const string CSVFormat = "{0},{1},{2},{3},{4}";

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

            var table = new Table(5);

            table.AddHeaderCell(new Cell().Add(new Paragraph("Paddle Number").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Name").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Bidding Total").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Paid").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Outstanding").SetFont(font).SetPadding(1)));

            using (TextWriter tw = new StreamWriter("GrandTotalReport.csv"))
            {
                tw.WriteLine("Paddle Number,Name,Bidding Total,Amount Paid,Amount Outstanding");

                var bidderList = context.Bidder.ToList();
                var paddleNumber = context.Paddle.ToDictionary(o => o.BidderId);
                var bidList = context.Bid.ToList();

                decimal bidTotal = 0;
                decimal pdTotal = 0;
                decimal amtOTotal = 0;


                foreach (Bidder bidder in bidderList)
                {
                    if (!paddleNumber.ContainsKey(bidder.BidderId)) continue; //If they don't have a paddle, we skip them (no bids)

                    var paddle = paddleNumber[bidder.BidderId];
                    var bids = bidList.Where(o => o.PaddleId == paddle.PaddleId).ToList();

                    var tmpTotal = bids.Sum(o => o.TotalCost);
                    var amtOutstanding = tmpTotal - bidder.AmountPaid;
                    if (amtOutstanding < 0) amtOutstanding = 0;

                    bidTotal += tmpTotal;
                    pdTotal += bidder.AmountPaid;
                    amtOTotal += amtOutstanding;

                    table.AddCell(new Cell().Add(new Paragraph(paddle.PaddleNumber.ToString()).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(bidder.FullName).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(tmpTotal.ToString("C2")).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(bidder.AmountPaid.ToString("C2")).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(amtOutstanding.ToString("C2")).SetFont(font).SetPadding(1)));

                    tw.WriteLine(String.Format(CSVFormat
                                , paddle.PaddleNumber.ToString()
                                , bidder.FullName.Replace(",", " ", StringComparison.CurrentCulture)
                                , tmpTotal
                                , bidder.AmountPaid
                                , amtOutstanding));


                }

                table.AddCell(new Cell().Add(new Paragraph(" ").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("Grand Totals:").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(bidTotal.ToString("C2")).SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(pdTotal.ToString("C2")).SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(amtOTotal.ToString("C2")).SetFont(font).SetPadding(1)));

                tw.Flush();
                tw.Close();
            }

            document.Add(title);
            document.Add(subtitle);
            document.Add(grandTotal);
            document.Add(paidTotal);
            document.Add(table);
            document.Close();
        }
    }
}
