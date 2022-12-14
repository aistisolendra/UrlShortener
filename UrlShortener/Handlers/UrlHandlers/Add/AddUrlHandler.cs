using AutoMapper;
using MediatR;
using UrlShortener.DataAccess.Entities;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Models.UrlModel;
using UrlShortener.Services.ShortStringGen;

namespace UrlShortener.Handlers.UrlHandlers.Add;

public sealed class AddUrlHandler : IRequestHandler<AddUrlRequest, UrlGetDto>
{
    private readonly IUrlRepository _urlRepository;
    private readonly IMapper _mapper;
    private readonly IShortStringGenService _shortStringGen;

    public AddUrlHandler(IUrlRepository urlRepository, IMapper mapper, IShortStringGenService shortStringGen)
    {
        ArgumentNullException.ThrowIfNull(nameof(urlRepository));
        ArgumentNullException.ThrowIfNull(nameof(mapper));
        ArgumentNullException.ThrowIfNull(nameof(shortStringGen));

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