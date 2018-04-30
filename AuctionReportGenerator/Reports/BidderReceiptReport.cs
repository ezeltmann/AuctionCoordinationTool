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
    public class BidderReceiptReport : IAuctionReport
    {
        public const string DISCLAIMER = "Thank You for participating and contributing to this year’s UUCY Auction." +
                                         " This statement is your record of items that you won the bid for at the Auction. " +
                                         "UUCY makes no claim regarding the tax status of your donations for this Auction. " +
                                         "Please consult with your Tax Advisor as to whether any of your Auction donations are tax deductible. " +
                                         "This will be your only statement from UUCY regarding the 2018 Auction.";

        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("BidderReceiptReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            var bidders = context.Bidder.ToList();

            int count = 0;

            foreach (var bidder in bidders)
            {
                var paddle = context.Paddle.Where(o => o.BidderId == bidder.BidderId).FirstOrDefault();
                if (paddle == null) continue;  //No Paddle, No receipt

                var bids = context.Bid.Where(o => o.PaddleId == paddle.PaddleId).ToList();
                if (bids.Count <= 0) continue; //No Bids, No receipt

                if (count > 0)
                    document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));

                var subtitle = new Paragraph().SetFontSize(15)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .Add(new Text("Bidder Receipt"));

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Primary Bidder: " + bidder.FullName);
                sb.AppendLine("Secondary Bidder: " + bidder.SecondaryFirstName + " " + bidder.SecondaryLastName);
                sb.AppendLine("Paddle Number: " + paddle.PaddleNumber);
                sb.AppendLine();

                var header = new Paragraph();
                header.SetFontSize(16)
                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                     .Add(new Text(sb.ToString()));


                var table = new Table(7);

                table.AddHeaderCell(new Cell().Add(new Paragraph("Item Name").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Donor Name").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Donor Phone").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Donor Email").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Cost Per Unit").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Unit Count").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total Cost").SetFont(font).SetPadding(1)));

                decimal totalCost = 0;


                // Info Lines
                foreach (var bid in bids)
                {
                    var donation = context.Donation.Where(o => o.DonationID == bid.DonationId).First();
                    var donor = context.Donor.Where(o => o.DonorID == donation.DonorID).First();

                    try
                    {

                        table.AddCell(new Cell().Add(new Paragraph(donation.FullTitle).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(donor.FullID).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(donor.PhoneNumber ?? "").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(donor.EmailAddress ?? "").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bid.CostPerUnit.ToString("C2")).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bid.Units.ToString()).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bid.TotalCost.ToString("C2")).SetFont(font).SetPadding(1)));

                        totalCost += bid.TotalCost;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                        if (e.InnerException != null)
                        {
                            Console.WriteLine(e.InnerException.Message);
                            Console.WriteLine(e.InnerException.StackTrace);
                        }

                        Console.WriteLine("Donor: {0},{1},{2}", donor.FullID, donor.PhoneNumber, donor.EmailAddress);
                        Console.WriteLine("Press Any Key To Continue");
                        Console.ReadKey();
                        throw;
                    }
                }

                table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("Total: ").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(totalCost.ToString("C2")).SetFont(font).SetPadding(1)));


                var breaker = new Paragraph();
                breaker.SetFontSize(16)
                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                     .Add(new Text("\n\n"));

                var amtOutstanding = totalCost - bidder.AmountPaid;
                if (amtOutstanding <= 0) amtOutstanding = 0;

                var finalTotal = new Table(3);

                finalTotal.AddHeaderCell(new Cell().Add(new Paragraph("Total Cost").SetFont(font).SetPadding(1)));
                finalTotal.AddHeaderCell(new Cell().Add(new Paragraph("Total Paid").SetFont(font).SetPadding(1)));
                finalTotal.AddHeaderCell(new Cell().Add(new Paragraph("Amount Due").SetFont(font).SetPadding(1)));

                finalTotal.AddCell(new Cell().Add(new Paragraph(totalCost.ToString("C2")).SetFont(font).SetPadding(1)));
                finalTotal.AddCell(new Cell().Add(new Paragraph(bidder.AmountPaid.ToString("C2")).SetFont(font).SetPadding(1)));
                finalTotal.AddCell(new Cell().Add(new Paragraph(amtOutstanding.ToString("C2")).SetFont(font).SetPadding(1)));

                

                var statement = new Paragraph().SetFontSize(12)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .Add(new Text(DISCLAIMER));

                document.Add(header);
                document.Add(table);
                document.Add(breaker);
                document.Add(finalTotal);
                document.Add(statement);

                count++;
            }

            document.Close();
        }
    }
}
