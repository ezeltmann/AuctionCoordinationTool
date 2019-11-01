using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    
    public class AuctionTickets
    {
        [Key]
        public int TicketTypeId { get; set; }

        [Required]
        [Display(Name = "Ticket Code")]
        public string Code { get; set; }

        [Display(Name ="Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Cost Per Unit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CostPerUnit { get; set; }

    }
}
