using Microsoft.AspNetCore.Mvc;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Utilities;
using NBDProjectNcstech.Models;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var clients = _context.Clients.AsNoTracking();
            var projects =  _context.Projects
                            .Include(p => p.Client)
                            .AsNoTracking();

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            // Separate paginated lists for clients and projects
            var pagedDataClient = await PaginatedList<Client>.CreateAsync(clients.AsQueryable(), page ?? 1, pageSize);
            var pagedDataProject = await PaginatedList<Project>.CreateAsync(projects.AsQueryable(), page ?? 1, pageSize);

            var combinedLists = new PaginatedHomeLists
            {
                PagedClients = pagedDataClient,
                PagedProjects = pagedDataProject
            };

            return View(combinedLists);
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
