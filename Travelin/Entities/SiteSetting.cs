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
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string GitHubUrl { get; set; }
        public string XUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }
    }
}