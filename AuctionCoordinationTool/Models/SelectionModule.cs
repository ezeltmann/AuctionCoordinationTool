using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class SelectionModule
    {
        public int BidderId { get; set; }

        public string BidderName { get; set; }

        public int DonationId { get; set; }

        public string DonationName { get; set; }
    }
}
