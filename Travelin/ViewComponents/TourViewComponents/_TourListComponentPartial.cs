using Microsoft.AspNetCore.Mvc;
using Travelin.Services.TourServices;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class _TourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;

        public _TourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tours = await _tourService.GetAllTourAsync();

            return View(tours);
        }
    }
}
