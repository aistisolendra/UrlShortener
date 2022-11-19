using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using UrlShortener.Application;
using UrlShortener.Services.ShortStringGen;

namespace UrlShortener.Tests.Services
{
    public class ShortStringGenServiceTests
    {
        private readonly IShortStringGenService _shortStringGenService;
        private readonly ApplicationSettings _applicationSettings;
        private readonly string _urlWithoutGuid;

        public ShortStringGenServiceTests()
        {
            _applicationSettings = SetupApplicationSettings();
            _urlWithoutGuid = SetupTestUrl();

            _shortStringGenService = new ShortStringGenService(_applicationSettings);
        }

        [Fact]
        public void GetShortString_PositiveNumber_ReturnShortString()
        {
            // Arrange
            var maxLength = 5;

            // Act
            var result = _shortStringGenService.GetShortString(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(maxLength, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortString_LargePositiveNumber_ReturnShortString()
        {
            // Arrange
            var maxLength = 999;

            // Act
            var result = _shortStringGenService.GetShortString(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(20, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortString_NegativeNumber_ReturnShortString_LengthOne()
        {
            // Arrange
            var maxLength = -1;

            // Act
            var result = _shortStringGenService.GetShortString(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortString_LargeNegativeNumber_ReturnShortString_LengthOne()
        {
            // Arrange
            var maxLength = -999;

            // Act
            var result = _shortStringGenService.GetShortString(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortUrl_PositiveNumber_ReturnShortUrl()
        {
            // Arrange
            var maxLength = 5;
            var expectedLength = _urlWithoutGuid.Length + maxLength;

            // Act
            var result = _shortStringGenService.GetShortUrl(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expectedLength, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortUrl_LargePositiveNumber_ReturnShortUrl_LengthOne()
        {
            // Arrange
            var maxLength = 999;
            var expectedLength = _urlWithoutGuid.Length + 20;

            // Act
            var result = _shortStringGenService.GetShortUrl(maxLength);
            
            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expectedLength, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortUrl_NegativeNumber_ReturnShortUrl_LengthOne()
        {
            // Arrange
            var maxLength = -1;
            var expectedLength = _urlWithoutGuid.Length + 1;

            // Act
            var result = _shortStringGenService.GetShortUrl(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expectedLength, result.Length);
            Assert.True(IsUrlValid(result));
        }

        [Fact]
        public void GetShortUrl_LargeNegativeNumber_ReturnShortUrl_LengthOne()
        {
            // Arrange
            var maxLength = -999;
            var expectedLength = _urlWithoutGuid.Length + 1;

            // Act
            var result = _shortStringGenService.GetShortUrl(maxLength);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expectedLength, result.Length);
            Assert.True(IsUrlValid(result));
        }

        private bool IsUrlValid(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }

        private ApplicationSettings SetupApplicationSettings()
        {
            return new ApplicationSettings()
            {
                Domain = "UrlShortener"
            };
        }

        private string SetupTestUrl() => $"http://{_applicationSettings.Domain}/";
    }
}