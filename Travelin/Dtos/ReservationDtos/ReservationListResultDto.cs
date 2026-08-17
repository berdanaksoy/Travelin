namespace Travelin.Dtos.ReservationDtos
{
    public class ReservationListResultDto
    {
        public List<ResultReservationDto> Reservations { get; set; }
        public long TotalCount { get; set; }
    }
}