using MediatR;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.GetAll
{
    public sealed record GetAllUrlRequest : IRequest<IList<UrlGetDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}