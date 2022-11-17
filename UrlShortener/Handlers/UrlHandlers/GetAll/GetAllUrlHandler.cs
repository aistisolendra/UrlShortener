using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.GetAll
{
    public class GetAllUrlHandler : IRequestHandler<GetAllUrlRequest, IList<UrlGetDto>>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper;
        public GetAllUrlHandler(IUrlRepository urlRepository, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(nameof(urlRepository));
            ArgumentNullException.ThrowIfNull(nameof(mapper));

            _urlRepository = urlRepository;
            _mapper = mapper;
        }

        public async Task<IList<UrlGetDto>> Handle(GetAllUrlRequest request, CancellationToken cancellationToken)
        {
            var result = await _urlRepository.GetAll(request.PageIndex, request.PageSize, cancellationToken);

            var mapped = _mapper.Map<IList<UrlGetDto>>(result);

            return mapped;
        }
    }
}
