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
                Console.WriteLine("Hello World!");
                IAuctionReport report = new GrandTotalsReport();

                report.GenerateReport(_context);
            }
        }
    }
}
