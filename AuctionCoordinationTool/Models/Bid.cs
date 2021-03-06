﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionCoordinationTool.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }

        [ForeignKey("Paddle")]
        [Display(Name ="Paddle #")]
        public int PaddleId { get; set; }

        [ForeignKey("Donation")]
        [Display(Name = "Donation #")]
        public int DonationId { get; set; }

        [Display(Name = "Number of Units")]
        public int Units { get; set; }

        [Display(Name = "Cost Per Unit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CostPerUnit { get; set; }

        [Display(Name = "Is Guest Pass?")]
        public bool IsGuestPass { get; set; }

        [BindNever]
        [Display(Name = "Total Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCost
        {
            get
            {
                return this.CostPerUnit * this.Units;
            }
        }
    }
}
