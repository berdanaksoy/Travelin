using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travelin.Entities
{
    public class Tour
    {
        [BsonId]        //sql'de primary key karşılığı
        [BsonRepresentation(BsonType.ObjectId)]     //sql'de int karşılığı (ama int değil, mongodb karşılığı)
        public string TourId { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime TourDate { get; set; }
        public string DayNight { get; set; }
    }
}
