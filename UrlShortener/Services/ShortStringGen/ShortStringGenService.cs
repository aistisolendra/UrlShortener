using System.Text.RegularExpressions;
using UrlShortener.Application;

namespace UrlShortener.Services.ShortStringGen
{
    public class ShortStringGenService : IShortStringGenService
    {
        private readonly ApplicationSettings _applicationSettings;

        public ShortStringGenService(ApplicationSettings applicationSettings)
        {
            ArgumentNullException.ThrowIfNull(nameof(applicationSettings));

            _applicationSettings = applicationSettings;
        }

        public string GetShortString(int maxLength)
        {
            if (maxLength <= 0)
                maxLength = 1;

            if(maxLength > 20)
                maxLength = 20;

            var generatedString = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");

            generatedString = Truncate(generatedString, maxLength);

            return generatedString;
        }

        public string GetShortUrl(int maxLength)
        {
            if (maxLength <= 0)
                maxLength = 1;

            if (maxLength > 20)
                maxLength = 20;

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