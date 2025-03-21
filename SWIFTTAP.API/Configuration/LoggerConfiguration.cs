using Serilog;
using SWIFTTAP.API.Middlewares;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Configuration;

public static class LoggerConfiguration
{
	public static IHostBuilder UseCustomLogger(this IHostBuilder builder, IServiceCollection services)
	{
		builder.UseSerilog((context, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration));

		services.AddTransient<UserActivityLoggingMiddleware>();

		return builder;
	}

	public static IApplicationBuilder UseCustomLogger(this IApplicationBuilder app)
	{
		app.UseMiddleware<UserActivityLoggingMiddleware>();

		app.UseSerilogRequestLogging(config =>
		{
			config.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			{
				diagnosticContext.Set("User-Agent", httpContext.Request.Headers.UserAgent);
				diagnosticContext.Set("Accept-Language", httpContext.Request.Headers.AcceptLanguage);
				diagnosticContext.Set("IP-Address", httpContext.Request.GetIpAddress());
			};
		});

		return app;
	}
}
