using UrlShortener.Application;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureSettings()
    .ConfigureBaseServices()
    .ConfigureMediator()
    .ConfigureAutoMapper()
    .ConfigureRepositories()
    .ConfigureServices();

var app = builder.Build();

app
    .ConfigureDevelopment()
    .ConfigureBaseApplication()
    .ConfigureMongoDb()
    .Run();