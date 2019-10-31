using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class TicketSummary
    {
        public Ticket Ticket { get; set; }
        public TicketCost TicketCost { get; set; }
    }
}
