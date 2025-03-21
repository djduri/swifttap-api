using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Authentication.Standard.DTOs;
public sealed class AuthenticateDTO
{
    public TokenDTO? TokenInfo { get; set; }
    public string? Auth2FAKey { get; set; }

    public AuthenticateDTO(TokenDTO token)
    {
        TokenInfo = token;
    }

    public AuthenticateDTO(string? auth2FAKey)
    {
        Auth2FAKey = auth2FAKey;
    }
}
