using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using UrlShortener.DataAccess.Base;
using UrlShortener.Middlewares;

namespace UrlShortener.Application;

public static class WebApp
{
    public static WebApplication UseBaseApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static WebApplication UseDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/status", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            AllowCachingResponses = false
        });

        return app;
    }

    public static WebApplication SetupMongoDb(this WebApplication app)
    {
        var applicationSettings = app.ResolveSettings();

        var client = new MongoClient(applicationSettings.ConnectionString);

        var database = client.GetDatabase(applicationSettings.DatabaseName);

        database.ConfigureCollections();

        return app;
    }

    public static WebApplication UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }

    public static WebApplication UseSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

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