using MongoDB.Driver;
using UrlShortener.DataAccess.Base;
using UrlShortener.DataAccess.Entities;

namespace UrlShortener.DataAccess.Repositories;

public sealed class UrlRepository : BaseRepository<UrlEntity>, IUrlRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UrlRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UrlEntity> AddAsync(UrlEntity urlEntity, CancellationToken cancellationToken)
    {
        await Collection.InsertOneAsync(urlEntity, cancellationToken: cancellationToken);

        return urlEntity;
    }

    public async Task<bool> UpdateAsync(string id, UrlEntity urlEntity, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

        var updateUrl = Builders<UrlEntity>.Update.Set(x => x.Url, urlEntity.Url);
        var updateShortUrl = Builders<UrlEntity>.Update.Set(x => x.ShortUrl, urlEntity.ShortUrl);

        var combinedUpdate = Builders<UrlEntity>.Update.Combine(updateUrl, updateShortUrl);

        var result = await Collection.UpdateOneAsync(filter, combinedUpdate, cancellationToken: cancellationToken);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    /* If Sharded Or has replica
    public async Task AddAsync(UrlEntity urlEntity, CancellationToken cancellationToken)
    {
        _unitOfWork.AddCommand(() => Collection.InsertOneAsync(urlEntity, cancellationToken: cancellationToken));

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        _unitOfWork.AddCommand(() =>
        {
            var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

            return Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        });

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
    }*/
}