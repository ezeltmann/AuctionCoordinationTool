using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    public class TicketLineItem
    {
        public string Description { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal CostPerUnit { get; set; }
        
        public decimal Subtotal
        {
            get
            {
                return this.CostPerUnit * this.Quantity;
            }
        }
    }
}
