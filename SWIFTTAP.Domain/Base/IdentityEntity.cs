using Microsoft.AspNetCore.Identity;

namespace SWIFTTAP.Domain.Base;

public abstract class IdentityEntity : IdentityUser<long>, IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
