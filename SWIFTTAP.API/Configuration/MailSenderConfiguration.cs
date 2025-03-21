using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class MailSenderConfiguration
{
    public static IServiceCollection AddCustomMailSender(this IServiceCollection services)
    {
        services.AddSettings<MailSenderSettings>();

        return services;
    }
}
