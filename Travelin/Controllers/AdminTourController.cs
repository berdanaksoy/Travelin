using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Entities;
using Travelin.Services.CategoryServices;
using Travelin.Services.CommentServices;
using Travelin.Services.ReservationServices;
using Travelin.Services.TourProgramServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;
        private readonly ITourProgramService _tourProgramService;
        private readonly ICommentService _commentService;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public AdminTourController(ITourService tourService, ICategoryService categoryService, ITourProgramService tourProgramService, ICommentService commentService, IMapper mapper, IReservationService reservationService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
            _tourProgramService = tourProgramService;
            _commentService = commentService;
            _mapper = mapper;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> TourList(string search, string country, string categoryId, DateTime? fromDate, DateTime? toDate, string sortBy, int page = 1)
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
                PageSize = 10
            };

            var result = await _tourService.GetFilteredToursAsync(filter);

            var categories = await _categoryService.GetAllCategoryAsync();

            foreach (var tour in result.Tours)
            {
                var category = categories.FirstOrDefault(c => c.CategoryId == tour.CategoryId);
                tour.CategoryName = category?.CategoryName ?? "-";
            }

            ViewBag.Categories = categories;

            ViewBag.TotalCount = result.TotalCount;
            ViewBag.CurrentPage = filter.Page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize);
            ViewBag.Filter = filter;
            ViewBag.Countries = await _tourService.GetDistinctCountriesAsync();

            return View(result.Tours);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                return View(createTourDto);
            }

            await _tourService.CreateTourAsync(createTourDto);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> DeleteTour(string id)
        {
            var reservations = await _reservationService.GetReservationsByTourIdAsync(id);

            bool hasActive = reservations.Any(r =>
                r.Status == ReservationStatuses.Pending || r.Status == ReservationStatuses.Approved);

            if (hasActive)
            {
                TempData["TourDeleteError"] = "Bu tura ait aktif rezervasyonlar var. Önce onları iptal edin.";
                return RedirectToAction("TourList");
            }

            await _commentService.DeleteCommentsByTourIdAsync(id);
            await _tourProgramService.DeleteTourProgramsByTourIdAsync(id);
            await _tourService.DeleteTourAsync(id);

            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> UpdateTour(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);

            if(value == null)
                return RedirectToAction("TourList");

            var model = _mapper.Map<UpdateTourDto>(value);

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                return View(updateTourDto);
            }

            await _tourService.UpdateTourAsync(updateTourDto);
            return RedirectToAction("TourList");
        }
    }
}
