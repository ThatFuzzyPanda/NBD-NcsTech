using Microsoft.AspNetCore.Mvc;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using System.Diagnostics;
using System.Dynamic;

namespace NBDProjectNcstech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NBDContext _context;

        public HomeController(ILogger<HomeController> logger, NBDContext nBDContext)
        {
            _logger = logger;
            _context = nBDContext;
        }

        public IActionResult Index()
        {
            dynamic combinedModel = new ExpandoObject();
            combinedModel.Clients = _context.Clients.ToList();
            combinedModel.Projects = _context.Projects.ToList();

            return View(combinedModel);
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
