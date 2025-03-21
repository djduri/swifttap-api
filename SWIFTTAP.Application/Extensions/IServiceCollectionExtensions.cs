using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSettings<TSettings>(this IServiceCollection @this)
        where TSettings : class, IValidatableSettings
    {
        ArgumentNullException.ThrowIfNull(@this);

        var settingsName = typeof(TSettings).Name;

        return @this.AddOptions<TSettings>()
                    .BindConfiguration(settingsName)
                    .Validate(settings => settings.Valid(), $"Settings validation failed for: \"{settingsName}\".")
                    .ValidateOnStart()
                    .Services;
    }

    public static TSettings GetSettings<TSettings>(this IServiceCollection @this, IConfiguration configuration)
        where TSettings : class, IValidatableSettings
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var section = configuration.GetSection(typeof(TSettings).Name);
        var settings = section.Get<TSettings>();

        if (settings is null || !settings.Valid())
            throw new InvalidOperationException($"Settings validation failed for section: {typeof(TSettings).Name}.");

        return settings;
    }

    public static TSettings AddAndGetSettings<TSettings>(this IServiceCollection services, IConfiguration configuration)
                where TSettings : class, IValidatableSettings
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.AddSettings<TSettings>();  // Register settings
        return services.GetSettings<TSettings>(configuration);  // Retrieve and validate settings
    }
}
