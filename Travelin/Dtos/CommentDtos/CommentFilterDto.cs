namespace Travelin.Dtos.CommentDtos
{
    public class CommentFilterDto
    {
        public string Status { get; set; }
        public string TourId { get; set; }
        public string SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}