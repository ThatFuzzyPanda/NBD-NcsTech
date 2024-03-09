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
    public class MaterialRequirmentsController : Controller
    {
        private readonly NBDContext _context;

        public MaterialRequirmentsController(NBDContext context)
        {
            _context = context;
        }

        // GET: MaterialRequirments
        public async Task<IActionResult> Index()
        {
            var nBDContext = _context.MaterialRequirments.Include(m => m.DesignBid).Include(m => m.Inventory);
            return View(await nBDContext.ToListAsync());
        }

        // GET: MaterialRequirments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments
                .Include(m => m.DesignBid)
                .Include(m => m.Inventory)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materialRequirments == null)
            {
                return NotFound();
            }

            return View(materialRequirments);
        }

        // GET: MaterialRequirments/Create
        public IActionResult Create()
        {
            string from = Request.Query["from"];
            ViewData["From"] = from;
            PopulateDropDownLists();
            return View();
        }

        // POST: MaterialRequirments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Quanity,InventoryID,DesignBidID")] MaterialRequirments materialRequirments, string from)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialRequirments);
                await _context.SaveChangesAsync();
                if (from == "AddLandM")
                {
                    return RedirectToAction("AddLabourAndMaterialPage", "DesignBids", new { id = materialRequirments.DesignBidID });
                }
                return RedirectToAction("Edit", "DesignBids", new { id = materialRequirments.DesignBidID });
            }
            PopulateDropDownLists(materialRequirments);
            return RedirectToAction("Edit", "DesignBids", new { id = materialRequirments.DesignBidID });
        }
        

        // GET: MaterialRequirments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments.FindAsync(id);
            if (materialRequirments == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            return View(materialRequirments);
        }

        // POST: MaterialRequirments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Quanity,InventoryID,DesignBidID")] MaterialRequirments materialRequirments)
        {
            if (id != materialRequirments.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialRequirments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialRequirmentsExists(materialRequirments.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "DesignBids", new { id = materialRequirments.DesignBidID });
            }
            PopulateDropDownLists(materialRequirments);
            return RedirectToAction("Edit", "DesignBids", new { id = materialRequirments.DesignBidID });
        }

        // GET: MaterialRequirments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments
                .Include(m => m.DesignBid)
                .Include(m => m.Inventory)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materialRequirments == null)
            {
                return NotFound();
            }

            return View(materialRequirments);
        }

        // POST: MaterialRequirments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MaterialRequirments == null)
            {
                return Problem("Entity set 'NBDContext.MaterialRequirments'  is null.");
            }
            var materialRequirments = await _context.MaterialRequirments.FindAsync(id);
            if (materialRequirments != null)
            {
                _context.MaterialRequirments.Remove(materialRequirments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "DesignBids", new { id = materialRequirments.DesignBidID });
        }

        //SelectLists for  DDLs
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

        private SelectList InventorySelectList(int? selectedId)
        {
            return new SelectList(_context
                .Inventory
                .OrderBy(m => m.Name), "ID", "Name", selectedId);
        }

        private void PopulateDropDownLists(MaterialRequirments materialRequirments = null)
        {
            ViewData["DesignBidID"] = OneDesignBidSelectList(materialRequirments?.DesignBidID);
            ViewData["InventoryID"] = InventorySelectList(materialRequirments?.InventoryID);
        }

        private bool MaterialRequirmentsExists(int id)
        {
          return _context.MaterialRequirments.Any(e => e.ID == id);
        }
    }
}
