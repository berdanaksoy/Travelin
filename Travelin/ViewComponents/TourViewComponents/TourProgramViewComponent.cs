using Microsoft.AspNetCore.Mvc;
using Travelin.Services.TourProgramServices;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class TourProgramViewComponent : ViewComponent
    {
        private readonly ITourProgramService _tourProgramService;

        public TourProgramViewComponent(ITourProgramService tourProgramService)
        {
            _tourProgramService = tourProgramService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string tourId)
        {
            var values = await _tourProgramService.GetTourProgramsByTourIdAsync(tourId);
            return View(values);
        }
    }
}