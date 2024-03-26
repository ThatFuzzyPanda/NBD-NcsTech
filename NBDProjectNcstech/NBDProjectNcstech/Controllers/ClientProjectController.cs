
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace NBDProjectNcstech.Controllers
{
    public class ClientProjectController : ElephantController
    {
        private readonly NBDContext _context;

        public ClientProjectController(NBDContext context)
        {
            _context = context;
        }

        // GET: ClientProject
        public async Task<IActionResult> Index(string SearchString, int? ClientId, int? page, int? pageSizeID, string sortOrder)
        {
            PopulateDropDownLists();

            var projects = from p in _context.Projects
                        .Include(p => p.Client)
                           where p.ClientId == ClientId.GetValueOrDefault()
                           select p;

            // Get the URL with the last filter, sort and page parameters from THE PATIENTS Index View
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "ClientProject");

            if (!ClientId.HasValue)
            {
                //Go back to the proper return URL for the Patients controller
                return Redirect(ViewData["returnURL"].ToString());
            }

            //Add as many filters as needed
            if (ClientId.HasValue)
            {
                projects = projects.Where(p => p.ClientId == ClientId);

            }
            if (!System.String.IsNullOrEmpty(SearchString))
            {
                projects = projects.Where(p => p.Client.ContactPersonFirst.ToUpper().Contains(SearchString.ToUpper()));
            }
            //sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortparm = sortOrder == "Date" ? "date_desc" : "Date";


            switch (sortOrder)
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

            //Now get the MASTER record, the client, so it can be displayed at the top of the screen
            Client client = await _context.Clients
                .Include(p => p.Projects)
                .Include(p=> p.City)
                .Include(p=> p.City.Province)
                .Where(p => p.ID == ClientId.GetValueOrDefault())
                .AsNoTracking()
                .FirstOrDefaultAsync();

            ViewBag.Client = client;

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }
        public IActionResult Add(int? ClientId, string ClientName)
        {
            if (!ClientId.HasValue)
            {
                return Redirect(ViewData["returnURL"].ToString());
            }
            ViewData["ClientName"] = ClientName;

            Project p = new Project()
            {
                ClientId = ClientId.GetValueOrDefault()
            };

            PopulateDropDownLists();
            return View(p);
        }

        // POST: ClientProject/Add
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
        public async Task<IActionResult> Add([Bind("BidDate,ProjectSite," +
            "Est_BeginDate,Est_CompleteDate,BidAmount,ClientId")] Project project, string ClientName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "ProjectsDesignBids", new { ProjectID = project.Id });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }

            PopulateDropDownLists(project);
            ViewData["ClientName"] = ClientName;
            return View(project);
        }

		// GET: ClientProject/Update/5
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Update(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                        .Include(p => p.Client)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);
                          
            if (project == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            return View(project);
        }

        // POST: ClientProject/Update/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Update(int id)
		{
			var projectToUpdate = await _context.Projects
						.Include(p => p.Client)
						.AsNoTracking()
						.FirstOrDefaultAsync(p => p.Id == id);

			if (projectToUpdate == null)
            {
                return NotFound();
            }
           
            if(await TryUpdateModelAsync<Project>(projectToUpdate,"",
                p => p.BidDate, p => p.ProjectSite, p => p.Est_BeginDate, p => p.Est_CompleteDate,
                p => p.BidAmount, p => p.ClientId))
            {
                try
                {
                    _context.Update(projectToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(projectToUpdate.Id))
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


            PopulateDropDownLists(projectToUpdate);
            return View(projectToUpdate);
        }

        // GET: ClientProject/Remove/5
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Remove(int? id)
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

        // POST: ClientProject/Remove/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'NBDContext.Projects'  is null.");
            }

            var project = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(m => m.Id == id);

            try
            {
                if (project != null)
                {
                    _context.Projects.Remove(project);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to Delete Project.");
            }

            return View(project);
        }

        //For Populating Lists
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
