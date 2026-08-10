using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.TourDtos
{
    public class UpdateTourDto
    {
        public string TourId { get; set; }

        [Required(ErrorMessage = "Tur başlığı zorunludur.")]
        [StringLength(120, ErrorMessage = "Başlık en fazla 120 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Ülke zorunludur.")]
        [StringLength(60, ErrorMessage = "Ülke en fazla 60 karakter olabilir.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Şehir zorunludur.")]
        [StringLength(60, ErrorMessage = "Şehir en fazla 60 karakter olabilir.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Açıklama 10 ile 1000 karakter arasında olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kapasite zorunludur.")]
        [Range(1, 500, ErrorMessage = "Kapasite 1 ile 500 arasında olmalıdır.")]
        public int? Capacity { get; set; }

        [Required(ErrorMessage = "Tur tarihi zorunludur.")]
        public DateTime TourDate { get; set; }

        [Required(ErrorMessage = "Süre bilgisi zorunludur.")]
        public string DayNight { get; set; }

        [Required(ErrorMessage = "Kapak görseli zorunludur.")]
        [Url(ErrorMessage = "Geçerli bir görsel bağlantısı giriniz.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 9999999, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal? Price { get; set; }

        [Url(ErrorMessage = "Geçerli bir görsel bağlantısı giriniz.")]
        public string? LocationImageUrl { get; set; }

        public bool IsStatus { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public string CategoryId { get; set; }

        [Url(ErrorMessage = "Geçerli bir video bağlantısı giriniz.")]
        public string? VideoUrl { get; set; }
    }
}