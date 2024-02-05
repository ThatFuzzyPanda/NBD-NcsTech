using Microsoft.AspNetCore.Mvc;

namespace NBDProjectNcstech.CustomControllers
{
    public class CognizantController : Controller
    {
        internal string ControllerName()
        {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        internal string ActionName()
        {
            return ControllerContext.RouteData.Values["action"].ToString();
        }
    }
}
