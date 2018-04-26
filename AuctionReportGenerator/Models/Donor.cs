using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionReportGenerator.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Repeat Donor?")]
        public bool RepeatDonor { get; set; }

        public string FullID
        {
            get
            {
                return DonorID.ToString() + " - " + FirstName + " " + LastName;
            }
        }
    }
}
