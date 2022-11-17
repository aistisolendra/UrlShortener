using Serilog;
using UrlShortener.Application;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder
        .ConfigureSettings()
        .ConfigureBaseServices()
        .ConfigureSerilog()
        .ConfigureMiddleware()
        .ConfigureMediator()
        .ConfigureAutoMapper()
        .ConfigureRepositories()
        .ConfigureServices();

    var app = builder.Build();

    app
        .UseDevelopment()
        .UseSerilog()
        .UseMiddleware()
        .UseBaseApplication()
        .SetupMongoDb()
        .Run();
}
catch (Exception exception)
{
    Log.Logger.Fatal("Application failed to start with an exception {Exception}", exception.Message);
}