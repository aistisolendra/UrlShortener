using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Middlewares
{
    public static class ExceptionHandlingExtensions
    {
        public static ProblemDetails ToProblemDetails(this ArgumentNullException exception)
        {
            return new ProblemDetails()
            {
                Status = 500,
                Title = exception.GetType().Name,
                Detail = exception.Message
            };
        }
    }
}
