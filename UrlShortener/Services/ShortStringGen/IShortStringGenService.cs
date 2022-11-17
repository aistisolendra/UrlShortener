namespace UrlShortener.Services.ShortStringGen
{
    public interface IShortStringGenService
    {
        string GetShortString(int? maxLength = null);
        string GetShortUrl(int? maxLength = null);
    }
}
