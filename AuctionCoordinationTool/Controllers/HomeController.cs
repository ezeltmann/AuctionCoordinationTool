using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionCoordinationTool.Models;

namespace AuctionCoordinationTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuctionDBContext _context;

        public HomeController(AuctionDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var GrandTotal = _context.Bid.Sum(m => m.TotalCost);

            ViewBag.GrandTotal = GrandTotal;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public ActionResult ClientFiltering_GetBidders(string term = "")
        {
            var bidders = _context.Bidder.Where(c => c.FullName.ToUpper().Contains(term.ToUpper()))
                                         .Select(c => new { BidderName = c.FullName, BidderId = c.BidderId })
                                         .Distinct().ToList();
            
            return Json(bidders);
        }

        public ActionResult ClientFiltering_GetDonations(string term = "")
        {
            var donations = _context.Donation.Where(c => c.FullTitle.ToUpper().Contains(term.ToUpper()))
                                         .Select(c => new { DonationName = c.FullTitle, DonationId = c.DonationID })
                                         .Distinct().ToList();

            return Json(donations);
        }

        //[{"bidderName":"Test One","bidderId":1},{"bidderName":"New Player Test Item","bidderId":2},{"bidderName":"Test Name5","bidderId":3}]
        [HttpPost]
        public IActionResult Route([Bind("BidderId,BidderName,DonationId,DonationName")] SelectionModule selectionModule)
        {
            if (ModelState.IsValid)
            {
                if (selectionModule.BidderId > 0)
                    return RedirectToAction("Details", "Bidders", new { id = selectionModule.BidderId });

                if (selectionModule.DonationId > 0)
                    return RedirectToAction("Details", "Donations", new { id = selectionModule.DonationId });
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Route_Paddle([Bind("BidderId,BidderName,DonationId,DonationName")] SelectionModule selectionModule)
        {
            if (ModelState.IsValid)
            {
                var pd_count = _context.Paddle.Where(o => o.BidderId == selectionModule.BidderId).Count();

                if (selectionModule.BidderId > 0)
                {
                    if (pd_count > 0)
                    {
                        return RedirectToAction("Details", "Bidders", new { id = selectionModule.BidderId });
                    }
                    else
                    {
                        return RedirectToAction("Create", "Paddles", new { id = selectionModule.BidderId });
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Route_CheckOut([Bind("BidderId,BidderName,DonationId,DonationName")] SelectionModule selectionModule)
        {
            if (ModelState.IsValid)
            {
                if (selectionModule.BidderId > 0)
                    return RedirectToAction("CheckOut", "Bidders", new { id = selectionModule.BidderId });
            }

            return NotFound();

        }

        [HttpPost]
        public IActionResult Route_Details([Bind("BidderId,BidderName,DonationId,DonationName")] SelectionModule selectionModule)
        {
            if (ModelState.IsValid)
            {
                if (selectionModule.DonationId > 0)
                    return RedirectToAction("Details", "Donations", new { id = selectionModule.DonationId });
            }

            return NotFound();

        }

        [HttpPost]
        public IActionResult Route_Runners([Bind("BidderId,BidderName,DonationId,DonationName")] SelectionModule selectionModule)
        {
            if (ModelState.IsValid)
            {
                if (selectionModule.DonationId > 0)
                    return RedirectToAction("EnterSheet", "Bids", new { id = selectionModule.DonationId });
            }

            return NotFound();

        }


    }
}
