using Microsoft.Extensions.DependencyInjection;

namespace SWIFTTAP.Application.Authorization;

public static class AuthorizationPoliciesConfiguration
{
    public static void AddCustomAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {

        });
    }
}


