using Microsoft.AspNetCore.Http;

namespace SWIFTTAP.Application.Extensions;
public static class HttpRequestExtensions
{
    private static readonly string[] IpForwardHeaders =
    {
        "X-Forwarded-For",
        "X-Coming-From",
        "HTTP_X_FORWARDED_FOR",
        "REMOTE_ADDR"
    };

    public static string? GetIpAddress(this HttpRequest @this)
    {
        // Sprawdzanie nagłówków HTTP
        var ipAddress = GetIpFromHeaders(@this);
        if (!string.IsNullOrEmpty(ipAddress)) return ipAddress;

        // Jeśli nie znaleziono IP w nagłówkach, sprawdzamy połączenie
        ipAddress = @this.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        if (!string.IsNullOrEmpty(ipAddress)) return ipAddress;

        // Jeśli brak IP w połączeniu, sprawdzamy lokalny adres
        ipAddress = @this.HttpContext?.Connection?.LocalIpAddress?.ToString();
        if (!string.IsNullOrEmpty(ipAddress)) return ipAddress;

        return null;
    }

    // Nowa metoda do uzyskiwania adresu IP z nagłówków
    private static string? GetIpFromHeaders(HttpRequest request)
    {
        return IpForwardHeaders
            .Select(header => request.Headers[header].FirstOrDefault())
            .FirstOrDefault(ip => !string.IsNullOrEmpty(ip))?.Split(',').FirstOrDefault()?.Trim();
    }
}

