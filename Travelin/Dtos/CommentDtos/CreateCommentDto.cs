using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.CommentDtos
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "HeadlineRequired")]
        [StringLength(100, ErrorMessage = "HeadlineLength")]
        public string Headline { get; set; }

        [Required(ErrorMessage = "CommentRequired")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "CommentLength")]
        public string CommentDetail { get; set; }

        [Range(1, 5, ErrorMessage = "ScoreRange")]
        public int Score { get; set; }

        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
        public string TourId { get; set; }

        [Required(ErrorMessage = "NameSurnameRequired")]
        [StringLength(80, ErrorMessage = "NameSurnameLength")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }
    }
}