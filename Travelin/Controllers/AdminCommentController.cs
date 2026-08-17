using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.CommentDtos;
using Travelin.Services.CommentServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class AdminCommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;

        public AdminCommentController(ICommentService commentService, ITourService tourService)
        {
            _commentService = commentService;
            _tourService = tourService;
        }

        public async Task<IActionResult> CommentList(string status, string tourId, string sortBy, int page = 1)
        {
            var filter = new CommentFilterDto
            {
                Status = status,
                TourId = tourId,
                SortBy = sortBy,
                Page = page < 1 ? 1 : page,
                PageSize = 10
            };

            var result = await _commentService.GetFilteredCommentsAsync(filter);
            var comments = result.Comments;

            var tours = await _tourService.GetAllTourAsync();

            foreach (var comment in comments)
            {
                var tour = tours.FirstOrDefault(t => t.TourId == comment.TourId);
                comment.TourTitle = tour?.Title ?? "-";
            }

            ViewBag.Tours = tours;
            ViewBag.Status = status;
            ViewBag.PendingCount = (await _commentService.GetAllCommentsAsync()).Count(c => !c.IsStatus);
            ViewBag.Filter = filter;
            ViewBag.CurrentPage = filter.Page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize);
            ViewBag.TotalCount = result.TotalCount;

            ViewBag.PaginationBaseUrl = Url.Action("CommentList", "AdminComment");
            ViewBag.PaginationParams = new Dictionary<string, string>
                {
                    { "status", status },
                    { "tourId", tourId },
                    { "sortBy", sortBy }
                };

            return View(comments);
        }

        public async Task<IActionResult> ApproveComment(string id, string status, string tourId, string sortBy, int page = 1)
        {
            await _commentService.ChangeCommentStatusAsync(id, true);
            TempData["Success"] = "Yorum onaylandı ve yayınlandı.";
            return RedirectToAction("CommentList", new { status, tourId, sortBy, page });
        }

        public async Task<IActionResult> RejectComment(string id, string status, string tourId, string sortBy, int page = 1)
        {
            await _commentService.ChangeCommentStatusAsync(id, false);
            TempData["Success"] = "Yorumun onayı kaldırıldı.";
            return RedirectToAction("CommentList", new { status, tourId, sortBy, page });
        }

        public async Task<IActionResult> DeleteComment(string id, string status, string tourId, string sortBy, int page = 1)
        {
            await _commentService.DeleteCommentAsync(id);
            TempData["Success"] = "Yorum kalıcı olarak silindi.";
            return RedirectToAction("CommentList", new { status, tourId, sortBy, page });
        }
    }
}