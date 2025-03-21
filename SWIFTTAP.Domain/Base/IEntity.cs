namespace SWIFTTAP.Domain.Base;

public interface IEntity : IAuditableEntity
{
    public long Id { get; }
}
