using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.API.Configuration;

public static class DataSeederConfiguration
{
	public static IServiceCollection AddCustomDataSeeder(this IServiceCollection services)
	{
		services.AddSettings<DataSeederSettings>();

		return services;
	}

	public static IApplicationBuilder UseCustomDataSeeder(this IApplicationBuilder app, IServiceProvider service)
	{
		using var scope = service.CreateScope();
		var dataSeederService = scope.ServiceProvider.GetRequiredService<IDataSeederService>();
		dataSeederService.Seed().Wait();

		return app;
	}
}
