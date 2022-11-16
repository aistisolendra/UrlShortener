using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.Models
{
    public class UrlEntity
    {
        [BsonId]
        public string Id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime InsertDate { get; set; }

        public UrlEntity(string url, string shortUrl, DateTime insertDate)
        {
            Id = Guid.NewGuid().ToString();
            Url = url;
            ShortUrl = shortUrl;
            InsertDate = insertDate;
        }
    }
}