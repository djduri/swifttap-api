using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SWIFTTAP.Domain.Common;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Abstractions.Settings;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettingsSection = configuration.GetSection(nameof(DatabaseSettings));
        services.Configure<DatabaseSettings>(databaseSettingsSection);

        var databaseSettings = databaseSettingsSection.Get<DatabaseSettings>();

        var connectionString = databaseSettings?.ConnectionString ??
            throw new InvalidOperationException("Database connection string is missing. Please check your configuration.");

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));       

        return services;
    }

    public static IApplicationBuilder UseInfrastructureLayer(this IApplicationBuilder app, IServiceProvider service)
    {
        using var scope = service.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var databaseSettings = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;

        if (databaseSettings.AutoMigrate)
        {
            databaseContext.Database.Migrate();
        }
       
        app.UseRequestLocalization(options =>
        {
            var supportedLanguages = Enum.GetValues<Language>()
                .Select(l => l.ToString().ToLower())
                .ToArray();

            var defaultLanguage = Language.EN.ToString().ToLower();

            options.SetDefaultCulture(defaultLanguage);
            options.AddSupportedCultures(supportedLanguages);
            options.AddSupportedUICultures(supportedLanguages);
        });

        return app;
    }
}
