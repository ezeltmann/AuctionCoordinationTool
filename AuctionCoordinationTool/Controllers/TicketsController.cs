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
    public class TicketsController : Controller
    {
        private readonly AuctionDBContext _context;

        public TicketsController(AuctionDBContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            ViewBag.Bidders = _context.Bidder.ToList().ToDictionary(o => o.BidderId);
            ViewBag.AuctionTickets = _context.AuctionTickets.ToList().ToDictionary(o => o.TicketTypeId);
            return View(await _context.Ticket.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.BidderName = _context.Bidder.Where(o => o.BidderId == ticket.BidderId).Single().FullName;
            ViewBag.TicketType = _context.AuctionTickets.Where(o => o.TicketTypeId == ticket.TicketTypeId).Single().Description;
            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
            ViewBag.AuctionTickets = new SelectList(_context.AuctionTickets.ToList(), "TicketTypeId", "Description");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,BidderId,TicketTypeId,Count")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
                ViewBag.AuctionTickets = new SelectList(_context.AuctionTickets.ToList(), "TicketTypeId", "Description");
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
            ViewBag.AuctionTickets = new SelectList(_context.AuctionTickets.ToList(), "TicketTypeId", "Description");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,BidderId,TicketTypeId,Count")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.BidderName = _context.Bidder.Where(o => o.BidderId == ticket.BidderId).Single().FullName;
            ViewBag.TicketType = _context.AuctionTickets.Where(o => o.TicketTypeId == ticket.TicketTypeId).Single().Description;
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
