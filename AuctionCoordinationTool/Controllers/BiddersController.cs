using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionCoordinationTool.Models;

namespace AuctionCoordinationTool.Controllers
{
    public class BiddersController : Controller
    {
        private readonly AuctionDBContext _context;

        public BiddersController(AuctionDBContext context)
        {
            _context = context;
        }

        // GET: Bidders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bidder.ToListAsync());
        }

        // GET: Bidders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidder = await _context.Bidder
                .SingleOrDefaultAsync(m => m.BidderId == id);
            if (bidder == null)
            {
                return NotFound();
            }

            var paddles = await _context.Paddle.Where(o => o.BidderId == bidder.BidderId).ToListAsync();

            if (paddles.Count > 0)
            {
                ViewBag.PaddleNumbers = paddles.Select(o => o.PaddleNumber.ToString()).Aggregate((tl, ni) => tl += ni + " ");

                var paddleIds = paddles.Select(a => a.PaddleId).ToList();
                var bids = await _context.Bid.Where(o => paddleIds.Contains(o.PaddleId)).ToListAsync();

                ViewBag.Bids = bids;
                ViewBag.TotalAmount = String.Format("{0:C}", bids.Select(o => o.TotalCost).Sum());
                ViewBag.Donations = await _context.Donation.Where(o => bids.Select(a => a.DonationId).Contains(o.DonationID)).ToDictionaryAsync(e => e.DonationID);
            }
            else
            {
                ViewBag.PaddleNumbers = null;
                ViewBag.Bids = new List<Bid>();
                ViewBag.TotalAmount = "$0.00";
                ViewBag.Donations = new Dictionary<int, Donation>();
            }


            return View(bidder);
        }

        // GET: Bidders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bidders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BidderId,MemberOrFriend,PrimaryFirstName,PrimaryLastName,SecondaryFirstName,SecondaryLastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,EmailAddress,FullyPaid")] Bidder bidder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bidder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bidder);
        }

        // GET: Bidders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidder = await _context.Bidder.SingleOrDefaultAsync(m => m.BidderId == id);
            if (bidder == null)
            {
                return NotFound();
            }
            return View(bidder);
        }

        // POST: Bidders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BidderId,MemberOrFriend,PrimaryFirstName,PrimaryLastName,SecondaryFirstName,SecondaryLastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,EmailAddress,AmountPaid")] Bidder bidder)
        {
            if (id != bidder.BidderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bidder.AmountPaid = 0;
                    _context.Update(bidder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidderExists(bidder.BidderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bidder);
        }

        // GET: Bidders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidder = await _context.Bidder
                .SingleOrDefaultAsync(m => m.BidderId == id);
            if (bidder == null)
            {
                return NotFound();
            }

            return View(bidder);
        }

        // POST: Bidders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bidder = await _context.Bidder.SingleOrDefaultAsync(m => m.BidderId == id);
            _context.Bidder.Remove(bidder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidderExists(int id)
        {
            return _context.Bidder.Any(e => e.BidderId == id);
        }

        // GET: Bidders/CheckOut/5
        public async Task<IActionResult> CheckOut(int? id)
        {

            //TODO -- Checkout Logic
            if (id == null)
            {
                return NotFound();
            }

            
            var bidder = await _context.Bidder
                .SingleOrDefaultAsync(m => m.BidderId == id);

            if (bidder == null)
            {
                return NotFound();
            }

            var paddles = await _context.Paddle.Where(o => o.BidderId == bidder.BidderId).ToListAsync();

            if (paddles.Count > 0)
            {
                var paddleIds = paddles.Select(a => a.PaddleId).ToList();
                var bids = await _context.Bid.Where(o => paddleIds.Contains(o.PaddleId)).ToListAsync();

                var chkout = new CheckOut
                {
                    BidderId = bidder.BidderId,
                    AmountOwed = bids.Select(o => o.TotalCost).Sum(),
                    TotalPaid = bidder.AmountPaid,
                    PaymentInfo = bidder.PaymentInfo
                };

                if (chkout.AmountOwed <= 0)
                {
                    var chkErr = new CheckOutError
                    {
                        BidderId = bidder.BidderId,
                        BidderFullName = bidder.FullName,
                        ErrorMessage = "Bidder does owe anything."
                    };

                    return View("CheckOutError", chkErr);
                }


                ViewBag.Bids = bids;
                ViewBag.TotalAmount = String.Format("{0:C}", chkout.AmountOwed);
                ViewBag.Donations = await _context.Donation.Where(o => bids.Select(a => a.DonationId).Contains(o.DonationID)).ToDictionaryAsync(e => e.DonationID);

                return View(chkout);
            }
            else
            {
                var chkErr = new CheckOutError
                {
                    BidderId = bidder.BidderId,
                    BidderFullName = bidder.FullName,
                    ErrorMessage = "Bidder does not have any paddles, cannot bid or check-out."
                };

                return View("CheckOutError", chkErr);
            }

            
        }

        // POST: Bidders/CheckOut/5
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutConfirmed(int id, [Bind("BidderId,TotalPaid,AmountOwed,PaymentInfo")] CheckOut result)
        {
            //var bidder = await _context.Bidder.SingleOrDefaultAsync(m => m.BidderId == id);
            //_context.Bidder.Remove(bidder);
            //await _context.SaveChangesAsync();
            //return View(result);


            if (ModelState.IsValid)
            {
                var bidder = await _context.Bidder.SingleOrDefaultAsync(m => m.BidderId == id);

                try
                {
                    bidder.AmountPaid = result.TotalPaid;
                    bidder.PaymentInfo = result.PaymentInfo;
                    _context.Update(bidder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidderExists(bidder.BidderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var bidder = await _context.Bidder.SingleOrDefaultAsync(m => m.BidderId == id);

                var chkErr = new CheckOutError
                {
                    BidderId = bidder.BidderId,
                    BidderFullName = bidder.FullName,
                    ErrorMessage = "Unknown Error, could not check out Bidder.  You shouldn't be seeing this."
                };

                return View("CheckOutError", chkErr);

            }
        }
    }
}
