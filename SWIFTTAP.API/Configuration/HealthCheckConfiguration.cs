using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Infrastructure.Abstractions.Settings;

namespace SWIFTTAP.API.Configuration;

public static class HealthCheckConfiguration
{
    private const string Path = "/health";

    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = services.GetSettings<DatabaseSettings>(configuration);

        services.AddHealthChecks()
            .AddNpgSql(databaseSettings.ConnectionString!)
            .AddHangfire(config =>
            {
                config.MaximumJobsFailed = 1;
            });

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseHealthChecks(Path, new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
        else
        {
            app.UseHealthChecks(Path);
        }

        return app;
    }
}
