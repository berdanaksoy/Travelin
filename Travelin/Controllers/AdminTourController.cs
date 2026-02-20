using Microsoft.AspNetCore.Mvc;

namespace Travelin.Controllers
{
    public class AdminTourController : Controller
    {
        public IActionResult TourList()
        {
            return View();
        }
    }
}
