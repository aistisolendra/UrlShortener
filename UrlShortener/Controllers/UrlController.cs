using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Handlers.UrlHandlers.Add;
using UrlShortener.Handlers.UrlHandlers.Delete;
using UrlShortener.Handlers.UrlHandlers.GetAll;
using UrlShortener.Handlers.UrlHandlers.GetById;
using UrlShortener.Handlers.UrlHandlers.Update;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
    private readonly IMediator _mediator;

    public UrlController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<UrlGetDto>>> GetAll(
        [FromRoute] int? pageIndex = null,
        [FromRoute] int? pageSize = null
    )
    {
        var request = new GetAllUrlRequest()
        {
            PageIndex = pageIndex.GetValueOrDefault(1),
            PageSize = pageSize.GetValueOrDefault(10)
        };

        var result = await _mediator.Send(request);

        return Ok(result.ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UrlGetDto>> GetById(
        [Required] [FromRoute] Guid id
    )
    {
        var request = new GetByIdRequest()
        {
            Id = id.ToString()
        };

        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UrlGetDto>> Add(
        [Required] [FromBody] UrlAddDto urlAddDto,
        [FromRoute] int? maxLength = null
    )
    {
        var request = new AddUrlRequest()
        {
            UrlAddDto = urlAddDto,
            MaxLength = maxLength.GetValueOrDefault(10)
        };

        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<ActionResult<bool>> Update(
        [Required][FromRoute] Guid id,
        [Required] [FromBody] UrlUpdateDto urlUpdateDto)
    {
        var request = new UpdateUrlRequest()
        {
            Id = id.ToString(),
            UrlUpdateDto = urlUpdateDto
        };

        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [Required] [FromRoute] Guid id
    )
    {
        var request = new DeleteUrlRequest()
        {
            Id = id.ToString()
        };

        await _mediator.Send(request);

        return NoContent();
    }
}