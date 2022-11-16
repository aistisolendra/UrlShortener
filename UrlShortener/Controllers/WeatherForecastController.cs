using Microsoft.AspNetCore.Mvc;
using UrlShortener.DataAccess;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly UrlRepository _urlRepository;

        public WeatherForecastController(UrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        [HttpGet("/create")]
        public async Task CreateTest()
        {
            await _urlRepository.AddAsync(new UrlEntity("test", "test", DateTime.Today), CancellationToken.None);
        }

        [HttpGet("/delete")]
        public async Task DeleteTest([FromQuery] string id)
        {
            await _urlRepository.DeleteAsync(id, CancellationToken.None);
        }
    }
}