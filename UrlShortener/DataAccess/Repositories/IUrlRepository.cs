using UrlShortener.DataAccess.Entities;

namespace UrlShortener.DataAccess.Repositories
{
    public interface IUrlRepository
    {
        Task<UrlEntity> AddAsync(UrlEntity entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(string id, UrlEntity entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}