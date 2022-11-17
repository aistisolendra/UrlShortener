using MediatR;

namespace UrlShortener.Handlers.UrlHandlers.Delete;

public sealed record DeleteUrlRequest : IRequest<bool>
{
    public string Id { get; set; } = null!;
}