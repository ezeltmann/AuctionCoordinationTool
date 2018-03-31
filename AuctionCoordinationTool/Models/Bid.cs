using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class Bid
    {
        public int BidId { get; set; }
        public int PaddleId { get; set; }
        public int DonationId { get; set; }
        public int Units { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
