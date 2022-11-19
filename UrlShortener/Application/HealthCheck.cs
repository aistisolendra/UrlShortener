using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace UrlShortener.Application
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IMongoDatabase _database;

        public HealthCheck(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            _database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new())
        {
            var result = await CheckMongoDbConnectionAsync(cancellationToken);

            return result
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Degraded();
        }

        private async Task<bool> CheckMongoDbConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}",
                    cancellationToken: cancellationToken);
            }

            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}