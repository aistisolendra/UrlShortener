namespace UrlShortener.Models.UrlModel;

public class UrlGetDto
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string ShortUrl { get; set; }

    public UrlGetDto(string id, string url, string shortUrl)
    {
        Id = id;
        Url = url;
        ShortUrl = shortUrl;
    }
}