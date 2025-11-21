using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.Application.Services;

// https://ip-api.com/docs/api:json
internal sealed class IpGeolocationService : IIpGeolocationService
{
    private static readonly Uri IpApiUri = new Uri("http://ip-api.com/");
    private static readonly int MemoryCacheDays = 3;
    private static string MemoryCacheKey(string ipAddress) => $"{nameof(IpGeolocationService)}_{ipAddress}";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<IpGeolocationService> _logger;

    public IpGeolocationService(IHttpClientFactory httpClientFactory, 
                                IMemoryCache memoryCache,
                                ILogger<IpGeolocationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<IpGeolocationResult?> GetGeolocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        // Sprawdzenie cache
        if (_memoryCache.TryGetValue(MemoryCacheKey(ipAddress), out IpGeolocationResult? geolocation))        
            return geolocation;        

        using var client = _httpClientFactory.CreateClient();

        var requestUri = new Uri(IpApiUri, $"json/{ipAddress}?fields=3719679");

        var response = await client.GetAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("IP API returned non-success status {StatusCode} for IP {IpAddress}", response.StatusCode, ipAddress);
            return null;
        }

        geolocation = await response.Content.ReadFromJsonAsync<IpGeolocationResult>(cancellationToken);

        if (geolocation is null)
        {
            _logger.LogError("IP geolocation API returned null data for {IpAddress} (deserialize error)", ipAddress);
            return null;
        }

        if (geolocation.Status.Equals("fail", StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogError("Geolocation lookup failed for IP {IpAddress}: {Message}", ipAddress, geolocation.Message);
            return null;
        }

        // Dodanie do cache
        _memoryCache.Set(MemoryCacheKey(ipAddress), geolocation, TimeSpan.FromDays(MemoryCacheDays));

        return geolocation;
    }
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
