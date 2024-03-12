using Microsoft.AspNetCore.Mvc;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Utilities;
using NBDProjectNcstech.Models;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NBDProjectNcstech.Controllers
{
    public class HomeController : CognizantController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NBDContext _context;

        public HomeController(ILogger<HomeController> logger, NBDContext nBDContext)
        {
            _logger = logger;
            _context = nBDContext;
        }
        public async Task<IActionResult> Index( DateTime? Start, DateTime? End,  string SearchString, string SearchStringORG, string SearchStringORGP, int? idProject, int? Id,int? page, int? pageSizeID)
        {
            var clients = _context.Clients.AsNoTracking();
            var projects =  _context.Projects
                            .Include(p => p.Client)
                            .AsNoTracking();

			if (Id.HasValue)
			{
				clients = clients.Where(p => p.ID == Id);
			}
			if (idProject.HasValue)
			{
				projects = projects.Where(p => p.ClientId == idProject);
			}

			if (!System.String.IsNullOrEmpty(SearchStringORG))
            { 
				clients = clients.Where(c => c.Name.ToUpper().Contains(SearchStringORG.ToUpper()));
			}
			if (!System.String.IsNullOrEmpty(SearchStringORGP))
			{
				projects = projects.Where(c => c.Client.Name.ToUpper().Contains(SearchStringORGP.ToUpper()));
			}
			if (!System.String.IsNullOrEmpty(SearchString))
			{
				clients = clients.Where(c => c.ContactPersonFirst.ToUpper().Contains(SearchString.ToUpper()));
			}
			if (Start.HasValue && End! > Start)
			{
				projects = projects.Where(t => t.BidDate >= Start && t.BidDate <= End);
			}

			//Handle Paging
			int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            // Separate paginated lists for clients and projects
            var pagedDataClient = await PaginatedList<Client>.CreateAsync(clients.AsQueryable(), page ?? 1, pageSize);
            var pagedDataProject = await PaginatedList<Project>.CreateAsync(projects.AsQueryable(), page ?? 1, pageSize);
            PopulateDropDownLists();
			var combinedLists = new PaginatedHomeLists
            {
                PagedClients = pagedDataClient,
                PagedProjects = pagedDataProject
            };

            return View(combinedLists);
        }
		private SelectList ClientSelectList(int? selectedId)
		{
			return new SelectList(_context
				.Clients
				.OrderBy(m => m.Name), "ID", "Name", selectedId);
		}
		private void PopulateDropDownLists(Client client = null)
        { 
			ViewData["Id"] = ClientSelectList(client?.ID);
			ViewData["idProject"] = ClientSelectList(client?.ID);
		}
		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
