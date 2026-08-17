using Travelin.Dtos.CommentDtos;

namespace Travelin.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllCommentsAsync();
        Task CreateCommentAsync(CreateCommentDto createCommentDto);
        Task UpdateCommentAsync(UpdateCommentDto updateCommentDto);
        Task DeleteCommentAsync(string id);
        Task<GetCommentByIdDto> GetCommentByIdAsync(string id);
        Task<List<ResultCommentListByTourIdDto>> GetCommentsByTourIdAsync(string id);
        Task<List<ResultCommentListByTourIdDto>> GetApprovedCommentsByTourIdAsync(string id);
        Task ChangeCommentStatusAsync(string id, bool status);
        Task DeleteCommentsByTourIdAsync(string tourId);
        Task<List<ResultCommentDto>> GetTopRatedCommentsAsync(int count);
        Task<(double average, int count)> GetTourRatingAsync(string tourId);
        Task<CommentListResultDto> GetFilteredCommentsAsync(CommentFilterDto filter);
    }
}
