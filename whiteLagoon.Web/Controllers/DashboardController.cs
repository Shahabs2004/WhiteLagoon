using Microsoft.AspNetCore.Mvc;

namespace whiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
