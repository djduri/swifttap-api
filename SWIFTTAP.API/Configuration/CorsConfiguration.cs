using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class CorsConfiguration
{
	private const string SettingsCorsPolcyName = "_cors_policy_settings";

	public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
	{
		var corsSettings = services.AddAndGetSettings<CorsSettings>(configuration);

		services.AddCors(options => options.AddPolicy(SettingsCorsPolcyName, builder =>
		{
			builder = (corsSettings.Origins.Any())
				? builder.WithOrigins(corsSettings.Origins.ToArray())
				: builder.SetIsOriginAllowed(x => true);

			builder = (corsSettings.Headers.Any())
				? builder.WithHeaders(corsSettings.Headers.ToArray())
				: builder.AllowAnyHeader();

			builder = (corsSettings.Methods.Any())
				? builder.WithMethods(corsSettings.Methods.ToArray())
				: builder.AllowAnyMethod();

			builder = (corsSettings.AllowCredentials)
				? builder.AllowCredentials()
				: builder.DisallowCredentials();

			if (corsSettings.AllowWildcardOrigins)
				builder = builder.SetIsOriginAllowedToAllowWildcardSubdomains();

			builder.Build();
		}));

		return services;
	}

	public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
	{
		app.UseCors(SettingsCorsPolcyName);

		return app;
	}
}
