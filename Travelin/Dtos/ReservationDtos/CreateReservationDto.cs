namespace Travelin.Dtos.ReservationDtos
{
    public class CreateReservationDto
    {
        public string TourId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PersonCount { get; set; }
    }
}