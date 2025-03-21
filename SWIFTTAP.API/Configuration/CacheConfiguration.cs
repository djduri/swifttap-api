namespace SWIFTTAP.API.Configuration;

public static class CacheConfiguration
{
    public static IServiceCollection AddCustomCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        return services;
    }
}
