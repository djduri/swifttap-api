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
			options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequireDigit = false;               // Hasło musi zawierać cyfrę
            options.Password.RequireLowercase = false;           // Hasło musi zawierać małą literę
            options.Password.RequireUppercase = false;           // Hasło musi zawierać dużą literę
            options.Password.RequireNonAlphanumeric = false;     // Hasło musi zawierać znak specjalny
            options.Password.RequiredLength = 4;                 // Minimalna długość hasła
            options.Password.RequiredUniqueChars = 1;           // Liczba unikalnych znaków
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
