using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;

namespace NBDProjectNcstech.Controllers
{
    public class ApprovalsController : Controller
    {
        private readonly NBDContext _context;

        public ApprovalsController(NBDContext context)
        {
            _context = context;
        }

        // GET: Approvals
        public async Task<IActionResult> Index()
        {
            var nBDContext = _context.Approvals.Include(a => a.DesignBid);
            return View(await nBDContext.ToListAsync());
        }

        // GET: Approvals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Approvals == null)
            {
                return NotFound();
            }

            var approval = await _context.Approvals
                .Include(a => a.DesignBid)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // GET: Approvals/Create
        public IActionResult Create()
        {
            ViewData["DesignBidID"] = new SelectList(_context.DesignBids, "ID", "ID");
            return View();
        }

        // POST: Approvals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AdminApprovalStatus,AdminApprovalDate,AdminApprovalNotes,ClientApprovalStatus,ClientApprovalDate,ClientApprovalNotes,DesignBidID")] Approval approval)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approval);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DesignBidID"] = new SelectList(_context.DesignBids, "ID", "ID", approval.DesignBidID);
            return View(approval);
        }

        // GET: Approvals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Approvals == null)
            {
                return NotFound();
            }

            var approval = await _context.Approvals.FindAsync(id);
            if (approval == null)
            {
                return NotFound();
            }
            ViewData["DesignBidID"] = new SelectList(_context.DesignBids, "ID", "ID", approval.DesignBidID);
            return View(approval);
        }

        // POST: Approvals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AdminApprovalStatus,AdminApprovalDate,AdminApprovalNotes,ClientApprovalStatus,ClientApprovalDate,ClientApprovalNotes,DesignBidID")] Approval approval)
        {
            if (id != approval.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approval);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalExists(approval.ID))
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
            ViewData["DesignBidID"] = new SelectList(_context.DesignBids, "ID", "ID", approval.DesignBidID);
            return View(approval);
        }

        // GET: Approvals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Approvals == null)
            {
                return NotFound();
            }

            var approval = await _context.Approvals
                .Include(a => a.DesignBid)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // POST: Approvals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Approvals == null)
            {
                return Problem("Entity set 'NBDContext.Approvals'  is null.");
            }
            var approval = await _context.Approvals.FindAsync(id);
            if (approval != null)
            {
                _context.Approvals.Remove(approval);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalExists(int id)
        {
          return _context.Approvals.Any(e => e.ID == id);
        }
    }
}
