using UrlShortener.Models;

namespace UrlShortener.DataAccess
{
    public interface IUrlRepository
    {
        Task AddAsync(UrlEntity entity, CancellationToken cancellationToken);
        Task UpdateAsync(string id, UrlEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}