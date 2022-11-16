namespace UrlShortener.Models.UrlModel;

public class UrlAddDto
{
    public string Url { get; set; }

    public UrlAddDto(string url)
    {
        Url = url;
    }
}