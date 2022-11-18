using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace UrlShortener.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public GlobalExceptionHandlingMiddleware()
        {
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: { Exception }", e.Message);
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
                ArgumentNullException e => e.CreateProblemDetails(500),
                InvalidOperationException e => e.CreateProblemDetails(500),
                ValidationException e => e.CreateProblemDetails(400),
                _ => new ProblemDetails()
                {
                    Status = 500,
                    Title = "Unknown exception",
                    Detail = "Unknown exception happened"
                }
            };
        }
    }
}