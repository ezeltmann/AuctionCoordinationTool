using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace AuctionCoordinationTool.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [ForeignKey("Bidder")]
        [Display(Name = "Bidder")]
        public int BidderId { get; set; }

        [ForeignKey("AuctionTickets")]
        [Display(Name = "Ticket Type")]
        public int TicketTypeId { get; set; }

        [Display(Name = "Count")]
        public int Count { get; set; }

    }
}
