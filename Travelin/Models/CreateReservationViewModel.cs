using Travelin.Dtos.ReservationDtos;
using Travelin.Dtos.TourDtos;

namespace Travelin.Models
{
    public class CreateReservationViewModel
    {
        public GetTourByIdDto Tour { get; set; }
        public CreateReservationDto Reservation { get; set; }
    }
}