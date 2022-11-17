﻿using MediatR;
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
        var applicationSettings = app.Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
        app.Services.AddSingleton(applicationSettings);

        var retrySettings = app.Configuration.GetSection("ApplicationRetrySettings").Get<RetrySettings>();
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
}