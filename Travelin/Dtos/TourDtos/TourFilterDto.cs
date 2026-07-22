namespace Travelin.Dtos.TourDtos
{
    public class TourFilterDto
    {
        public string Search { get; set; }
        public string Country { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string CategoryId { get; set; }
    }
}