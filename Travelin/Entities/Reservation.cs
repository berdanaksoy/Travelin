using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travelin.Entities
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReservationId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime ReservationDate { get; set; }
        public int PersonCount { get; set; }

        public string Status { get; set; } = "Beklemede";
        public DateTime CreatedDate { get; set; }
    }
}