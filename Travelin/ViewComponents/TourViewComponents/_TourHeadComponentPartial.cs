using Microsoft.AspNetCore.Mvc;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class _TourHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
