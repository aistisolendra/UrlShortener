using MongoDB.Driver;

namespace UrlShortener.Services.Retry
{
    public static class RetryServiceExceptions
    {
        public static Type[] MongoDbExceptions()
        {
            return new[]
            {
                typeof(MongoExecutionTimeoutException),
                typeof(MongoConnectionException),
                typeof(MongoWriteException)
            };
        }
    }
}
