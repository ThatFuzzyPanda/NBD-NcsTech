using Microsoft.AspNetCore.Mvc;

namespace NBDProjectNcstech.Controllers
{
	public class Login : Controller
	{
		public IActionResult Index()
		{
			return View("_LoginPartial");
		}
	}
}
