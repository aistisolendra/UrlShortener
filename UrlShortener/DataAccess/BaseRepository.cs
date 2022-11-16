﻿using MongoDB.Driver;
using UrlShortener.Models;

namespace UrlShortener.DataAccess
{
    public abstract class BaseRepository<TEntity>
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected IMongoCollection<TEntity> Collection;

        protected BaseRepository(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            Collection = uow.GetCollection<TEntity>(GetRepositoryNameFromType(typeof(TEntity)));
        }

        private string GetRepositoryNameFromType(Type type)
        {
            if (type == typeof(UrlEntity))
                return CollectionNames.UrlCollection;

            return string.Empty;
        }
    }
}