using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travelin.Entities
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentId { get; set; }
        public string Headline { get; set; }
        public string CommentDetail { get; set; }
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
    }
}
