using Microsoft.AspNetCore.RateLimiting;

namespace SWIFTTAP.API.Configuration;

public static class RateLimiterConfiguration
{
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        return services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("ContactFormLimiter", opt =>
            {
                opt.PermitLimit = 5; // Maksymalnie 5 żądań
                opt.Window = TimeSpan.FromMinutes(1); // W ciągu 1 minuty
            });
        });
    }

    public static IApplicationBuilder UseCustomRateLimiter(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}
