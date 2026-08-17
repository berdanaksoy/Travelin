using AutoMapper;
using MongoDB.Bson;
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

        public async Task<ReservationListResultDto> GetFilteredReservationsAsync(ReservationFilterDto filter)
        {
            var builder = Builders<Reservation>.Filter;
            var conditions = new List<FilterDefinition<Reservation>>();

            if (!string.IsNullOrWhiteSpace(filter.Status))
                conditions.Add(builder.Eq(r => r.Status, filter.Status));

            if (!string.IsNullOrWhiteSpace(filter.TourId))
                conditions.Add(builder.Eq(r => r.TourId, filter.TourId));

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchFilter = builder.Or(
                    builder.Regex(r => r.Name, new BsonRegularExpression(filter.Search, "i")),
                    builder.Regex(r => r.Surname, new BsonRegularExpression(filter.Search, "i")),
                    builder.Regex(r => r.Email, new BsonRegularExpression(filter.Search, "i"))
                );
                conditions.Add(searchFilter);
            }

            var finalFilter = conditions.Any() ? builder.And(conditions) : builder.Empty;

            var totalCount = await _reservationCollection.CountDocumentsAsync(finalFilter);

            var sortDefinition = filter.SortBy switch
            {
                "oldest" => Builders<Reservation>.Sort.Ascending(r => r.CreatedDate),
                "personDesc" => Builders<Reservation>.Sort.Descending(r => r.PersonCount),
                "personAsc" => Builders<Reservation>.Sort.Ascending(r => r.PersonCount),
                _ => Builders<Reservation>.Sort.Descending(r => r.CreatedDate)
            };

            var values = await _reservationCollection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();

            return new ReservationListResultDto
            {
                Reservations = _mapper.Map<List<ResultReservationDto>>(values),
                TotalCount = totalCount
            };
        }
    }
}