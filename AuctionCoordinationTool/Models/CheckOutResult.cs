using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class CheckOutResult
    {

        decimal TotalPaid { get; set; }
        decimal AmountOwed { get; set; }

        bool PaidInFull
        {
            get
            {
                return (TotalPaid >= AmountOwed);
            }
        }
    }
}
