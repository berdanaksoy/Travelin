using Travelin.Dtos.TourDtos;
using Travelin.Dtos.CategoryDtos;
using Travelin.Dtos.CommentDtos;

namespace Travelin.Models
{
    public class HomeViewModel
    {
        public List<ResultTourDto> FeaturedTours { get; set; }
        public List<ResultCategoryDto> Categories { get; set; }
        public List<ResultCategoryDto> FeaturedCategories { get; set; }
        public List<ResultCommentDto> TopComments { get; set; }
    }
}