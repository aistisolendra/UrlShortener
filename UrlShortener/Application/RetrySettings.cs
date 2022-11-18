namespace UrlShortener.Application
{
    public record RetrySettings
    {
        public DatabaseRetrySettings DatabaseRetrySettings { get; set; }
    }

    public record DatabaseRetrySettings
    {
        public int FirstTryDelayInSeconds { get; set; }
        public int RetryCount { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}