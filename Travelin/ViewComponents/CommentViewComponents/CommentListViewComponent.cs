using Microsoft.AspNetCore.Mvc;
using Travelin.Services.CommentServices;

namespace Travelin.ViewComponents.CommentViewComponents
{
    public class CommentListViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;

        public CommentListViewComponent(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string tourId)
        {
            var values = await _commentService.GetApprovedCommentsByTourIdAsync(tourId);
            return View(values);
        }
    }
}