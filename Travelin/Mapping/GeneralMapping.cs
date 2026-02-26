using AutoMapper;
using Travelin.Dtos.CategoryDtos;
using Travelin.Dtos.CommentDtos;
using Travelin.Dtos.TourDtos;
using Travelin.Entities;

namespace Travelin.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();

            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentByIdDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentListByTourIdDto>().ReverseMap();
        }
    }
}
