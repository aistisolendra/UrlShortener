using MongoDB.Bson;
using MongoDB.Driver;

namespace UrlShortener
{
    public static class ApplicationSetup
    {
        public static WebApplication SetupMongoDb(this WebApplication app)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var database = client.GetDatabase("local");

            if (!database.CollectionExists("UrlShortener"))
                database.CreateCollection("UrlShortener");

            return app;
        }

        private static bool CollectionExists(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return collections.Any();
        }
    }
}