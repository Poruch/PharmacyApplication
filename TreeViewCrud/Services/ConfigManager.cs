using Microsoft.Extensions.Configuration;
using System.IO;

public static class ConfigManager
{
    private static IConfigurationRoot _configuration;

    static ConfigManager()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = builder.Build();
    }

    public static string ConnectionString =>
        _configuration.GetConnectionString("PharmacyDb") ?? "";
}