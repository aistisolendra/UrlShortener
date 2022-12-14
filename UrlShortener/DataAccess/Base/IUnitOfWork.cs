using MongoDB.Driver;

namespace UrlShortener.DataAccess.Base;

public interface IUnitOfWork
{
    void AddCommand(Func<Task> func);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    IMongoCollection<T> GetCollection<T>(string name);
}