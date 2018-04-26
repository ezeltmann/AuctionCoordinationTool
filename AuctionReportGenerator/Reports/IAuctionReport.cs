using AuctionReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionReportGenerator.Reports
{
    public interface IAuctionReport
    {
        void GenerateReport(AuctionDBContext context);
    }
}
