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
    public class BidsController : Controller
    {
        private readonly AuctionDBContext _context;

        public BidsController(AuctionDBContext context)
        {
            _context = context;
        }

        // GET: Bids
        public async Task<IActionResult> Index()
        {
            ViewBag.Paddles = _context.Paddle.ToList().ToDictionary(o => o.PaddleId);
            ViewBag.Donations = _context.Donation.ToList().ToDictionary(o => o.DonationID);

            return View(await _context.Bid.ToListAsync());
        }

        // GET: Bids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bid
                .SingleOrDefaultAsync(m => m.BidId == id);
            if (bid == null)
            {
                return NotFound();
            }

            ViewBag.Paddle = await _context.Paddle.SingleOrDefaultAsync(o => o.PaddleId == bid.PaddleId);
            ViewBag.Donation = await _context.Donation.SingleOrDefaultAsync(o => o.DonationID == bid.DonationId);

            return View(bid);
        }

        // GET: Bids/Create
        public IActionResult Create()
        {
            ViewBag.Paddles = new SelectList(_context.Paddle.ToList(), "PaddleId", "PaddleNumber");
            ViewBag.Donations = new SelectList(_context.Donation.ToList(), "DonationID", "Title");
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BidId,PaddleId,DonationId,Units,CostPerUnit,IsGuestPass")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Paddles = new SelectList(_context.Paddle.ToList(), "PaddleId", "PaddleNumber");
                ViewBag.Donations = new SelectList(_context.Donation.ToList(), "DonationID", "Title");
                return View(bid);
            }
            
        }

        public IActionResult EnterSheet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation =  _context.Donation.SingleOrDefault(m => m.DonationID == id);
            if (donation == null)
            {
                return NotFound();
            }

            List<Bid> bids = _context.Bid.Where(m => m.DonationId == id).ToList();
            if (bids == null || bids.Count < 1) bids = new List<Bid>() { new Bid() { DonationId = id.Value } };

            ViewBag.DonationId = id;
            ViewBag.Paddles = _context.Paddle.ToList();

            return View(bids);
        }

        [HttpPost]
        public async Task<IActionResult> EnterSheet(List<Bid> bids)
        {
            var id = bids[0]?.DonationId;

            if (ModelState.IsValid)
            {
                                
                foreach (Bid bid in bids)
                {
                    try
                    {
                        if (bid.BidId <= 0)
                            _context.Add(bid);
                        else _context.Update(bid);

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BidExists(bid.BidId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                ViewBag.Message = "Data Successfully Saved";
                ModelState.Clear();

                bids = _context.Bid.Where(m => m.DonationId == id.Value).ToList();
                if (bids == null || bids.Count < 1) bids = new List<Bid>() { new Bid() { DonationId = id.Value } };

            }

            if (!id.HasValue) return NotFound();

            ViewBag.DonationId = id.Value;
            ViewBag.Paddles = _context.Paddle.ToList();

            return View(bids);
        }

        public List<Paddle> GetPaddles()
        {
            return _context.Paddle.ToList();
        }

        // GET: Bids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bid.SingleOrDefaultAsync(m => m.BidId == id);
            if (bid == null)
            {
                return NotFound();
            }

            ViewBag.Paddles = new SelectList(_context.Paddle.ToList(), "PaddleId", "PaddleNumber");
            ViewBag.Donations = new SelectList(_context.Donation.ToList(), "DonationID", "Title");

            return View(bid);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BidId,PaddleId,DonationId,Units,CostPerUnit,IsGuestPass")] Bid bid)
        {
            if (id != bid.BidId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidExists(bid.BidId))
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
                ViewBag.Paddles = new SelectList(_context.Paddle.ToList(), "PaddleId", "PaddleNumber");
                ViewBag.Donations = new SelectList(_context.Donation.ToList(), "DonationID", "Title");

                return View(bid);
            }
        }

        // GET: Bids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bid
                .SingleOrDefaultAsync(m => m.BidId == id);
            if (bid == null)
            {
                return NotFound();
            }

            ViewBag.Paddle = await _context.Paddle.SingleOrDefaultAsync(o => o.PaddleId == bid.PaddleId);
            ViewBag.Donation = await _context.Donation.SingleOrDefaultAsync(o => o.DonationID == bid.DonationId);

            return View(bid);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bid = await _context.Bid.SingleOrDefaultAsync(m => m.BidId == id);
            _context.Bid.Remove(bid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidExists(int id)
        {
            return _context.Bid.Any(e => e.BidId == id);
        }
    }
}
