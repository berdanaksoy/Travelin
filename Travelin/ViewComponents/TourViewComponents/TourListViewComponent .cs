using Microsoft.AspNetCore.Mvc;
using Travelin.Services.TourServices;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class TourListViewComponent : ViewComponent
    {
        private readonly ITourService _tourService;

        public TourListViewComponent(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            int pageSize = 3;

            var totalCount = await _tourService.GetTotalTourCountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedValues = await _tourService.GetToursByPageAsync(page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedValues);
        }
    }
}
