using AuctionReportGenerator.Models;
using AuctionReportGenerator.Reports;
using System;

namespace AuctionReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (AuctionDBContext _context = new AuctionDBContext())
            {
                IAuctionReport report = new GrandTotalsReport();
                report.GenerateReport(_context);

                report = new OutstandingBalanceReport();
                report.GenerateReport(_context);

                report = new DonorInfoReport();
                report.GenerateReport(_context);

                report = new GuestPassReport();
                report.GenerateReport(_context);

                report = new BidderReceiptReport();
                report.GenerateReport(_context);

            }
        }
    }
}
