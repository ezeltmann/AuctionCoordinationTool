//using AuctionCoordinationTool.Models;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AuctionCoordinationTool.ModelBinders
//{
//    public class DonationBinder : IModelBinder
//    {
//        private readonly AuctionDBContext _db;

//        public DonationBinder(AuctionDBContext db)
//        {
//            _db = db;
//        }

//        public Task BindModelAsync(ModelBindingContext bindingContext)
//        {
//            if (bindingContext == null)
//            {
//                throw new ArgumentNullException(nameof(bindingContext));
//            }

//            Donation result = new Donation();           
            
//            var vpr0 = bindingContext.ValueProvider.GetValue("Donor");

//            var value = vpr0.FirstValue;

//            int id = 0;
//            if (!int.TryParse(value, out id))
//            {
//                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Donor Id must be an integer.");
//                return Task.CompletedTask;
//            }


//            result.DonorID = _db.Donor.Find(id);

//            result.Title = bindingContext.ValueProvider.GetValue("Title").FirstValue;
//            result.Description = bindingContext.ValueProvider.GetValue("Description").FirstValue;
//            result.EstimatedValue = decimal.Parse(bindingContext.ValueProvider.GetValue("EstimatedValue").FirstValue);
//            result.SuggestedStartingBid = decimal.Parse(bindingContext.ValueProvider.GetValue("SuggestedStartingBid").FirstValue);
//            //result.



//            //var vpr3 = bindingContext.BindingSource



//            Donation temp = new Donation();

//            bindingContext.Result = ModelBindingResult.Success(temp);
//            return Task.CompletedTask;
//        }
//    }
//}
