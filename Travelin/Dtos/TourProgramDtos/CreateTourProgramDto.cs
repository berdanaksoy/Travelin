using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.TourProgramDtos
{
    public class CreateTourProgramDto
    {
        [Required(ErrorMessage = "Tur seçimi zorunludur.")]
        public string TourId { get; set; }

        [Range(1, 30, ErrorMessage = "Gün numarası 1 ile 30 arasında olmalıdır.")]
        public int? DayNumber { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(90, ErrorMessage = "Başlık en fazla 90 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; }
    }
}