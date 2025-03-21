using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.Application.Services;

// Docs: https://ip-api.com/docs/api:json
// TODO: Check if the provided IpAddress is valid
internal sealed class IpGeolocationService : IIpGeolocationService
{
    private static readonly Uri BaseUri = new Uri("http://ip-api.com/");

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<IpGeolocationService> _logger;

    public IpGeolocationService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, ILogger<IpGeolocationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<IpGeolocationResult?> GetGeolocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(CacheKey(ipAddress), out IpGeolocationResult? geolocation))
        {
            return geolocation;
        }

        using var client = _httpClientFactory.CreateClient();

        var requestUri = new Uri(BaseUri, $"json/{ipAddress}?fields=3719679");

        var response = await client.GetAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("External API responded with status {StatusCode} for {IpAdress}", response.StatusCode, ipAddress);
            return null;
        }

        geolocation = await response.Content.ReadFromJsonAsync<IpGeolocationResult>(cancellationToken);
        if (geolocation is null)
        {
            _logger.LogError("Failed to deserialize the response for {IpAddress}", ipAddress);
            return null;
        }

        if (geolocation.Status.Equals("fail", StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogError("Ip address based geolocation lookup failed for {IpAddress} with {Message}", ipAddress, geolocation.Message);
            return null;
        }

        _memoryCache.Set(CacheKey(ipAddress), geolocation, TimeSpan.FromDays(3));

        return geolocation;
    }

    private static string CacheKey(string ipAddress) => $"{nameof(IpGeolocationService)}_{ipAddress}";
}

public sealed record IpGeolocationResult(
    string Status,
    string Message,
    string Continent,
    string ContinentCode,
    string Country,
    string CountryCode,
    string Region,
    string RegionName,
    string City,
    string District,
    string Zip,
    double Lat,
    double Lon,
    string Timezone
);
