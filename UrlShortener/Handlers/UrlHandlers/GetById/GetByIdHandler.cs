using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, UrlGetDto>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper;

        public GetByIdHandler(IUrlRepository urlRepository, IMapper mapper)
        {
            _urlRepository = urlRepository;
            _mapper = mapper;
        }

        public async Task<UrlGetDto> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _urlRepository.GetById(request.Id, cancellationToken);

            var mapped = _mapper.Map<UrlGetDto>(result);

            return mapped;
        }
    }
}
