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
    public class DonorInfoReport : IAuctionReport
    {
        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("DonorReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            var donations = context.Donation.ToList();


            int count = 0;

            foreach (var donation in donations)
            {
                if (count > 0)
                    document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));

                var donor = context.Donor.Where(o => o.DonorID == donation.DonorID).FirstOrDefault();
                if (donor is null) continue;

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Donation: " + donation.FullTitle);
                sb.AppendLine("Donor: " + donor.FullID);
                sb.AppendLine("Description: ");
                sb.AppendLine(donation.Description);
                sb.AppendLine();

                var header = new Paragraph();
                header.SetFontSize(16)
                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                     .Add(new Text(sb.ToString()));

                sb.Clear();

                var table = new Table(5);

                table.AddHeaderCell(new Cell().Add(new Paragraph("Primary Bidder").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Secondary Bidder").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Phone Number").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Email Address").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Unit Count").SetFont(font).SetPadding(1)));

                var bids = context.Bid.Where(o => o.DonationId == donation.DonationID).ToList();

                foreach (var bid in bids)
                {
                    var bidder = context.Bidder.Where(o => o.BidderId == context.Paddle.Where(p => p.PaddleId == bid.PaddleId).First().BidderId).First();

                    if (!bid.IsGuestPass)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(bidder.FullName).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bidder.SecondaryFirstName + " " + bidder.SecondaryLastName).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bidder.PhoneNumber).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bidder.EmailAddress).SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bid.Units.ToString()).SetFont(font).SetPadding(1)));
                    }
                    else
                    {
                        table.AddCell(new Cell().Add(new Paragraph("Guest Pass").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph("").SetFont(font).SetPadding(1)));
                        table.AddCell(new Cell().Add(new Paragraph(bid.Units.ToString()).SetFont(font).SetPadding(1)));
                    }
                }

                document.Add(header);
                document.Add(table);

                count++;
            }

            document.Close();

        }
    }
}
