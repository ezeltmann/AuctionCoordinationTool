using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class CheckOut
    {

        public int BidderId { get; set; }

        public decimal TotalPaid { get; set; }
        public decimal AmountOwed { get; set; }

        public decimal TicketOwed { get; set; }

        public string PaymentInfo { get; set; }

        public bool PaidInFull
        {
            get
            {
                return (TotalPaid >= (AmountOwed + TicketOwed));
            }
        }
    }
}
