using Travelin.Dtos.TourProgramDtos;

namespace Travelin.Services.TourProgramServices
{
    public interface ITourProgramService
    {
        Task<List<ResultTourProgramDto>> GetAllTourProgramAsync();
        Task CreateTourProgramAsync(CreateTourProgramDto createTourProgramDto);
        Task UpdateTourProgramAsync(UpdateTourProgramDto updateTourProgramDto);
        Task DeleteTourProgramAsync(string id);
        Task<GetTourProgramByIdDto> GetTourProgramByIdAsync(string id);
        Task<List<ResultTourProgramDto>> GetTourProgramsByTourIdAsync(string tourId);
        Task DeleteTourProgramsByTourIdAsync(string tourId);
    }
}