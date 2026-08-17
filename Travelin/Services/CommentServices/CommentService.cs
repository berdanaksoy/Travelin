using AutoMapper;
using MongoDB.Driver;
using Travelin.Dtos.CommentDtos;
using Travelin.Entities;
using Travelin.Settings;

namespace Travelin.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Comment> _commentCollection;

        public CommentService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var values=new MongoClient(databaseSettings.ConnectionString);
            var database= values.GetDatabase(databaseSettings.DatabaseName);
            _commentCollection=database.GetCollection<Comment>(databaseSettings.CommentCollectionName);

            _mapper =mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var values= _mapper.Map<Comment>(createCommentDto);
            await _commentCollection.InsertOneAsync(values);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentCollection.DeleteOneAsync(c=>c.CommentId==id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentsAsync()
        {
            var values = await _commentCollection.Find(c=>true).ToListAsync();
            return _mapper.Map<List<ResultCommentDto>>(values);
        }

        public async Task<GetCommentByIdDto> GetCommentByIdAsync(string id)
        {
            var values = await _commentCollection.Find<Comment>(c => c.CommentId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetCommentByIdDto>(values);
        }

        public async Task<List<ResultCommentListByTourIdDto>> GetCommentsByTourIdAsync(string id)
        {
            var values = await _commentCollection.Find<Comment>(c => c.TourId == id).ToListAsync();
            return _mapper.Map<List<ResultCommentListByTourIdDto>>(values);
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            var values = _mapper.Map<Comment>(updateCommentDto);
            await _commentCollection.FindOneAndReplaceAsync(c => c.CommentId == updateCommentDto.CommentId, values);
        }

        public async Task<List<ResultCommentListByTourIdDto>> GetApprovedCommentsByTourIdAsync(string id)
        {
            var values = await _commentCollection
                .Find(c => c.TourId == id && c.IsStatus)
                .SortByDescending(c => c.CommentDate)
                .ToListAsync();

            return _mapper.Map<List<ResultCommentListByTourIdDto>>(values);
        }

        public async Task ChangeCommentStatusAsync(string id, bool status)
        {
            var update = Builders<Comment>.Update.Set(c => c.IsStatus, status);
            await _commentCollection.UpdateOneAsync(c => c.CommentId == id, update);
        }

        public async Task DeleteCommentsByTourIdAsync(string tourId)
        {
            await _commentCollection.DeleteManyAsync(c => c.TourId == tourId);
        }

        public async Task<List<ResultCommentDto>> GetTopRatedCommentsAsync(int count)
        {
            var values = await _commentCollection
                .Find(c => c.IsStatus && c.Score == 5)
                .ToListAsync();

            var random = values.OrderBy(x => Guid.NewGuid()).Take(count).ToList();

            return _mapper.Map<List<ResultCommentDto>>(random);
        }

        public async Task<(double average, int count)> GetTourRatingAsync(string tourId)
        {
            var comments = await _commentCollection
                .Find(c => c.TourId == tourId && c.IsStatus)
                .ToListAsync();

            if (comments.Count == 0)
                return (0, 0);

            return (comments.Average(c => c.Score), comments.Count);
        }

        public async Task<CommentListResultDto> GetFilteredCommentsAsync(CommentFilterDto filter)
        {
            var builder = Builders<Comment>.Filter;
            var conditions = new List<FilterDefinition<Comment>>();

            if (filter.Status == "pending")
                conditions.Add(builder.Eq(c => c.IsStatus, false));
            else if (filter.Status == "approved")
                conditions.Add(builder.Eq(c => c.IsStatus, true));

            if (!string.IsNullOrWhiteSpace(filter.TourId))
                conditions.Add(builder.Eq(c => c.TourId, filter.TourId));

            var finalFilter = conditions.Any() ? builder.And(conditions) : builder.Empty;

            var totalCount = await _commentCollection.CountDocumentsAsync(finalFilter);

            var sortDefinition = filter.SortBy switch
            {
                "oldest" => Builders<Comment>.Sort.Ascending(c => c.CommentDate),
                "scoreDesc" => Builders<Comment>.Sort.Descending(c => c.Score),
                "scoreAsc" => Builders<Comment>.Sort.Ascending(c => c.Score),
                _ => Builders<Comment>.Sort.Descending(c => c.CommentDate)
            };

            var values = await _commentCollection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();

            return new CommentListResultDto
            {
                Comments = _mapper.Map<List<ResultCommentDto>>(values),
                TotalCount = totalCount
            };
        }
    }
}
