namespace UrlShortener.Services.Retry
{
    public record RetryOptions
    {
        public TimeSpan RetryDelay { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan RetryTimeout { get; set; }
        public Type[] ExceptionsToCatch { get; set; } = null!;
    }
}