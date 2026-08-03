using AutoMapper;
using MongoDB.Driver;
using Travelin.Dtos.ReservationDtos;
using Travelin.Entities;
using Travelin.Settings;

namespace Travelin.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Reservation> _reservationCollection;

        public ReservationService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(_databaseSettings.ReservationCollectionName);

            _mapper = mapper;
        }

        public async Task CreateReservationAsync(CreateReservationDto createReservationDto)
        {
            var value = _mapper.Map<Reservation>(createReservationDto);
            value.Status = ReservationStatuses.Pending;
            value.CreatedDate = DateTime.Now;
            await _reservationCollection.InsertOneAsync(value);
        }

        public async Task DeleteReservationAsync(string id)
        {
            await _reservationCollection.DeleteOneAsync(x => x.ReservationId == id);
        }

        public async Task<List<ResultReservationDto>> GetAllReservationAsync()
        {
            var values = await _reservationCollection
                .Find(x => true)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(values);
        }

        public async Task<GetReservationByIdDto> GetReservationByIdAsync(string id)
        {
            var value = await _reservationCollection.Find(x => x.ReservationId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetReservationByIdDto>(value);
        }

        public async Task<List<ResultReservationDto>> GetReservationsByTourIdAsync(string tourId)
        {
            var values = await _reservationCollection
                .Find(x => x.TourId == tourId)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(values);
        }

        public async Task ChangeReservationStatusAsync(string id, string status)
        {
            var update = Builders<Reservation>.Update.Set(x => x.Status, status);
            await _reservationCollection.UpdateOneAsync(x => x.ReservationId == id, update);
        }

        public Task UpdateReservationAsync(UpdateReservationDto updateReservationDto)
        {
            var value = _mapper.Map<Reservation>(updateReservationDto);
            return _reservationCollection.FindOneAndReplaceAsync(x => x.ReservationId == updateReservationDto.ReservationId, value);
        }

        public async Task<int> GetApprovedPersonCountByTourIdAsync(string tourId)
        {
            var approved = await _reservationCollection
                .Find(r => r.TourId == tourId && r.Status == ReservationStatuses.Approved)
                .ToListAsync();

            return approved.Sum(r => r.PersonCount);
        }

        public async Task<List<ResultReservationDto>> GetApprovedReservationsByTourIdAsync(string tourId)
        {
            var values = await _reservationCollection
                .Find(r => r.TourId == tourId && r.Status == ReservationStatuses.Approved)
                .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(values);
        }
    }
}