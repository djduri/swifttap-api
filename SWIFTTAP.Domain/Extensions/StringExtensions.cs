using System.Text;

namespace SWIFTTAP.Domain.Extensions;
public static class StringExtensions
{
    public static bool IsEmpty(this string @this) => string.IsNullOrEmpty(@this);

    public static string EncodeToBase64(this string @this)
    {
        var bytes = Encoding.UTF8.GetBytes(@this);
        return Convert.ToBase64String(bytes);
    }

    public static string DecodeFromBase64(this string @this)
    {
        var bytes = Convert.FromBase64String(@this);
        return Encoding.UTF8.GetString(bytes);
    }
}
