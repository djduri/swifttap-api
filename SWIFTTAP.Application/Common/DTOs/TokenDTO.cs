namespace SWIFTTAP.Application.Common.DTOs;
public class TokenDTO
{
    public required string Token { get; set; }
    public required DateTime TokenExpiration { get; set; }
    public required string RefreshToken { get; set; }
}
