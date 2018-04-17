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
    public class DonationsController : Controller
    {
        private readonly AuctionDBContext _context;

        public DonationsController(AuctionDBContext context)
        {
            _context = context;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            ViewBag.Donors = _context.Donor.ToList().ToDictionary(o => o.DonorID);            
            return View(await _context.Donation.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var donation = await _context.Donation
                .SingleOrDefaultAsync(m => m.DonationID == id);


            if (donation == null)
            {
                return NotFound();
            }
            ViewBag.Donor = await _context.Donor.SingleOrDefaultAsync(o => o.DonorID == donation.DonorID);
            var bids = await _context.Bid.Where(o => o.DonationId == donation.DonationID).ToListAsync();
            ViewBag.Bids = bids;
            ViewBag.TotalAmount = String.Format("{0:C}", bids.Select(o => o.TotalCost).Sum());
            ViewBag.Paddles = await _context.Paddle.Where(o => bids.Select(a => a.PaddleId).Contains(o.PaddleId)).ToDictionaryAsync(o => o.PaddleId);
            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {                        
            ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationID,DonorID,Title,Description,EstimatedValue,SuggestedStartingBid,UnitsOffered,PotentialTaxBreak,DateOfEvent,RainDate")] Donation donation)
        {

            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
                return View();
            }
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donation.SingleOrDefaultAsync(m => m.DonationID == id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationID,DonorID,Title,Description,EstimatedValue,SuggestedStartingBid,UnitsOffered,PotentialTaxBreak,DateOfEvent,RainDate")] Donation donation)
        {
            if (id != donation.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.DonationID))
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
                ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
                return View(donation);
            }
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> EnterRunSheet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donation.SingleOrDefaultAsync(m => m.DonationID == id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterRunSheet(int id, [Bind("DonationID,DonorID,Title,Description,EstimatedValue,SuggestedStartingBid,UnitsOffered,PotentialTaxBreak,DateOfEvent,RainDate")] Donation donation)
        {
            if (id != donation.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.DonationID))
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
                ViewBag.Donors = new SelectList(_context.Donor.ToList(), "DonorID", "FullID");
                return View(donation);
            }
        }


        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donation
                .SingleOrDefaultAsync(m => m.DonationID == id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewBag.Donor = await _context.Donor.SingleOrDefaultAsync(o => o.DonorID == donation.DonorID);

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donation.SingleOrDefaultAsync(m => m.DonationID == id);
            _context.Donation.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donation.Any(e => e.DonationID == id);
        }
    }
}
