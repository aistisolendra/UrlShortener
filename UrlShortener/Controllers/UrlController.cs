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

    [HttpGet("{pageIndex:int}/{pageSize:int}")]
    public async Task<ActionResult<List<UrlGetDto>>> GetAll(
        [FromRoute] int pageIndex,
        [FromRoute] int pageSize
    )
    {
        if (pageIndex < 1)
            throw new ValidationException($"pageIndex cannot be less than 1, but was {pageIndex}");

        if (pageSize < 1)
            throw new ValidationException($"pageSize cannot be less than 1, but was {pageSize}");

        var request = new GetAllUrlRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize
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

    [HttpPost("{maxLength:int}")]
    public async Task<ActionResult<UrlGetDto>> Add(
        [Required] [FromBody] UrlAddDto urlAddDto,
        [FromRoute] int maxLength
    )
    {
        if (maxLength < 1)
            throw new ValidationException($"maxLength cannot be less than 1, but was {maxLength}");

        var request = new AddUrlRequest()
        {
            UrlAddDto = urlAddDto,
            MaxLength = maxLength
        };

        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<bool>> Update(
        [Required] [FromRoute] Guid id,
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

    [HttpDelete("{id:guid}")]
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