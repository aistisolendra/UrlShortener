using MediatR;
using UrlShortener.DataAccess.Repositories;

namespace UrlShortener.Handlers.UrlHandlers.Delete;

public sealed class DeleteUrlHandler : IRequestHandler<DeleteUrlRequest, bool>
{
    private readonly IUrlRepository _urlRepository;

    public DeleteUrlHandler(IUrlRepository urlRepository)
    {
        ArgumentNullException.ThrowIfNull(nameof(urlRepository));

        _urlRepository = urlRepository;
    }

    public async Task<bool> Handle(DeleteUrlRequest request, CancellationToken cancellationToken)
    {
        var result = await _urlRepository.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}