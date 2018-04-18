using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class CheckOutError
    {
        public int BidderId { get; set; }
        public string BidderFullName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
