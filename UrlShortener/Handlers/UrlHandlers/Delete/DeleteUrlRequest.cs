using MediatR;

namespace UrlShortener.Handlers.UrlHandlers.Delete;

public class DeleteUrlRequest : IRequest<bool>
{
    public string Id { get; set; }
}