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

        [Display(Name = "Adult Count")]
        public int AdultCount { get; set; }

        [Display(Name = "Child Count")]
        public int ChildCount { get; set; }

        [Display(Name = "Adult Count at Door")]
        public int AdultCountDoor { get; set; }

        [Display(Name = "Child Count at Door")]
        public int ChildCountDoor { get; set; }

    }
}
