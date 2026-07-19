using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travelin.Entities
{
    public class TourProgram
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourProgramId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }

        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}