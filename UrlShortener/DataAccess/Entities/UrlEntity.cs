using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.DataAccess.Entities;

public class UrlEntity
{
    [BsonId] public string Id { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
    public DateTime InsertDate { get; set; }
}