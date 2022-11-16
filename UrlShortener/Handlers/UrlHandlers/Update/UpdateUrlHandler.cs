using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Entities;
using UrlShortener.DataAccess.Repositories;

namespace UrlShortener.Handlers.UrlHandlers.Update
{
    public class UpdateUrlHandler : IRequestHandler<UpdateUrlRequest, bool>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper;
        public UpdateUrlHandler(IUrlRepository urlRepository, IMapper mapper)
        {
            _urlRepository = urlRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateUrlRequest request, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<UrlEntity>(request.UrlUpdateDto);

            var result = await _urlRepository.UpdateAsync(request.Id, mapped, cancellationToken);

            return result;
        }
    }
}
