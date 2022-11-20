using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using UrlShortener.Handlers.UrlHandlers.Add;
using UrlShortener.Handlers.UrlHandlers.Delete;
using UrlShortener.Handlers.UrlHandlers.GetAll;
using UrlShortener.Handlers.UrlHandlers.GetById;
using UrlShortener.Handlers.UrlHandlers.Update;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Tests.Controllers
{
    public class UrlControllerTests
    {
        private readonly UrlController _controller;
        private readonly Mock<IMediator> _mediator;

        public UrlControllerTests()
        {
            _mediator = new Mock<IMediator>();

            _controller = new UrlController(_mediator.Object);
        }

        [Theory]
        [InlineData(3,1)]
        [InlineData(10,1)]
        [InlineData(999,1)]
        public async Task GetAll_LargeInput_ReturnsAllResults(int pageSize, int pageIndex)
        {
            // Arrange
            var expectedReturn = Urls().ToList();

            _mediator
                .Setup(x => x.Send(It.IsAny<GetAllUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedReturn);

            // Act
            var result = await _controller.GetAll(pageIndex, pageSize);
            var resultObject = (OkObjectResult)result.Result!;
            var actualResult = resultObject.Value;

            // Assert
            Assert.Equal(expectedReturn, actualResult);
            Assert.Equal(StatusCodes.Status200OK, resultObject.StatusCode);
        }

        [Fact]
        public async Task GetAll_SmallInput_ReturnsTwoResults()
        {
            // Arrange
            var expectedReturn = Urls().Take(2).ToList();
            var pageSize = 2;
            var pageIndex = 1;

            _mediator
                .Setup(x => x.Send(It.IsAny<GetAllUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedReturn);

            // Act
            var result = await _controller.GetAll(pageIndex, pageSize);
            var resultObject = (OkObjectResult)result.Result!;
            var actualResult = resultObject.Value;

            // Assert
            Assert.Equal(expectedReturn, actualResult);
            Assert.Equal(StatusCodes.Status200OK, resultObject.StatusCode);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(-1,-1)]
        [InlineData(-999,-999)]
        [InlineData(0,-1)]
        [InlineData(-1,0)]
        public async Task GetAll_BadInputs_ThrowsValidationException(int pageIndex, int pageSize)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _controller.GetAll(pageIndex, pageSize));
        }

        [Fact]
        public async Task GetById_GoodInput_ReturnsResult()
        {
            // Arrange
            var expectedReturn = Urls().First();

            _mediator
                .Setup(x => x.Send(It.IsAny<GetByIdRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedReturn);

            // Act
            var result = await _controller.GetById(Guid.Parse((ReadOnlySpan<char>)expectedReturn.Id));
            var resultObject = (OkObjectResult)result.Result!;
            var actualResult = resultObject.Value;

            // Assert
            Assert.Equal(expectedReturn, actualResult);
            Assert.Equal(StatusCodes.Status200OK, resultObject.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(999)]
        public async Task Add_GoodInput_ReturnsResult(int maxLength)
        {
            // Arrange
            var input = UrlAddDto();
            var expectedResult = UrlGetDto();

            _mediator
                .Setup(x => x.Send(It.IsAny<AddUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Add(input, maxLength);
            var resultObject = (OkObjectResult)result.Result!;
            var actualResult = (UrlGetDto)resultObject.Value!;

            // Assert
            Assert.Equal(input.Url, actualResult.Url);
            Assert.NotEmpty(actualResult.Id);
            Assert.NotEmpty(actualResult.ShortUrl);
            Assert.Equal(StatusCodes.Status200OK, resultObject.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task Add_BadMaxLength_ThrowsValidationException(int maxLength)
        {
            // Arrange
            var input = UrlAddDto();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _controller.Add(input, maxLength));
        }

        [Fact]
        public async Task Update_GoodInput_ReturnsResult()
        {
            // Arrange
            var input = UrlUpdateDto();
            var expectedResult = UrlGetDto();
            var id = expectedResult.Id;

            _mediator
                .Setup(x => x.Send(It.IsAny<UpdateUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(Guid.Parse(id), input);
            var resultObject = (OkObjectResult)result.Result!;
            var actualResult = (bool)resultObject.Value!;

            // Assert
            Assert.True(actualResult);
            Assert.Equal(StatusCodes.Status200OK, resultObject.StatusCode);
        }

        [Fact]
        public async Task Update_GoodInput_Delete()
        {
            // Arrange
            var expectedResult = UrlGetDto();
            var id = expectedResult.Id;

            _mediator
                .Setup(x => x.Send(It.IsAny<DeleteUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(Guid.Parse(id));
            var resultObject = (NoContentResult)result;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, resultObject.StatusCode);
        }

        private IEnumerable<UrlGetDto> Urls()
        {
            return new List<UrlGetDto>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ShortUrl = "ShortUrl1",
                    Url = "Url1"
                },

                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ShortUrl = "ShortUrl2",
                    Url = "Url2"
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ShortUrl = "ShortUrl2",
                    Url = "Url2"
                }
            };
        }

        private UrlAddDto UrlAddDto()
        {
            return new UrlAddDto()
            {
                Url = "Url"
            };
        }

        private UrlGetDto UrlGetDto()
        {
            return new UrlGetDto()
            {
                Id = Guid.NewGuid().ToString(),
                ShortUrl = "ShortUrl",
                Url = "Url"
            };
        }

        private UrlUpdateDto UrlUpdateDto()
        {
            return new UrlUpdateDto()
            {
                ShortUrl = "ShortUrlUpdated",
                Url = "UrlUpdated"
            };
        }
    }
}