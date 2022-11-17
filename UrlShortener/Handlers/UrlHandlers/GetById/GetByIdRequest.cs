using MediatR;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.GetById
{
    public sealed record GetByIdRequest : IRequest<UrlGetDto>
    {
        public string Id { get; set; } = null!;
    }
}
