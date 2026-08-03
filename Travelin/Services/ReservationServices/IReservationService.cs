using Travelin.Dtos.ReservationDtos;

namespace Travelin.Services.ReservationServices
{
    public interface IReservationService
    {
        Task<List<ResultReservationDto>> GetAllReservationAsync();
        Task CreateReservationAsync(CreateReservationDto createReservationDto);
        Task UpdateReservationAsync(UpdateReservationDto updateReservationDto);
        Task DeleteReservationAsync(string id);
        Task<GetReservationByIdDto> GetReservationByIdAsync(string id);

        Task<List<ResultReservationDto>> GetReservationsByTourIdAsync(string tourId);
        Task ChangeReservationStatusAsync(string id, string status);
        Task<int> GetApprovedPersonCountByTourIdAsync(string tourId);
        Task<List<ResultReservationDto>> GetApprovedReservationsByTourIdAsync(string tourId);
    }
}