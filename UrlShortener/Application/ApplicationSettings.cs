namespace UrlShortener.Application;

public class ApplicationSettings
{
    public string ApplicationName { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string Domain { get; set; } = null!;
}