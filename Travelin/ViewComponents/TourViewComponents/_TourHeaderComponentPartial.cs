using Microsoft.AspNetCore.Mvc;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class _TourHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
