using MediatR;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.Add;

public sealed record AddUrlRequest : IRequest<UrlGetDto>
{
    public UrlAddDto UrlAddDto { get; set; }
    public int MaxLength { get; set; }
}