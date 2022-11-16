using MediatR;

namespace UrlShortener.Models.UrlModel
{
    public class UrlAddDto
    {
        public string Url { get; set; }
        public string ShortUrl { get; set; }

        public UrlAddDto(string url, string shortUrl)
        {
            Url = url;
            ShortUrl = shortUrl;
        }
    }
}