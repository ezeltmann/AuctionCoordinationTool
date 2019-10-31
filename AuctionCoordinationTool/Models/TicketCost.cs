using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class TicketCost
    {
        public decimal AdultTicketCost { get; set; }
        public decimal AdultDoorTicketCost { get; set; }
        public decimal ChildTicketCost { get; set; }
        public decimal ChildDoorTicketCost { get; set; }
    }
}
