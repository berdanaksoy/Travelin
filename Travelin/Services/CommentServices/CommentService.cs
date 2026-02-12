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

        public Task<GetCommentByIdDto> GetCommentByIdAsync(string id)
        {
            var values = _commentCollection.Find<Comment>(c => c.CommentId == id).FirstOrDefaultAsync();
            return _mapper.Map<Task<GetCommentByIdDto>>(values);
        }

        public Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            var values = _mapper.Map<Comment>(updateCommentDto);
            return _commentCollection.FindOneAndReplaceAsync(c => c.CommentId == updateCommentDto.CommentId, values);
        }
    }
}
