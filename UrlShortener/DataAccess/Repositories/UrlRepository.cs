using MongoDB.Driver;
using UrlShortener.Application;
using UrlShortener.DataAccess.Base;
using UrlShortener.DataAccess.Entities;
using UrlShortener.Services.Retry;

namespace UrlShortener.DataAccess.Repositories;

public sealed class UrlRepository : BaseRepository<UrlEntity>, IUrlRepository
{
    private readonly RetryOptions _retrySettings;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRetryService _retryService;

    public UrlRepository(
        RetrySettings retrySettings,
        IRetryService retryService,
        IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _retryService = retryService;

        _retrySettings = new RetryOptions()
        {
            ExceptionsToCatch = RetryServiceExceptions.MongoDbExceptions(),
            RetryCount = retrySettings.DatabaseRetrySettings.RetryCount,
            RetryDelay = TimeSpan.FromSeconds(retrySettings.DatabaseRetrySettings.FirstTryDelayInSeconds),
            RetryTimeout = TimeSpan.FromSeconds(retrySettings.DatabaseRetrySettings.TimeoutInSeconds)
        };
    }

    public async Task<IList<UrlEntity>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _retryService.RetryAsync(
            () => Collection
                .Aggregate()
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken),
            _retrySettings
        );

        return result;
    }
    public async Task<UrlEntity> GetById(string id, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

        var result = await _retryService.RetryAsync(
            () => Collection
                .Find(filter)
                .FirstOrDefaultAsync(cancellationToken),
            _retrySettings
        );

        return result;
    }

    public async Task<UrlEntity> AddAsync(UrlEntity urlEntity, CancellationToken cancellationToken)
    {
        await _retryService.RetryAsync(
            () => Collection.InsertOneAsync(urlEntity, cancellationToken: cancellationToken),
            _retrySettings
        );

        return urlEntity;
    }

    public async Task<bool> UpdateAsync(string id, UrlEntity urlEntity, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

        var updateUrl = Builders<UrlEntity>.Update.Set(x => x.Url, urlEntity.Url);
        var updateShortUrl = Builders<UrlEntity>.Update.Set(x => x.ShortUrl, urlEntity.ShortUrl);

        var combinedUpdate = Builders<UrlEntity>.Update.Combine(updateUrl, updateShortUrl);

        var result = await _retryService.RetryAsync(
            () => Collection.UpdateOneAsync(filter, combinedUpdate, cancellationToken: cancellationToken),
            _retrySettings
        );

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);

        var result = await _retryService.RetryAsync(
            () => Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken),
            _retrySettings
        );

        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}