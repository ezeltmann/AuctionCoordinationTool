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
    public class GuestPassReport : IAuctionReport
    {
        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("GuestPassReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            var donations = context.Donation.ToList();


            int count = 0;

            foreach (var donation in donations)
            {

                var donor = context.Donor.Where(o => o.DonorID == donation.DonorID).FirstOrDefault();
                if (donor is null) continue;

                int passCount = context.Bid.Where(o => o.DonationId == donation.DonationID).Count(m => m.IsGuestPass);
                if (passCount <= 0) continue;

                if (count > 0)
                    document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Donation: " + donation.FullTitle);
                sb.AppendLine("Donor: " + donor.FullID);
                sb.AppendLine("Donor Phone: " + donor.PhoneNumber);
                sb.AppendLine("Donor Email: " + donor.EmailAddress);
                sb.AppendLine("Description: ");
                sb.AppendLine(donation.Description);
                sb.AppendLine();

                var header = new Paragraph();
                header.SetFontSize(16)
                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                     .Add(new Text(sb.ToString()));

                sb.Clear();

                var table = new Table(2);

                table.AddHeaderCell(new Cell().Add(new Paragraph("Type").SetFont(font).SetPadding(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total Count").SetFont(font).SetPadding(1)));

                var totalGuest = context.Bid.Where(o => o.DonationId == donation.DonationID).Where(m => m.IsGuestPass).Sum(n => n.Units);
                            
                table.AddCell(new Cell().Add(new Paragraph("Guest Passes").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(totalGuest.ToString()).SetFont(font).SetPadding(1)));
                
                document.Add(header);
                document.Add(table);

                count++;
            }

            document.Close();

        }
    }
}
