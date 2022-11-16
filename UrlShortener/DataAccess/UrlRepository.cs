using MongoDB.Driver;
using UrlShortener.Models;

namespace UrlShortener.DataAccess
{
    public class UrlRepository : BaseRepository<UrlEntity>, IUrlRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UrlRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(UrlEntity urlEntity, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(urlEntity, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(string id, UrlEntity urlEntity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, urlEntity.Id);

            var result = await Collection.ReplaceOneAsync(filter, urlEntity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UrlEntity>().Eq(x => x.Id, id);
            await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
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
}