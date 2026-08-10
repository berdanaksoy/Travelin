using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.ContactDtos
{
    public class ContactMessageDto
    {
        [Required(ErrorMessage = "NameRequired")]
        [StringLength(50, ErrorMessage = "NameLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SurnameRequired")]
        [StringLength(50, ErrorMessage = "SurnameLength")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [RegularExpression(@"^$|^[0-9\s\+\-\(\)]{7,20}$", ErrorMessage = "PhoneInvalid")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "MessageRequired")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "MessageLength")]
        public string Message { get; set; }
    }
}