using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.ReservationDtos
{
    public class CreateReservationDto
    {
        public string TourId { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        [StringLength(50, ErrorMessage = "NameLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SurnameRequired")]
        [StringLength(50, ErrorMessage = "SurnameLength")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneRequired")]
        [RegularExpression(@"^[0-9\s\+\-\(\)]{7,20}$", ErrorMessage = "PhoneInvalid")]
        public string Phone { get; set; }

        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "PersonCountRequired")]
        [Range(1, 10, ErrorMessage = "PersonCountRange")]
        public int? PersonCount { get; set; }
    }
}