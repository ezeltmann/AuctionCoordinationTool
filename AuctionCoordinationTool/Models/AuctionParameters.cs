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
    //[ModelBinder(BinderType = typeof(DonationBinder))]
    public class AuctionParameters
    {
        [Key]
        public int ParameterID { get; set; }

        [Required]
        [Display(Name = "Parameter Code")]
        public string Code { get; set; }

        [Display(Name ="Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Value Text")]
        public string ValueText { get; set; }

        [Display(Name = "Value Int")]
        public int ValueInt { get; set; }

        [Display(Name = "Value Decimal")]
        public decimal ValueDec { get; set; }
    }
}
