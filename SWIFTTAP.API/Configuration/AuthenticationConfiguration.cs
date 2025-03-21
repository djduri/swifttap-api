using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Database;
using System.Text;

namespace SWIFTTAP.API.Configuration;

public static class AuthenticationConfiguration
{
	public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var authenticationSettings = services.AddAndGetSettings<AuthenticationSettings>(configuration);

		services.AddIdentity<User, IdentityRole<long>>(options =>
		{
			options.SignIn.RequireConfirmedAccount = true;

            options.Password.RequireDigit = true;               // Hasło musi zawierać cyfrę
            options.Password.RequireLowercase = true;           // Hasło musi zawierać małą literę
            options.Password.RequireUppercase = true;           // Hasło musi zawierać dużą literę
            options.Password.RequireNonAlphanumeric = true;     // Hasło musi zawierać znak specjalny
            options.Password.RequiredLength = 8;                 // Minimalna długość hasła
            options.Password.RequiredUniqueChars = 6;           // Liczba unikalnych znaków
        })
		.AddDefaultTokenProviders()
		.AddEntityFrameworkStores<DatabaseContext>();

		var secret = Encoding.ASCII.GetBytes(authenticationSettings.JwtSecret!);

		if (authenticationSettings.IsDevelopment)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(secret),
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true
					};
				});
		}
		else
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(secret),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidAudiences = authenticationSettings.Audiences,
						ValidIssuers = authenticationSettings.Issuers,
					};
				});
		}
		services.AddCustomAuthorizationPolicies();
		return services;
	}
}
