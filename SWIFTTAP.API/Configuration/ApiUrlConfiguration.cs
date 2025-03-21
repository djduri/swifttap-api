using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class ApiUrlConfiguration
{
    public static IServiceCollection AddCustomApiUrl(this IServiceCollection services)
    {
        services.AddSettings<ApiUrlSettings>();

        return services;
    }
}
