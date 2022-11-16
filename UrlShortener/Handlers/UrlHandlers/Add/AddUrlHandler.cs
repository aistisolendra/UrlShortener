using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Entities;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Models.UrlModel;
using UrlShortener.Services;

namespace UrlShortener.Handlers.UrlHandlers.Add;

public class AddUrlHandler : IRequestHandler<AddUrlRequest, UrlGetDto>
{
    private readonly IUrlRepository _urlRepository;
    private readonly IMapper _mapper;
    private readonly IShortStringGenService _shortStringGen;

    public AddUrlHandler(IUrlRepository urlRepository, IMapper mapper, IShortStringGenService shortStringGen)
    {
        _urlRepository = urlRepository;
        _mapper = mapper;
        _shortStringGen = shortStringGen;
    }

    public async Task<UrlGetDto> Handle(AddUrlRequest request, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<UrlEntity>(request.UrlAddDto);
        var shortUrl = _shortStringGen.GetShortUrl(request.MaxLength);
        mapped.ShortUrl = shortUrl;

        var result = await _urlRepository.AddAsync(mapped, cancellationToken);

        var mappedResult = _mapper.Map<UrlGetDto>(result);

        return mappedResult;
    }
}