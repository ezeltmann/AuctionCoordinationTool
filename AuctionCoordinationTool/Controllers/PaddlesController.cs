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
    public class PaddlesController : Controller
    {
        private readonly AuctionDBContext _context;

        public PaddlesController(AuctionDBContext context)
        {
            _context = context;
        }

        // GET: Paddles
        public async Task<IActionResult> Index()
        {
            ViewBag.Bidders = _context.Bidder.ToList().ToDictionary(o => o.BidderId);
            return View(await _context.Paddle.ToListAsync());
        }

        // GET: Paddles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paddle = await _context.Paddle
                .SingleOrDefaultAsync(m => m.PaddleId == id);
            if (paddle == null)
            {
                return NotFound();
            }

            ViewBag.Bidder = await _context.Bidder.SingleOrDefaultAsync(o => o.BidderId == paddle.BidderId);
            return View(paddle);
        }

        // GET: Paddles/Create
        public IActionResult Create()
        {
            ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
            return View();
        }

        // POST: Paddles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaddleId,BidderId,PaddleNumber")] Paddle paddle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paddle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
                return View(paddle);
            }
        }

        // GET: Paddles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paddle = await _context.Paddle.SingleOrDefaultAsync(m => m.PaddleId == id);
            if (paddle == null)
            {
                return NotFound();
            }
            ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
            return View(paddle);
        }

        // POST: Paddles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaddleId,BidderId,PaddleNumber")] Paddle paddle)
        {
            if (id != paddle.PaddleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paddle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaddleExists(paddle.PaddleId))
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
                ViewBag.Bidders = new SelectList(_context.Bidder.ToList(), "BidderId", "FullName");
                return View(paddle);
            }            
        }

        // GET: Paddles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paddle = await _context.Paddle
                .SingleOrDefaultAsync(m => m.PaddleId == id);
            if (paddle == null)
            {
                return NotFound();
            }

            ViewBag.Bidder = await _context.Bidder.SingleOrDefaultAsync(o => o.BidderId == paddle.BidderId);

            return View(paddle);
        }

        // POST: Paddles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paddle = await _context.Paddle.SingleOrDefaultAsync(m => m.PaddleId == id);
            _context.Paddle.Remove(paddle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaddleExists(int id)
        {
            return _context.Paddle.Any(e => e.PaddleId == id);
        }
    }
}
