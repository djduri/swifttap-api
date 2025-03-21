using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.API.Middlewares;

public sealed class UserActivityLoggingMiddleware : IMiddleware
{
    private readonly IUserService _userService;

    public UserActivityLoggingMiddleware(IUserService userService)
    {
        _userService = userService;
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var user = GetUserDataFromService();
        var userActivityLogEnricher = new UserActivityLogEnricher(user);

        using (LogContext.Push(userActivityLogEnricher))
        {
            return next(context);
        }
    }

    private UserData GetUserDataFromService()
    {
        var userId = _userService.GetAuthenticatedUserIdOrDefault();
        var userEmail = _userService.GetAuthenticatedUserEmailOrDefault();

        return new UserData(userId, userEmail);
    }

    private sealed class UserActivityLogEnricher : ILogEventEnricher
    {
        private readonly UserData _userData;

        public UserActivityLogEnricher(UserData userData)
        {
            _userData = userData;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var authenticationStatus = _userData.IsAuthenticated;
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("IsUserAuthenticated", authenticationStatus));

            if (_userData.UserId.HasValue)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("AuthenticatedUserId", _userData.UserId.Value));
            }

            if (!string.IsNullOrWhiteSpace(_userData.UserEmail))
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("AuthenticatedUserEmail", _userData.UserEmail));
            }
        }
    }

    private class UserData
    {
        public long? UserId { get; }
        public string? UserEmail { get; }
        public bool IsAuthenticated => UserId.HasValue && !string.IsNullOrWhiteSpace(UserEmail);

        public UserData(long? userId, string? userEmail)
        {
            UserId = userId;
            UserEmail = userEmail;
        }
    }
}

