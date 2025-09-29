using Microsoft.AspNetCore.Http;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Services;

internal sealed class CurrentScopeService : ICurrentScopeService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentScopeService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Request.GetIpAddress();
    }

    public string? GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
    }

    public Language GetLanguage(Language fallback = Language.EN)
    {
        // Pobieramy nagłówek Accept-Language
        var acceptLanguageHeader = _httpContextAccessor.HttpContext?.Request.Headers.AcceptLanguage.ToString();

        // Jeśli nagłówek jest pusty, zwracamy domyślny język
        if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
            return fallback;

        // Dzielimy nagłówek na języki
        var requestLanguage = acceptLanguageHeader
            .Split(',')
            .Select(l => l.Split(';').First().Trim())
            .FirstOrDefault();

        // Jeśli nie udało się znaleźć języka, zwracamy domyślny
        if (requestLanguage is null)
            return fallback;

        // Próbujemy dopasować język z nagłówka do enumu
        if (Enum.TryParse(requestLanguage, true, out Language parsedLanguage))
        {
            return parsedLanguage;
        }

        // Jeśli nie znaleziono pasującego języka, zwracamy domyślny
        return fallback;
    }
}
