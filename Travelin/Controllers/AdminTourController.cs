using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Services.CategoryServices;
using Travelin.Services.CommentServices;
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
        private readonly IMapper _mapper;

        public AdminTourController(ITourService tourService, ICategoryService categoryService, ITourProgramService tourProgramService, ICommentService commentService, IMapper mapper)
        {
            _tourService = tourService;
            _categoryService = categoryService;
            _tourProgramService = tourProgramService;
            _commentService = commentService;
            _mapper = mapper;
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
            await _tourService.CreateTourAsync(createTourDto);

            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> DeleteTour(string id)
        {
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
            await _tourService.UpdateTourAsync(updateTourDto);

            return RedirectToAction("TourList");
        }
    }
}
