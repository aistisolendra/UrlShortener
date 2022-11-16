using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DataAccess;
using UrlShortener.DataAccess.Entities;
using UrlShortener.Handlers.UrlHandlers.Add;
using UrlShortener.Handlers.UrlHandlers.Delete;
using UrlShortener.Handlers.UrlHandlers.Update;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UrlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/add")]
        public async Task<ActionResult<UrlGetDto>> Add(
            [Required] [FromBody] UrlAddDto urlAddDto
        )
        {
            var request = new AddUrlRequest()
            {
                UrlAddDto = urlAddDto
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPatch("/update")]
        public async Task<ActionResult<bool>> Update(
            [Required][FromQuery] string id,
            [Required] [FromBody] UrlUpdateDto urlUpdateDto)
        {
            var request = new UpdateUrlRequest()
            {
                Id = id,
                UrlUpdateDto = urlUpdateDto
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpDelete("/delete")]
        public async Task<IActionResult> Delete(
            [Required] [FromQuery] string id
        )
        {
            var request = new DeleteUrlRequest()
            {
                Id = id
            };

            await _mediator.Send(request);

            return NoContent();
        }
    }
}