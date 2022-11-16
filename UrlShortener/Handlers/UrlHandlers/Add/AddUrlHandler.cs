using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Entities;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Models.UrlModel;

namespace UrlShortener.Handlers.UrlHandlers.Add
{
    public class AddUrlHandler : IRequestHandler<AddUrlRequest, UrlGetDto>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper;

        public AddUrlHandler(IUrlRepository urlRepository, IMapper mapper)
        {
            _urlRepository = urlRepository;
            _mapper = mapper;
        }

        public async Task<UrlGetDto> Handle(AddUrlRequest request, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<UrlEntity>(request.UrlAddDto);

            var result = await _urlRepository.AddAsync(mapped, cancellationToken);

            var mappedResult = _mapper.Map<UrlGetDto>(result);

            return mappedResult;
        }
    }
}