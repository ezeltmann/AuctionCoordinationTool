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
        public async Task<IActionResult> Create([Bind("BidderId,MemberOrFriend,PrimaryFirstName,PrimaryLastName,SecondaryFirstName,SecondaryLastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,EmailAddress")] Bidder bidder)
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
        public async Task<IActionResult> Edit(int id, [Bind("BidderId,MemberOrFriend,PrimaryFirstName,PrimaryLastName,SecondaryFirstName,SecondaryLastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,EmailAddress")] Bidder bidder)
        {
            if (id != bidder.BidderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
    }
}
