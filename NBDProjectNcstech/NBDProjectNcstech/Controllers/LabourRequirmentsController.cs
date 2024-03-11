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
    public class LabourRequirmentsController : Controller
    {
        private readonly NBDContext _context;

        public LabourRequirmentsController(NBDContext context)
        {
            _context = context;
        }

        // GET: LabourRequirments
        public async Task<IActionResult> Index()
        {
            var nBDContext = _context.LabourRequirments.Include(l => l.DesignBid).Include(l => l.Labour);
            return View(await nBDContext.ToListAsync());
        }

        // GET: LabourRequirments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabourRequirments == null)
            {
                return NotFound();
            }

            var labourRequirments = await _context.LabourRequirments
                .Include(l => l.DesignBid)
                .Include(l => l.Labour)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourRequirments == null)
            {
                return NotFound();
            }

            return View(labourRequirments);
        }

        // GET: LabourRequirments/Create
        public IActionResult Create()
        {
            string from = Request.Query["from"];
            ViewData["From"] = from;
            PopulateDropDownLists();
            GetLabourCosts();
            return View();
        }

        // POST: LabourRequirments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Hours,Description,UnitPrice,LabourID,DesignBidID")] LabourRequirments labourRequirments, string from)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labourRequirments);
                await _context.SaveChangesAsync();
                if(from == "AddLandM")
                {
                    return RedirectToAction("AddLabourAndMaterialPage", "DesignBids", new { id = labourRequirments.DesignBidID });
                }
                return RedirectToAction("Edit", "DesignBids", new { id = labourRequirments.DesignBidID });
            }
            PopulateDropDownLists(labourRequirments);
            return RedirectToAction("Edit", "DesignBids", new { id = labourRequirments.DesignBidID });
        }

        // GET: LabourRequirments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabourRequirments == null)
            {
                return NotFound();
            }

            var labourRequirments = await _context.LabourRequirments.FindAsync(id);
            if (labourRequirments == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            return View(labourRequirments);
        }

        // POST: LabourRequirments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Hours,Description,UnitPrice,LabourID,DesignBidID")] LabourRequirments labourRequirments)
        {
            if (id != labourRequirments.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labourRequirments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabourRequirmentsExists(labourRequirments.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "DesignBids", new { id = labourRequirments.DesignBidID });
            }
            PopulateDropDownLists(labourRequirments);
            return RedirectToAction("Edit", "DesignBids", new { id = labourRequirments.DesignBidID });
        }

        // GET: LabourRequirments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabourRequirments == null)
            {
                return NotFound();
            }

            var labourRequirments = await _context.LabourRequirments
                .Include(l => l.DesignBid)
                .Include(l => l.Labour)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourRequirments == null)
            {
                return NotFound();
            }

            return View(labourRequirments);
        }

        // POST: LabourRequirments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabourRequirments == null)
            {
                return Problem("Entity set 'NBDContext.LabourRequirments'  is null.");
            }
            var labourRequirments = await _context.LabourRequirments.FindAsync(id);
            if (labourRequirments != null)
            {
                _context.LabourRequirments.Remove(labourRequirments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "DesignBids", new { id = labourRequirments.DesignBidID });
        }

        private SelectList OneDesignBidSelectList(int? selectedId)
        {
            int bidID = Convert.ToInt32(Request.Query["bidID"]);

            //ViewData["bidID"] = bidID;
            return new SelectList(_context
                .DesignBids
                .Where(m => m.ID == bidID)
                .Select(m => new
                {
                    ID = m.ID,
                    DisplayText = m.Project.ProjectSite // DisplayText will contain only the ProjectSite
                }), "ID", "DisplayText", selectedId);
        }

        private SelectList LabourSelectList(int? selectedId)
        {
            return new SelectList(_context
                .Labours
                .OrderBy(m => m.LabourType), "ID", "LabourType", selectedId);
        }

        private void PopulateDropDownLists(LabourRequirments labourRequirments = null)
        {
            ViewData["DesignBidID"] = OneDesignBidSelectList(labourRequirments?.DesignBidID);
            ViewData["LabourID"] = LabourSelectList(labourRequirments?.LabourID);
        }

        private void GetLabourCosts()
        {
            var labourCosts = _context.Labours; // Replace _yourRepository with your actual repository or data access method

            // Convert the list to a dictionary where LabourID is the key and LabourCost is the value
            var labourCostsDictionary = labourCosts.ToDictionary(l => l.ID, l => l.LabourCost);

            // Pass the dictionary to ViewBag.LabourCosts
            ViewBag.LabourCosts = labourCostsDictionary;
        }


    

        private bool LabourRequirmentsExists(int id)
        {
            return _context.LabourRequirments.Any(e => e.ID == id);
        }
    }
}
