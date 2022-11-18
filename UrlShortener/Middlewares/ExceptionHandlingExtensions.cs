using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Middlewares
{
    public static class ExceptionHandlingExtensions
    {
        public static ProblemDetails CreateProblemDetails<T>(this T exception, int statusCode) where T : Exception
        {
            return new ProblemDetails()
            {
                Status = statusCode,
                Title = exception.GetType().Name,
                Detail = exception.Message
            };
        }
    }
}
