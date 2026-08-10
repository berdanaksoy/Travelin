using System.ComponentModel.DataAnnotations;

namespace Travelin.Dtos.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(60, ErrorMessage = "Kategori adı en fazla 60 karakter olabilir.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Kategori ikonu zorunludur.")]
        [Url(ErrorMessage = "Geçerli bir ikon bağlantısı giriniz.")]
        public string IconUrl { get; set; }

        public bool IsStatus { get; set; }
    }
}