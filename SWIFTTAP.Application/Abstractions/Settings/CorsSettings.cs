using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class CorsSettings : IValidatableSettings
{
	public IReadOnlyCollection<string> Origins { get; init; } = [];
	public IReadOnlyCollection<string> Methods { get; init; } = [];
	public IReadOnlyCollection<string> Headers { get; init; } = [];
	public bool AllowCredentials { get; init; }
	public bool AllowWildcardOrigins { get; init; }

    public bool Valid()
    {
		if (Origins is null) return false;

		if (Methods is null) return false;

		if (Headers is null) return false;

		return true;
    }
}
