namespace UrlShortener.Models.UrlModel;

public record UrlUpdateDto
{
    public string Url { get; set; } 
    public string ShortUrl { get; set; }
}