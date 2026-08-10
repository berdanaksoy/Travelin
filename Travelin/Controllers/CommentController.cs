using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.CommentDtos;
using Travelin.Services.CommentServices;

namespace Travelin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["CommentError"] = "Lütfen yorum formundaki alanları kontrol edin.";
                return RedirectToAction("Detail", "Tour", new { id = createCommentDto.TourId });
            }

            createCommentDto.CommentDate = DateTime.Now;
            createCommentDto.IsStatus = false;

            await _commentService.CreateCommentAsync(createCommentDto);

            TempData["CommentSuccess"] = "Yorumunuz alındı, onaylandıktan sonra yayınlanacaktır.";
            return RedirectToAction("Detail", "Tour", new { id = createCommentDto.TourId });
        }
    }
}
