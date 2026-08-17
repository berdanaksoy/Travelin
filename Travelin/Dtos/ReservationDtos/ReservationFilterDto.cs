namespace Travelin.Dtos.ReservationDtos
{
    public class ReservationFilterDto
    {
        public string Status { get; set; }
        public string TourId { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}