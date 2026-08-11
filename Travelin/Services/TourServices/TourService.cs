using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Travelin.Dtos.TourDtos;
using Travelin.Entities;
using Travelin.Settings;
using Travelin.Helpers;

namespace Travelin.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);

            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            createTourDto.VideoUrl = YouTubeHelper.NormalizeUrl(createTourDto.VideoUrl);
            var values = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(values);
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(t => t.TourId == id);
        }

        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(t => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var value = await _tourCollection.Find(t => t.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(value);
        }

        public Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            updateTourDto.VideoUrl = YouTubeHelper.NormalizeUrl(updateTourDto.VideoUrl);
            var values = _mapper.Map<Tour>(updateTourDto);
            return _tourCollection.FindOneAndReplaceAsync(t => t.TourId == updateTourDto.TourId, values);
        }

        public async Task<List<ResultTourDto>> GetToursByPageAsync(int page, int pageSize)
        {
            var values = await _tourCollection
                .Find(t => t.IsStatus)
                .SortByDescending(t => t.TourDate)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<long> GetTotalTourCountAsync()
        {
            return await _tourCollection.CountDocumentsAsync(t => t.IsStatus);
        }

        public async Task<TourListResultDto> GetFilteredToursAsync(TourFilterDto filter, bool onlyActive = false)
        {
            var builder = Builders<Tour>.Filter;
            var conditions = new List<FilterDefinition<Tour>>();

            if (onlyActive)
                conditions.Add(builder.Eq(t => t.IsStatus, true));

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchFilter = builder.Or(
                    builder.Regex(t => t.Title, new BsonRegularExpression(filter.Search, "i")),
                    builder.Regex(t => t.City, new BsonRegularExpression(filter.Search, "i")),
                    builder.Regex(t => t.Country, new BsonRegularExpression(filter.Search, "i"))
                );
                conditions.Add(searchFilter);
            }

            if (!string.IsNullOrWhiteSpace(filter.Country))
                conditions.Add(builder.Eq(t => t.Country, filter.Country));

            if (filter.FromDate.HasValue)
                conditions.Add(builder.Gte(t => t.TourDate, filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                conditions.Add(builder.Lte(t => t.TourDate, filter.ToDate.Value));

            if (!string.IsNullOrWhiteSpace(filter.CategoryId))
                conditions.Add(builder.Eq(t => t.CategoryId, filter.CategoryId));

            var finalFilter = conditions.Any() ? builder.And(conditions) : builder.Empty;

            var totalCount = await _tourCollection.CountDocumentsAsync(finalFilter);

            var sortDefinition = filter.SortBy switch
            {
                "oldest" => Builders<Tour>.Sort.Ascending(t => t.TourDate),
                "titleAsc" => Builders<Tour>.Sort.Ascending(t => t.Title),
                "capacity" => Builders<Tour>.Sort.Descending(t => t.Capacity),
                "priceAsc" => Builders<Tour>.Sort.Ascending(t => t.Price),
                "priceDesc" => Builders<Tour>.Sort.Descending(t => t.Price),
                _ => Builders<Tour>.Sort.Descending(t => t.TourDate)
            };

            var collation = new Collation("tr", strength: CollationStrength.Secondary);

            var values = await _tourCollection
                .Find(finalFilter, new FindOptions { Collation = collation })
                .Sort(sortDefinition)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();

            return new TourListResultDto
            {
                Tours = _mapper.Map<List<ResultTourDto>>(values),
                TotalCount = totalCount
            };
        }

        public async Task<List<string>> GetDistinctCountriesAsync()
        {
            var values = await _tourCollection.Distinct(t => t.Country, Builders<Tour>.Filter.Empty).ToListAsync();
            return values.Where(c => !string.IsNullOrWhiteSpace(c)).OrderBy(c => c).ToList();
        }
    }
}
