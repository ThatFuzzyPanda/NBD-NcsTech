 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class ProjectsController : CognizantController
    {
        private readonly NBDContext _context;

        public ProjectsController(NBDContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index( DateTime? Start, DateTime? End, string SearchString,string SearchStringORG, int? ClientId, int? page, int? pageSizeID,string sortOrder)
        {
            PopulateDropDownLists();

            var projects = _context.Projects
                            .Include(p => p.Client)                 
                            .AsNoTracking();

            
            //Add as many filters as needed
            if (ClientId.HasValue)
            {
                projects = projects.Where(p => p.ClientId == ClientId);

            }
            
            if (!System.String.IsNullOrEmpty(SearchStringORG))
            {
                projects = projects.Where(p => p.Client.Name.ToUpper().Contains(SearchStringORG.ToUpper()));
            }
            if (Start.HasValue && End !> Start)
            {
                projects = projects.Where(t => t.BidDate >= Start && t.BidDate <= End);
            }
            //sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortparm = sortOrder == "Date" ? "date_desc" : "Date";
            

            switch(sortOrder)
            {
                case "Date":
                    projects = projects.OrderBy(p => p.BidDate);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(p => p.BidDate);
                    break;
                default:
                    projects = projects.OrderBy(p => p.BidDate);
                    break;
            }

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            Project p = new Project()
            {
                BidDate = DateTime.Today,
            };
            PopulateDropDownLists();
			return View(p);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BidDate,ProjectSite,Est_BeginDate,Est_CompleteDate,BidAmount,ClientId")] Project project)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(project);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ProjectsDesignBids", new { ProjectId = project.Id });
            }
            
            PopulateDropDownLists(project);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
			PopulateDropDownLists(project);
			return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BidDate,ProjectSite,Est_BeginDate,Est_CompleteDate,BidAmount,ClientId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
			PopulateDropDownLists(project);
			return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("There are no project to delete.");
            }
            var project = await _context.Projects.FindAsync(id);
            try
            {
                if (project != null)
                {
                    _context.Projects.Remove(project);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Project. You cannot delete a Project that has a Client in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(project);

        }
        //client ddl
        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context
                .Clients
                .OrderBy(m => m.Name), "ID", "Name", selectedId);
        }
        private void PopulateDropDownLists(Project project = null)
        {
            ViewData["ClientId"] = ClientSelectList(project?.ClientId);
        }
            private bool ProjectExists(int id)
        {
          return _context.Projects.Any(e => e.Id == id);
        }
    }
}
