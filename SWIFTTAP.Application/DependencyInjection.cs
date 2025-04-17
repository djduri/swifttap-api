using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Application.Features.Cards.Links.DTOs;
using SWIFTTAP.Application.Features.Cards.Themes.DTOs;
using SWIFTTAP.Application.Pipelines;
using SWIFTTAP.Application.Services;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IMailSenderService, MailSenderService>();

        services.Scan(x => x.FromAssemblyOf<Assembly>()
            .AddClasses(y => y.AssignableTo<IScopedAppService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(x => x.FromAssemblyOf<Assembly>()
            .AddClasses(y => y.AssignableTo<ISingletonAppService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(x => x.FromAssemblyOf<Assembly>()
            .AddClasses(y => y.AssignableTo<IBackgroundJob>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.AddValidatorsFromAssemblyContaining<Assembly>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Assembly>();

			config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
		});

        services.AddSingleton(provider => new MapperConfiguration(mapper =>
        {
            mapper.AddProfile(new UserProfile());
            mapper.AddProfile(new DeletedUserProfile());
            mapper.AddProfile(new LinkProfile());
            mapper.AddProfile(new ThemeProfile());
            mapper.AddProfile(new CardProfile());

        }).CreateMapper());

        return services;
    }
}

public sealed class Assembly
{
}
