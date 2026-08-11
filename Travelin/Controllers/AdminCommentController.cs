using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> CommentList(string status)
        {
            var comments = await _commentService.GetAllCommentsAsync();

            if (status == "pending")
                comments = comments.Where(c => !c.IsStatus).ToList();
            else if (status == "approved")
                comments = comments.Where(c => c.IsStatus).ToList();

            var tours = await _tourService.GetAllTourAsync();

            foreach (var comment in comments)
            {
                var tour = tours.FirstOrDefault(t => t.TourId == comment.TourId);
                comment.TourTitle = tour?.Title ?? "-";
            }

            ViewBag.Status = status;
            ViewBag.PendingCount = (await _commentService.GetAllCommentsAsync()).Count(c => !c.IsStatus);

            return View(comments.OrderByDescending(c => c.CommentDate).ToList());
        }

        public async Task<IActionResult> ApproveComment(string id, string status)
        {
            await _commentService.ChangeCommentStatusAsync(id, true);
            TempData["Success"] = "Yorum onaylandı ve yayınlandı.";
            return RedirectToAction("CommentList", new { status });
        }

        public async Task<IActionResult> RejectComment(string id, string status)
        {
            await _commentService.ChangeCommentStatusAsync(id, false);
            TempData["Success"] = "Yorumun onayı kaldırıldı.";
            return RedirectToAction("CommentList", new { status });
        }

        public async Task<IActionResult> DeleteComment(string id, string status)
        {
            await _commentService.DeleteCommentAsync(id);
            TempData["Success"] = "Yorum kalıcı olarak silindi.";
            return RedirectToAction("CommentList", new { status });
        }
    }
}