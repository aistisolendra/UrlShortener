using MediatR;
using UrlShortener.DataAccess.Base;
using UrlShortener.DataAccess.Repositories;

namespace UrlShortener.Application;

public static class WebServices
{
    public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder app)
    {
        var applicationSettings = app.Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
        app.Services.AddSingleton(applicationSettings);

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

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        app.Services.AddTransient<IUrlRepository, UrlRepository>();
        app.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return app;
    }
}