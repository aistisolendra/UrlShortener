﻿using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleError(e, context);
            }
        }

        private async Task HandleError(Exception exception, HttpContext context)
        {
            if (exception is OperationCanceledException || exception.InnerException is OperationCanceledException)
            {
                context.Response.StatusCode = 499;

                return;
            }

            var problemDetails = GetDetailedException(exception);
            var serializedProblemDetails = JsonSerializer.Serialize(problemDetails);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                problemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);

            await context.Response.WriteAsync(serializedProblemDetails, context.RequestAborted);
        }

        private ProblemDetails GetDetailedException(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException e => e.ToProblemDetails()
            };
        }
    }
}
