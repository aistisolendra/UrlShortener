namespace UrlShortener.Models.UrlModel;

public record UrlAddDto
{
    public string Url { get; set; } = null!;
}