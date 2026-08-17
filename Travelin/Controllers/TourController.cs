using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Services.CategoryServices;
using Travelin.Services.ReservationServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;
        private readonly IReservationService _reservationService;

        public TourController(ITourService tourService, ICategoryService categoryService, IReservationService reservationService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> TourList(string search, string country, string categoryId,
    DateTime? fromDate, DateTime? toDate, string sortBy, int page = 1)
        {
            var filter = new TourFilterDto
            {
                Search = search,
                Country = country,
                CategoryId = categoryId,
                FromDate = fromDate,
                ToDate = toDate,
                SortBy = sortBy,
                Page = page < 1 ? 1 : page,
                PageSize = 6
            };

            ViewBag.Countries = await _tourService.GetDistinctCountriesAsync();
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();

            var result = await _tourService.GetFilteredToursAsync(filter, onlyActive: true);
            ViewBag.TotalCount = result.TotalCount;

            return View(filter);
        }

        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("TourList");

            var value = await _tourService.GetTourByIdAsync(id);

            if (value == null)
                return RedirectToAction("TourList");

            var approvedCount = await _reservationService.GetApprovedPersonCountByTourIdAsync(id);
            ViewBag.IsFull = approvedCount >= value.Capacity;
            ViewBag.IsPast = value.TourDate.Date < DateTime.Now.Date;

            return View(value);
        }
    }
}
