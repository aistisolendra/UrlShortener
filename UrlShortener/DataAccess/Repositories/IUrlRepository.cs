using UrlShortener.DataAccess.Entities;

namespace UrlShortener.DataAccess.Repositories;

public interface IUrlRepository
{
    Task<IList<UrlEntity>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<UrlEntity> GetById(string id, CancellationToken cancellationToken);
    Task<UrlEntity> AddAsync(UrlEntity entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(string id, UrlEntity entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}