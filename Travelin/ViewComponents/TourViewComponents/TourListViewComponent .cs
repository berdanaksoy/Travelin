using Microsoft.AspNetCore.Mvc;
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

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            int pageSize = 3;

            var totalCount = await _tourService.GetTotalTourCountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedValues = await _tourService.GetToursByPageAsync(page, pageSize);

            foreach (var tour in pagedValues)
            {
                var rating = await _commentService.GetTourRatingAsync(tour.TourId);
                tour.AverageRating = rating.average;
                tour.CommentCount = rating.count;
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedValues);
        }
    }
}