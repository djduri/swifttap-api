using FluentValidation;

namespace SWIFTTAP.Application.Extensions;

internal static class IRuleBuilderExtensions
{
    public static IRuleBuilder<T, string> IsEmail<T>(this IRuleBuilder<T, string> @this, int maxLength = 255)
    { 
        return @this.NotEmpty().EmailAddress().MaximumLength(maxLength);
    }

    public static IRuleBuilderOptions<T, long> IsIdentifier<T>(this IRuleBuilder<T, long> @this)
    {
        return @this.NotEmpty().GreaterThan(0);
    }

    public static IRuleBuilderOptions<T, long?> IsIdentifier<T>(this IRuleBuilder<T, long?> @this)
    {
        return @this.NotEmpty().GreaterThan(0);
    }

    public static IRuleBuilderOptions<T, string?> ShouldMatchColorCode<T>(this IRuleBuilder<T, string?> @this)
    {   
        return @this.Matches(@"^#([0-9a-fA-F]{3}|[0-9a-fA-F]{4}|[0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");
    }
}
