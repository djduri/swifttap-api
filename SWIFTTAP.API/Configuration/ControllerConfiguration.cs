using System.Text.Json.Serialization;

namespace SWIFTTAP.API.Configuration;

public static class ControllerConfiguration
{
	public static IServiceCollection AddCustomControllers(this IServiceCollection services)
	{
		services.AddControllers(options =>
		{
			options.ModelValidatorProviders.Clear();
		}).AddJsonOptions(options =>
		{
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		});

		return services;
	}

	public static IApplicationBuilder UseCustomControllers(this WebApplication app)
	{
		app.MapAreaControllerRoute("AreaCms", "Cms", "api/cms/{controller}/{action}");
		app.MapAreaControllerRoute("AreaMobileUser", "Mobile User", "api/mobile/user/{controller}/{action}");

		app.MapControllers();

		return app;
	}
}
