using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.Models
{
    //[ModelBinder(BinderType = typeof(DonationBinder))]
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [Required]
        [Display(Name = "Donor ID")]
        public int DonorID { get; set; }

        [Display(Name ="Title")]
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Estimated Value")]
        [DataType(DataType.Currency)]        
        public decimal EstimatedValue { get; set; }

        [Display(Name = "Suggested Starting Bid")]
        [DataType(DataType.Currency)]
        public decimal SuggestedStartingBid { get; set; }

        [Required]
        [Display(Name = "Units Offered")]
        public int UnitsOffered { get; set; }

        [Display(Name = "Potential Tax Break?")]
        public bool PotentialTaxBreak { get; set; }

        [Display(Name = "Date Of Event")]
        [DataType(DataType.Date)]        
        public DateTime? DateOfEvent { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Rain Date")]
        public DateTime? RainDate { get; set; }
    }
}
