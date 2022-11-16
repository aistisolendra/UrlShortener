using MediatR;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.Update;

public class UpdateUrlRequest : IRequest<bool>
{
    public string Id { get; set; }
    public UrlUpdateDto UrlUpdateDto { get; set; }
}