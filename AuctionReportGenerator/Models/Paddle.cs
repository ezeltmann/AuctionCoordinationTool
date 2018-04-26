using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionReportGenerator.Models
{
    public class Paddle
    {
        [Key]
        public int PaddleId { get; set; }

        [ForeignKey("Bidder")]
        [Display(Name = "Bidder")]
        public int BidderId { get; set; }

        [Display(Name = "Paddle Number")]
        public int PaddleNumber { get; set; }
    }
}
