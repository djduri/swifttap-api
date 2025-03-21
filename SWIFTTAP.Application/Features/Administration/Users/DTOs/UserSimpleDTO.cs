namespace SWIFTTAP.Application.Features.Administration.Users.DTOs;
public sealed class UserSimpleDTO
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Role { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required bool TwoFactorEnabled { get; set; }
}
