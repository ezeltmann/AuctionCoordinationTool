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
    public class OutstandingBalanceReport : IAuctionReport
    {
        private const string CSVFormat = "{0},{1},{2},{3},{4}";

        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("OutstandingBalance.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            

            var title = new Paragraph();
            title.SetFontSize(30)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .Add(new Text("Outstanding Balance Report"));

            var subtitle = new Paragraph().SetFontSize(15)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                        .Add(new Text("Report Time: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

            var table = new Table(5);

            table.AddHeaderCell(new Cell().Add(new Paragraph("Paddle Number").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Name").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Bidding Total").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Paid").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Amount Outstanding").SetFont(font).SetPadding(1)));

            var infoList = new List<Bidder>();
            var bidderList = context.Bidder.ToList();
            var paddleNumber = context.Paddle.ToDictionary(o => o.BidderId);
            var bidList = context.Bid.ToList();

            using (TextWriter tw = new StreamWriter("OutstandingBalance.csv"))
            {
                tw.WriteLine("Paddle Number,Name,Bidding Total,Amount Paid,Amount Outstanding");



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
                    if (amtOutstanding <= 0) continue;

                    infoList.Add(bidder);

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
            document.Add(table);


            foreach(Bidder bidder in infoList)
            {
                document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));

                var paddle = paddleNumber[bidder.BidderId];
                var bids = bidList.Where(o => o.PaddleId == paddle.PaddleId).ToList();

                var tmpTotal = bids.Sum(o => o.TotalCost);
                var amtOutstanding = tmpTotal - bidder.AmountPaid;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Bidder Name: " + bidder.FullName);
                if (!String.IsNullOrWhiteSpace(bidder.SecondaryFirstName))
                    sb.AppendLine("Second Name: " + bidder.SecondaryFirstName + " " + bidder.SecondaryLastName);
                sb.AppendLine("Paddle Number: " + paddle.PaddleNumber.ToString());
                sb.AppendLine("Amount Bid: " + tmpTotal.ToString("C2"));
                sb.AppendLine("Amount Paid: " + bidder.AmountPaid.ToString("C2"));
                sb.AppendLine("Amount Owed: " + amtOutstanding.ToString("C2"));
                sb.AppendLine();
                sb.AppendLine("Phone Number: " + bidder.PhoneNumber);
                sb.AppendLine("Email Address: " + bidder.EmailAddress);
                sb.AppendLine("Address: ");
                sb.AppendLine(bidder.AddressLine1);
                sb.AppendLine(bidder.AddressLine2);
                sb.AppendFormat("{0}, {1} {2}", bidder.City, bidder.State, bidder.ZipCode);
                sb.AppendLine();


                var bidderInfo = new Paragraph();
                bidderInfo.SetFontSize(12)
                          .Add(new Text(sb.ToString()));

                document.Add(bidderInfo);
                
            }


            document.Close();
            
        }
    }
}
