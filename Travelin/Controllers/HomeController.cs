using Microsoft.AspNetCore.Mvc;
using Travelin.Models;
using Travelin.Services.CategoryServices;
using Travelin.Services.CommentServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;

        public HomeController(ITourService tourService, ICategoryService categoryService, ICommentService commentService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredTours = await _tourService.GetToursByPageAsync(1, 6);

            foreach (var tour in featuredTours)
            {
                var rating = await _commentService.GetTourRatingAsync(tour.TourId);
                tour.AverageRating = rating.average;
                tour.CommentCount = rating.count;
            }

            var model = new HomeViewModel
            {
                FeaturedTours = featuredTours,
                Categories = await _categoryService.GetActiveCategoriesAsync(),
                FeaturedCategories = await _categoryService.GetRandomActiveCategoriesAsync(6),
                TopComments = await _commentService.GetTopRatedCommentsAsync(6)
            };

            return View(model);
        }
    }
}