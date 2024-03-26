using Microsoft.AspNetCore.Authorization;
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
            var nBDContext = _context.MaterialRequirments
                .Include(m => m.DesignBid)
                .Include(m => m.Inventory)
                .Include(m => m.Unit);
            return View(await nBDContext.ToListAsync());
        }

		// GET: MaterialRequirments/Details/5
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments
                .Include(m => m.DesignBid)
                .Include(m => m.Inventory)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (materialRequirments == null)
            {
                return NotFound();
            }

            return View(materialRequirments);
        }

		// GET: MaterialRequirments/Create
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
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
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Create([Bind("ID,Quanity,InventoryID,DesignBidID,UnitID")] MaterialRequirments materialRequirments, string from)
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
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments.Include(m=>m.Unit).FirstOrDefaultAsync(m=>m.ID==id);
            if (materialRequirments == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(materialRequirments);
            return View(materialRequirments);
        }

        // POST: MaterialRequirments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Quanity,InventoryID,DesignBidID,UnitID")] MaterialRequirments materialRequirments)
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
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MaterialRequirments == null)
            {
                return NotFound();
            }

            var materialRequirments = await _context.MaterialRequirments
                .Include(m => m.DesignBid)
                .Include(m => m.Inventory)
                .Include(m => m.Unit)
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
        [Authorize(Roles = "Admin,Management")]
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
        private SelectList UnitSelectList(int? InventoryID, int? selectedId)
        {
            
            var query = from u in _context.Units
                        select u;
            if (InventoryID >= 0)
            {
                Inventory inv = _context.Inventory.FirstOrDefaultAsync(i => i.ID == InventoryID).Result;
                var invUnit = inv.UnitID;
                query = query.Where(u => u.ID == invUnit);
            }
            return new SelectList(query.OrderBy(m => m.Name), "ID", "Name", selectedId);

        }
        private void PopulateDropDownLists(MaterialRequirments materialRequirments = null)
        {
            ViewData["DesignBidID"] = OneDesignBidSelectList(materialRequirments?.DesignBidID);
            if ((materialRequirments?.UnitID).HasValue)
            {
                if (materialRequirments.Unit == null)
                {
                    materialRequirments.Unit = _context.Units.Find(materialRequirments.UnitID);
                }
                ViewData["InventoryID"] = InventorySelectList(materialRequirments.InventoryID);
                ViewData["UnitID"] = UnitSelectList(materialRequirments.InventoryID, materialRequirments.UnitID);
            }
            else
            {
                ViewData["InventoryID"] = InventorySelectList(null);
                ViewData["UnitID"] = UnitSelectList(null, null);
            }
        }
        [HttpGet]
        public JsonResult GetUnits(int InventoryID)
        {
            return Json(UnitSelectList(InventoryID, null));
        }

        private bool MaterialRequirmentsExists(int id)
        {
            return _context.MaterialRequirments.Any(e => e.ID == id);
        }
    }
}
