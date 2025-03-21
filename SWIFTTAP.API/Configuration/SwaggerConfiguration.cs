using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using SWIFTTAP.API.Filters;

namespace SWIFTTAP.API.Configuration;

public static class SwaggerConfiguration
{
	public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(config =>
		{
			var assemblyVersion = typeof(SwaggerConfiguration)?.Assembly?.GetName()?.Version?.ToString() ?? "b.d";

			config.SwaggerDoc("Cms", new OpenApiInfo
			{
				Title = "SWIFT TAP - CMS",
				Version = $"cms_v_{assemblyVersion}"
			});

			//config.SwaggerDoc("Mobile User", new OpenApiInfo
			//{
			//	Title = "SWIFT TAP - Mobile User",
			//	Version = $"mobile_user_v_{assemblyVersion}"
			//});

            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				In = ParameterLocation.Header
			});

			config.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
						Scheme = "Bearer",
						Name = "Authorization",
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.ApiKey
					},
					new[] { "readAccess", "writeAccess" }
				}
			});

			config.OperationFilter<AuthorizationPolicyOperationFilter>();
			config.OperationFilter<RequiredRolesOperationFilter>();

            config.SupportNonNullableReferenceTypes();
			config.NonNullableReferenceTypesAsRequired();
        });

		services.AddFluentValidationRulesToSwagger();

		return services;
	}

	public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(config =>
			{
				config.SwaggerEndpoint("/swagger/Cms/swagger.json", "Cms");
				//config.SwaggerEndpoint("/swagger/Mobile%20User/swagger.json", "Mobile User");
			});
		}

		return app;
	}
}
