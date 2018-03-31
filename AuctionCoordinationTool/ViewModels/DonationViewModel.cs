using AuctionCoordinationTool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCoordinationTool.ViewModels
{
    public class DonationViewModel : Donation
    {
        public IEnumerable<SelectListItem> Donors { get; set; }
        public int SelectedDonorId { get; set; }
        
        public Donation GetDonation()
        {
            Donation result = new Donation();
            result.DateOfEvent = this.DateOfEvent;
            result.Description = this.Description;
            result.DonationID = this.DonationID;
            result.DonorID = this.DonorID;
            result.EstimatedValue = this.EstimatedValue;
            result.PotentialTaxBreak = this.PotentialTaxBreak;
            result.RainDate = this.RainDate;
            result.SuggestedStartingBid = this.SuggestedStartingBid;
            result.Title = this.Title;
            result.UnitsOffered = this.UnitsOffered;

            return result;
        }
    }
}
