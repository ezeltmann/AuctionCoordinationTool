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

        [Display(Name = "Amount Due")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountDue { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountPaid {get; set;}

        [BindNever]
        public bool IsPaid
        {
            get
            {
                return (this.AmountPaid >= this.AmountDue);
            }
        }
    }
}
