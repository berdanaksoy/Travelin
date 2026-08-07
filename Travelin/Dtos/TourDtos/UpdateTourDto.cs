namespace Travelin.Dtos.TourDtos
{
    public class UpdateTourDto
    {
        public string TourId { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string LocationImageUrl { get; set; }
        public bool IsStatus { get; set; }
        public string CategoryId { get; set; }
        public string VideoUrl { get; set; }
    }
}
