using UrlShortener.Application;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureSettings()
    .ConfigureBaseServices()
    .ConfigureServices();


var app = builder.Build();

app
    .ConfigureDevelopment()
    .ConfigureBaseApplication()
    .ConfigureMongoDb()
    .Run();
