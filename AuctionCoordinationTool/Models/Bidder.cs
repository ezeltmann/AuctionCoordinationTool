using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace AuctionCoordinationTool.Models
{
    public class Bidder
    {
        [Key]        
        public int BidderId { get; set; }

        [Display(Name = "Member or Contributing Friend")]
        public bool MemberOrFriend { get; set; }

        [Display(Name = "First Name")]
        public string PrimaryFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string PrimaryLastName { get; set; }

        [Display(Name = "2nd Person First Name")]
        public string SecondaryFirstName { get; set; }

        [Display(Name = "2nd Person Last Name")]
        public string SecondaryLastName { get; set; }

        [Display(Name = "Street Address")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State Abbrev.")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [BindNever]
        public string FullName
        {
            get
            {
                return this.PrimaryFirstName + " " + this.PrimaryLastName;
            }
        }
    }
}
