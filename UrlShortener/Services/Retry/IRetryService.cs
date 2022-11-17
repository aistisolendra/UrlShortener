namespace UrlShortener.Services.Retry
{
    public interface IRetryService
    {
        Task<T> RetryAsync<T>(Func<Task<T>> func, RetryOptions retryOptions);
        Task RetryAsync(Func<Task> func, RetryOptions retryOptions);
        Task<T> RetryAsync<T>(Func<Task<T>> func, TimeSpan retryDelay, int retryCount, TimeSpan timeout, params Type[] exceptionsToCatch);
        Task RetryAsync(Func<Task> func, TimeSpan retryDelay, int retryCount, TimeSpan timeout, params Type[] exceptionsToCatch);
    }
}
