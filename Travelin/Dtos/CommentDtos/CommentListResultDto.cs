namespace Travelin.Dtos.CommentDtos
{
    public class CommentListResultDto
    {
        public List<ResultCommentDto> Comments { get; set; }
        public long TotalCount { get; set; }
    }
}