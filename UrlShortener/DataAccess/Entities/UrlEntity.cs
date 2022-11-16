using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.DataAccess.Entities;

public class UrlEntity
{
    [BsonId]
    public string Id { get; set; }
    public string Url { get; set; }
    public string ShortUrl { get; set; }
    public DateTime InsertDate { get; set; }
}