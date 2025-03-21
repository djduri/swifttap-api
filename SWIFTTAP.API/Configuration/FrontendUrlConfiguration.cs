using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class FrontendUrlConfiguration
{
    public static IServiceCollection AddCustomFrontendUrl(this IServiceCollection services)
    {
        services.AddSettings<FrontendUrlSettings>();

        return services;
    }
}
