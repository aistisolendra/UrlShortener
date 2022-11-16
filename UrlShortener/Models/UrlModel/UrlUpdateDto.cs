﻿namespace UrlShortener.Models.UrlModel
{
    public class UrlUpdateDto
    {
        public string Url { get; set; }
        public string ShortUrl { get; set; }

        public UrlUpdateDto(string url, string shortUrl)
        {
            Url = url;
            ShortUrl = shortUrl;
        }
    }
}
