using MediatR;
using Serilog;
using UrlShortener.DataAccess.Base;
using UrlShortener.DataAccess.Repositories;
using UrlShortener.Middlewares;
using UrlShortener.Services.Retry;
using UrlShortener.Services.ShortStringGen;

namespace UrlShortener.Application;

public static class WebServices
{
    public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder app)
    {
        var applicationSettings = app.Configuration.GetSection(Constants.ApplicationSettings).Get<ApplicationSettings>();
        app.Services.AddSingleton(applicationSettings);

        var retrySettings = app.Configuration.GetSection(Constants.ApplicationRetrySettings).Get<RetrySettings>();
        app.Services.AddSingleton(retrySettings);

        return app;
    }


    public static WebApplicationBuilder ConfigureBaseServices(this WebApplicationBuilder app)
    {
        app.Services.AddControllers();
        app.Services.AddEndpointsApiExplorer();
        app.Services.AddSwaggerGen();

        return app;
    }

    public static WebApplicationBuilder ConfigureMediator(this WebApplicationBuilder app)
    {
        app.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies()); 
            
        return app;
    }

    public static WebApplicationBuilder ConfigureAutoMapper(this WebApplicationBuilder app)
    {
        app.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return app;
    }

    public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        app.Services.AddTransient<IUrlRepository, UrlRepository>();

        return app;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IRetryService, RetryService>();
        app.Services.AddScoped<IShortStringGenService, ShortStringGenService>();

        return app;
    }

    public static WebApplicationBuilder ConfigureMiddleware(this WebApplicationBuilder app)
    {
        app.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return app;
    }

    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder app)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        app.Host.UseSerilog();

        return app;
    }
}