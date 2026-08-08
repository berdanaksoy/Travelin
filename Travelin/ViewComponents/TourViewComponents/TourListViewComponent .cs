using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Services.CommentServices;
using Travelin.Services.TourServices;

namespace Travelin.ViewComponents.TourViewComponents
{
    public class TourListViewComponent : ViewComponent
    {
        private readonly ITourService _tourService;
        private readonly ICommentService _commentService;

        public TourListViewComponent(ITourService tourService, ICommentService commentService)
        {
            _tourService = tourService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(TourFilterDto filter, string viewName = "Default")
        {
            var result = await _tourService.GetFilteredToursAsync(filter, onlyActive: true);

            foreach (var tour in result.Tours)
            {
                var rating = await _commentService.GetTourRatingAsync(tour.TourId);
                tour.AverageRating = rating.average;
                tour.CommentCount = rating.count;
            }

            ViewBag.CurrentPage = filter.Page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize);
            ViewBag.TotalCount = result.TotalCount;
            ViewBag.Filter = filter;
            ViewBag.ViewMode = viewName == "Grid" ? "grid" : "list";

            return View(viewName, result.Tours);
        }
    }
}