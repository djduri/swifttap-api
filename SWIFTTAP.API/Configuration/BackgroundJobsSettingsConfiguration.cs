using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Jobs.Interfaces;

namespace SWIFTTAP.API.Configuration;

public static class BackgroundJobsSettingsConfiguration
{
    public static IServiceCollection AddCustomBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var backgroundJobSettings = services.AddAndGetSettings<BackgroundJobsSettings>(configuration);

        if (!backgroundJobSettings.IsEnabled)
            return services;

        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(storage => storage.UseNpgsqlConnection(backgroundJobSettings.ConnectionString), new PostgreSqlStorageOptions
            {
                SchemaName = "Hangfire"
            }));

        services.AddHangfireServer();

        return services;
    }

    public static IApplicationBuilder UseCustomBackgroundJobs(this IApplicationBuilder app, IHostEnvironment host, IServiceProvider service)
    {
        var backgroundJobsSettings = service.GetRequiredService<IOptions<BackgroundJobsSettings>>().Value;

        if (!backgroundJobsSettings.IsEnabled)
            return app;

        _ = service.GetRequiredService<IBackgroundJobClient>();

        if (host.IsDevelopment() && backgroundJobsSettings.IsDashboardEnabled)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [ new HangfireDashboardAuthorizationSkipFilter() ],
                DashboardTitle = "SWIFTTAP Hangfire Dashboard",
                DarkModeEnabled = true,
                IsReadOnlyFunc = (DashboardContext context) => true,
            });
        }

        RegisterJob<IRefreshTokenExpiredCleanupBackgroundJob>(backgroundJobsSettings.CronJobSchedules.RefreshTokenExpiredCleanupSchedule);
        RegisterJob<IUserCountStatisticBackgroundJob>(backgroundJobsSettings.CronJobSchedules.UserCountStatisticSchedule);

        return app;
    }
    internal static void RegisterJob<TBackgroundJob>(string cronExpression, bool forceRemove = true) where TBackgroundJob : IBackgroundJob
    {
        var backgroundJobName = typeof(TBackgroundJob).Name;

        if (forceRemove) RecurringJob.RemoveIfExists(backgroundJobName);

        RecurringJob.AddOrUpdate<TBackgroundJob>(backgroundJobName, service => service.Run(CancellationToken.None), cronExpression);
    }
    internal sealed class HangfireDashboardAuthorizationSkipFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context) => true;
    }
}
