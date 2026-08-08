using AutoMapper;
using Travelin.Dtos.CategoryDtos;
using Travelin.Dtos.CommentDtos;
using Travelin.Dtos.ReservationDtos;
using Travelin.Dtos.SiteSettingDtos;
using Travelin.Dtos.TourDtos;
using Travelin.Dtos.TourProgramDtos;
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
            CreateMap<GetCategoryByIdDto, UpdateCategoryDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();
            CreateMap<GetTourByIdDto, UpdateTourDto>().ReverseMap();

            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentByIdDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentListByTourIdDto>().ReverseMap();

            CreateMap<TourProgram, CreateTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, ResultTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, UpdateTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, GetTourProgramByIdDto>().ReverseMap();

            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResultReservationDto>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, GetReservationByIdDto>().ReverseMap();

            CreateMap<SiteSetting, ResultSiteSettingDto>().ReverseMap();
            CreateMap<SiteSetting, UpdateSiteSettingDto>().ReverseMap();
            CreateMap<ResultSiteSettingDto, UpdateSiteSettingDto>();
        }
    }
}
