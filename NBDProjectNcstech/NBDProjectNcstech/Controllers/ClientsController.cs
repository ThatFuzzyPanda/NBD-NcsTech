using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using NBDProjectNcstech.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NBDProjectNcstech.Controllers
{
    public class ClientsController : CognizantController
    {
        private readonly NBDContext _context;

        public ClientsController(NBDContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string SearchString, int? Id, int? page, int? pageSizeID)
        {
            var clients = _context.Clients
                          .AsNoTracking();

            if (Id.HasValue)
            {
                clients = clients.Where(p => p.ID == Id);
            }
            if (!System.String.IsNullOrEmpty(SearchString))
            {
               
                clients = clients.Where(c => c.ContactPersonFirst.ToUpper().Contains(SearchString.ToUpper()));
            }
            
            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), page ?? 1, pageSize);
            PopulateDropDownLists();
            return View(pagedData);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .Include(c=>c.City.Province)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,ContactPerson,Phone,Street,CityID,PostalCode")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ClientProject", new { ClientId = client.ID });
            }
            PopulateDropDownLists(client);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(client);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ContactPersonFirst,ContactPersonLast,Phone,Street,CityID,PostalCode")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "ClientProject", new { ClientId = client.ID });
            }
            PopulateDropDownLists(client);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .Include(c => c.City.Province)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("There are no Clients to delete.");
            }
            
            var client = await _context.Clients.FindAsync(id);
            try
            {
                if (client != null)
                {
                    _context.Clients.Remove(client);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Client. You cannot delete a Client that has a Project in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(client);
        }
      
        private SelectList ProvinceSelectList(string selectedId)
        {
            return new SelectList(_context.Provinces
                .OrderBy(p => p.Name), "ID", "Name", selectedId);
        }
        private SelectList CitySelectList(string ProvinceID, int? selectedId)
        {
            var query = from c in _context.Cities
                        select c;
            if (!string.IsNullOrEmpty(ProvinceID))
            {
                query = query.Where(c => c.ProvinceID == ProvinceID);
            }
            return new SelectList(query.OrderBy(c => c.Name), "ID", "Summary", selectedId);

        }
        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context
                .Clients
                .OrderBy(m => m.Name), "ID", "Name", selectedId);
        }
        private void PopulateDropDownLists(Client client = null)
        {
            

            if ((client?.CityID).HasValue)
            {
                if (client.City == null)
                {
                    client.City = _context.Cities.Find(client.CityID);
                }
                ViewData["ProvinceID"] = ProvinceSelectList(client.City.ProvinceID);
                ViewData["CityID"] = CitySelectList(client.City.ProvinceID, client.CityID);
            }
            else
            {
                ViewData["ProvinceID"] = ProvinceSelectList(null);
                ViewData["CityID"] = CitySelectList(null, null);
            }
            ViewData["Id"] = ClientSelectList(client?.ID);
        }
        [HttpGet]
        public JsonResult GetCities(string ProvinceID)
        {
            return Json(CitySelectList(ProvinceID, null));
        }
        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }
    }
}
