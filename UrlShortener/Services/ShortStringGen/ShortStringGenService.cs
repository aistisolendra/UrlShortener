using System.Text.RegularExpressions;
using UrlShortener.Application;

namespace UrlShortener.Services.ShortStringGen
{
    public class ShortStringGenService : IShortStringGenService
    {
        private readonly ApplicationSettings _applicationSettings;

        public ShortStringGenService(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public string GetShortString(int? maxLength = null)
        {
            var generatedString = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");

            if (maxLength.HasValue)
                generatedString = Truncate(generatedString, maxLength.Value);

            return generatedString;
        }

        public string GetShortUrl(int? maxLength = null)
        {
            var shortString = GetShortString(maxLength);
            var fullUrl = $"http://{_applicationSettings.Domain}/{shortString}";

            return fullUrl;
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength
                ? value
                : value[..maxLength];
        }
    }
}
