using AutoMapper;
using MongoDB.Driver;
using Travelin.Dtos.TourProgramDtos;
using Travelin.Entities;
using Travelin.Settings;

namespace Travelin.Services.TourProgramServices
{
    public class TourProgramService : ITourProgramService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<TourProgram> _tourProgramCollection;

        public TourProgramService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourProgramCollection = database.GetCollection<TourProgram>(_databaseSettings.TourProgramCollectionName);

            _mapper = mapper;
        }

        public async Task CreateTourProgramAsync(CreateTourProgramDto createTourProgramDto)
        {
            var value = _mapper.Map<TourProgram>(createTourProgramDto);
            await _tourProgramCollection.InsertOneAsync(value);
        }

        public async Task DeleteTourProgramAsync(string id)
        {
            await _tourProgramCollection.DeleteOneAsync(x => x.TourProgramId == id);
        }

        public async Task<List<ResultTourProgramDto>> GetAllTourProgramAsync()
        {
            var values = await _tourProgramCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourProgramDto>>(values);
        }

        public async Task<GetTourProgramByIdDto> GetTourProgramByIdAsync(string id)
        {
            var value = await _tourProgramCollection.Find(x => x.TourProgramId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourProgramByIdDto>(value);
        }

        public async Task<List<ResultTourProgramDto>> GetTourProgramsByTourIdAsync(string tourId)
        {
            var values = await _tourProgramCollection
                .Find(x => x.TourId == tourId)
                .SortBy(x => x.DayNumber)
                .ToListAsync();

            return _mapper.Map<List<ResultTourProgramDto>>(values);
        }

        public Task UpdateTourProgramAsync(UpdateTourProgramDto updateTourProgramDto)
        {
            var value = _mapper.Map<TourProgram>(updateTourProgramDto);
            return _tourProgramCollection.FindOneAndReplaceAsync(x => x.TourProgramId == updateTourProgramDto.TourProgramId, value);
        }

        public async Task DeleteTourProgramsByTourIdAsync(string tourId)
        {
            await _tourProgramCollection.DeleteManyAsync(x => x.TourId == tourId);
        }
    }
}