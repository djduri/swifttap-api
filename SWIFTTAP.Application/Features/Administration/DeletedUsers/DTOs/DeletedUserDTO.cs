namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;
public sealed class DeletedUserDTO
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Reason { get; set; }
    public required DateTime CreatedAt { get; set; }
}
