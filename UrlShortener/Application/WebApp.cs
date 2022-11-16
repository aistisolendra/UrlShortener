using MongoDB.Bson;
using MongoDB.Driver;
using UrlShortener.DataAccess.Base;

namespace UrlShortener.Application
{
    public static class WebApp
    {
        public static WebApplication ConfigureBaseApplication(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

        public static WebApplication ConfigureDevelopment(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

        public static WebApplication ConfigureMongoDb(this WebApplication app)
        {
            var applicationSettings = app.ResolveSettings();

            var client = new MongoClient(applicationSettings.ConnectionString);

            var database = client.GetDatabase(applicationSettings.DatabaseName);

            database.ConfigureCollections();

            return app;
        }

        private static void ConfigureCollections(this IMongoDatabase database)
        {
            if (!database.CollectionExists(CollectionNames.UrlCollection))
                database.CreateCollection(CollectionNames.UrlCollection);
        }

        private static bool CollectionExists(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return collections.Any();
        }

        private static ApplicationSettings ResolveSettings(this WebApplication app)
        {
            return app.Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
        }
    }
}