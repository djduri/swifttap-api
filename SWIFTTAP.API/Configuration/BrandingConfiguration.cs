using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class BrandingConfiguration
{
    public static IServiceCollection AddCustomBranding(this IServiceCollection services)
    {
        services.AddSettings<BrandingSettings>();

        return services;
    }
}
