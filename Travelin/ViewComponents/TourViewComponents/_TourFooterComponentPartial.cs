using Microsoft.AspNetCore.Mvc;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class _TourFooterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
