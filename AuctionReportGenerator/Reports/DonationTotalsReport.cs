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
    public class DonationsTotalReport : IAuctionReport
    {
        private const string CSVFormat = "{0},{1},{2}";

        public void GenerateReport(AuctionDBContext context)
        {
            var writer = new PdfWriter("DonationsTotalReport.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);


            var title = new Paragraph();
            title.SetFontSize(30)
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                 .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                 .Add(new Text("Donations Total Report"));

            var subtitle = new Paragraph().SetFontSize(15)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                        .Add(new Text("Report Time: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));


            var table = new Table(3);

            table.AddHeaderCell(new Cell().Add(new Paragraph("Donation Item").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Donor Name").SetFont(font).SetPadding(1)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Bidding Total").SetFont(font).SetPadding(1)));

            using (TextWriter tw = new StreamWriter("DonationsTotalReport.csv"))
            {
                tw.WriteLine("Donation Item,Donor Name,Bidding Total");

                var donations = context.Donation.ToList();
                
                decimal bidTotal = 0;
                
                foreach (Donation donation in donations)
                {
                    decimal tmpTotal = context.Bid.Where(o => o.DonationId == donation.DonationID).Sum(m => m.TotalCost);
                    string donorName = context.Donor.Where(o => o.DonorID == donation.DonorID).First().FullID;

                    table.AddCell(new Cell().Add(new Paragraph(donation.FullTitle).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(donorName).SetFont(font).SetPadding(1)));
                    table.AddCell(new Cell().Add(new Paragraph(tmpTotal.ToString("C2")).SetFont(font).SetPadding(1)));

                    tw.WriteLine(String.Format(CSVFormat
                                , donation.FullTitle.Replace(",", " ", StringComparison.CurrentCulture)
                                , donorName.Replace(",", " ", StringComparison.CurrentCulture)
                                , tmpTotal
                                ));

                    bidTotal += tmpTotal;

                }

                table.AddCell(new Cell().Add(new Paragraph(" ").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph("Grand Totals:").SetFont(font).SetPadding(1)));
                table.AddCell(new Cell().Add(new Paragraph(bidTotal.ToString("C2")).SetFont(font).SetPadding(1)));

                tw.Flush();
                tw.Close();
            }

            document.Add(title);
            document.Add(subtitle);
            document.Add(table);
            document.Close();
        }
    }
}
