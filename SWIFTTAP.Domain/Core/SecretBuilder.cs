using System.Security.Cryptography;
using System.Text;

namespace SWIFTTAP.Domain.Core;
public static class SecretBuilder
{
    private static readonly string UppercaseCharset = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
    private static readonly string LowercaseCharset = "abcdefghijkmnopqrstuvwxyz";
    private static readonly string DigitsCharset = "123456789";
    private static readonly string SpecialCharset = "!@$?_-";

    private static readonly string TokenCharset = string.Concat(LowercaseCharset, UppercaseCharset, DigitsCharset);
    private static readonly string GuidCharset = string.Concat(LowercaseCharset, DigitsCharset);
    private static readonly string UserFriendlyCharset = "2346789ABCDEFGHJKLMNPQRTUVWXYZ";

    private static readonly PasswordSecretOptions DefaultPasswordOptions = new()
    {
        RequiredLength = 10,
        RequiredUniqueChars = 4,
        RequireDigit = true,
        RequireLowercase = true,
        RequireNonAlphanumeric = true,
        RequireUppercase = true
    };

    /// <summary>
    /// Generate a temporary password-like string based on the provided options.
    /// </summary>
    public static async Task<string> GeneratePasswordAsync(PasswordSecretOptions? options = null, CancellationToken cancellationToken = default)
    {
        var opts = options ?? DefaultPasswordOptions;

        var charsets = BuildCharsetsForPassword(opts);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var passwordCandidate = GenerateStringFromCharsets(charsets, opts.RequiredLength);

            if (passwordCandidate.Distinct().Count() >= opts.RequiredUniqueChars)
                return await Task.FromResult(passwordCandidate);
        }
    }

    /// <summary>
    /// Generate a random token-like string for API keys.
    /// </summary>
    public static string GenerateToken(int length)
    {
        return GenerateFromCharset(TokenCharset, length, false);
    }

    /// <summary>
    /// Generate a GUID-like token string with additional random parts.
    /// </summary>
    public static string GenerateGuidToken(int additionalPartsLength)
    {
        var builder = new StringBuilder();

        builder.Append(GenerateFromCharset(GuidCharset, additionalPartsLength, false));
        builder.Append('-');

        builder.Append(Guid.NewGuid().ToString());
        builder.Append('-');

        builder.Append(GenerateFromCharset(GuidCharset, additionalPartsLength, false));

        return builder.ToString();
    }

    /// <summary>
    /// Generate a random numeric PIN code.
    /// </summary>
    public static string GeneratePin(int length)
    {
        return GenerateFromCharset(DigitsCharset, length, shuffle: false);
    }

    /// <summary>
    /// Generate a user-friendly code consisting of digits and letters, excluding hard-to-recognize characters.
    /// </summary>
    public static string GenerateUserFriendlyCode(int length)
    {
        return GenerateFromCharset(UserFriendlyCharset, length, shuffle: false);
    }

    private static string GenerateFromCharset(string charset, int length, bool shuffle)
    {
        ArgumentException.ThrowIfNullOrEmpty(charset, nameof(charset));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length, nameof(length));

        var valueBuilder = new StringBuilder();

        while (valueBuilder.Length < length)
        {
            var charsetIndex = RandomNumberGenerator.GetInt32(charset.Length);
            valueBuilder.Append(charset[charsetIndex]);
        }

        return shuffle ? ShuffleString(valueBuilder.ToString()) : valueBuilder.ToString();
    }

    private static string GenerateStringFromCharsets(IEnumerable<string> charsets, int length)
    {
        if (!charsets.Any()) throw new ArgumentException("Charsets cannot be empty.", nameof(charsets));

        var charsetArray = charsets.ToArray();
        var distribution = GenerateDistribution(length, charsetArray.Length);

        var valueBuilder = new StringBuilder();

        for (var i = 0; i < charsetArray.Length; i++)
        {
            var charset = charsetArray[i];
            foreach (var _ in Enumerable.Range(0, distribution[i]))
            {
                var charIndex = RandomNumberGenerator.GetInt32(charset.Length);
                valueBuilder.Append(charset[charIndex]);
            }
        }

        return ShuffleString(valueBuilder.ToString());
    }

    private static string ShuffleString(string value)
    {
        return new string(value.OrderBy(x => Guid.NewGuid()).ToArray());
    }

    private static int[] GenerateDistribution(int length, int groups)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, groups);

        var distribution = new int[groups];
        length -= groups;

        for (var i = 0; i < groups; i += 1)
        {
            var group = (i < groups - 1)
                ? RandomNumberGenerator.GetInt32(0, length)
                : length;

            length -= group;
            distribution[i] = 1 + group;
        }

        return distribution.OrderDescending().ToArray();
    }

    private static List<string> BuildCharsetsForPassword(PasswordSecretOptions opts)
    {
        var charsets = new List<string>();
        if (opts.RequireLowercase) charsets.Add(LowercaseCharset);
        if (opts.RequireUppercase) charsets.Add(UppercaseCharset);
        if (opts.RequireDigit) charsets.Add(DigitsCharset);
        if (opts.RequireNonAlphanumeric) charsets.Add(SpecialCharset);

        return charsets;
    }
}

public class PasswordSecretOptions
{
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireUppercase { get; set; }
}
