namespace UrlShortener.Services.ShortStringGen
{
    public interface IShortStringGenService
    {
        string GetShortString(int maxLength);
        string GetShortUrl(int maxLength);
    }
}
