using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class EmailTemplatesConfiguration
{
    public static IServiceCollection AddCustomEmailTemplates(this IServiceCollection services)
    {
        services.AddSettings<EmailTemplatesSettings>();

        return services;
    }
}
