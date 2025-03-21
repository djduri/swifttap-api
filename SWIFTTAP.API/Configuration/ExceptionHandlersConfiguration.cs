using SWIFTTAP.API.Handlers;

namespace SWIFTTAP.API.Configuration;

public static class ExceptionHandlersConfiguration
{
	public static IServiceCollection AddCustomExceptionHandlers(this IServiceCollection services)
	{
		services.AddExceptionHandler<AuthorizationExceptionHandler>();
        services.AddExceptionHandler<AuthenticationExceptionHandler>();
        services.AddExceptionHandler<AccessDeniedExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
		services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
		services.AddExceptionHandler<EntityCreateExceptionHandler>();
		services.AddExceptionHandler<EntityDeleteExceptionHandler>();
		services.AddExceptionHandler<EntityUpdateExceptionHandler>();
		services.AddExceptionHandler<DomainExceptionHandler>();
		services.AddExceptionHandler<UnhandledExceptionHandler>();

		services.AddProblemDetails();

		return services;
	}

	public static IApplicationBuilder UseCustomExceptionHandlers(this IApplicationBuilder app)
	{
		app.UseExceptionHandler();

		return app;
	}
}
