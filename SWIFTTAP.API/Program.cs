using SWIFTTAP.API.Configuration;
using SWIFTTAP.Application;
using SWIFTTAP.Infrastructure;

namespace SWIFTTAP.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Zbiorcza konfiguracja us³ug
        ConfigureServices(builder);

        var app = builder.Build();

        // Konfiguracja aplikacji
        ConfigureApp(app);

        app.Run();
    }

    // Jedna metoda odpowiedzialna za konfiguracjê wszystkich us³ug
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Host.UseCustomLogger(builder.Services);

        builder.Services.AddCustomAuthentication(builder.Configuration);

        builder.Services.AddCustomExceptionHandlers();

        builder.Services.AddCustomControllers();

        builder.Services.AddCustomCors(builder.Configuration);

        builder.Services.AddCustomSwagger();

        builder.Services.AddHttpClient();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCustomCaching();

        builder.Services.AddApplicationLayer(builder.Configuration);

        builder.Services.AddInfrastructureLayer(builder.Configuration);

        builder.Services.AddCustomDataSeeder();

        builder.Services.AddCustomMailSender();

        builder.Services.AddCustomContactForm();

        builder.Services.AddCustomBranding();

        builder.Services.AddCustomEmailTemplates();

        builder.Services.AddCustomFrontendUrl();

        builder.Services.AddCustomApiUrl();

        builder.Services.AddCustomBackgroundJobs(builder.Configuration);

        builder.Services.AddCustomHealthChecks(builder.Configuration);
    }

    // Metoda konfiguruj¹ca aplikacjê
    private static void ConfigureApp(WebApplication app)
    {
        app.UseCustomHealthChecks(app.Environment);
        app.UseCustomExceptionHandlers();
        app.UseInfrastructureLayer(app.Services);
        app.UseCustomDataSeeder(app.Services);
        app.UseCustomSwagger(app.Environment);
        app.UseCustomBackgroundJobs(app.Environment, app.Services);
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCustomCors();
        app.UseCustomLogger();
        app.UseAuthorization();
        app.UseCustomControllers();
    }
}

