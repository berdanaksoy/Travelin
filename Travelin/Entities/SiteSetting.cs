using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travelin.Entities
{
    public class SiteSetting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SiteSettingId { get; set; }

        public string VideoUrl { get; set; }
    }
}