using UrlShortener.Application;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder
        .ConfigureSettings()
        .ConfigureBaseServices()
        .ConfigureMiddleware()
        .ConfigureMediator()
        .ConfigureAutoMapper()
        .ConfigureRepositories()
        .ConfigureServices();

    var app = builder.Build();

    app
        .ConfigureDevelopment()
        .ConfigureMiddleware()
        .ConfigureBaseApplication()
        .ConfigureMongoDb()
        .Run();
}
catch (Exception exception)
{
    
}