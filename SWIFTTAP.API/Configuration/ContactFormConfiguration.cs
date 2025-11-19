using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class ContactFormConfiguration
{
    public static IServiceCollection AddCustomContactForm(this IServiceCollection services)
    {
        services.AddSettings<ContactFormSettings>();

        return services;
    }
}
