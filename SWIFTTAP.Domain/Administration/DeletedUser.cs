using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Administration;
public sealed class DeletedUser : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Reason { get; private set; }

    public DeletedUser SetName(string name)
    {
        if (name.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.DeletedUser.InvalidName);

        Name = name;
        return this;
    }
    public DeletedUser SetEmail(string email)
    {
        if (email.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.DeletedUser.InvalidEmail);

        Email = email;
        return this;
    }

    public DeletedUser SetReason(string reason)
    {
        if (reason.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.DeletedUser.InvalidReason);

        Reason = reason;
        return this;
    }

    private DeletedUser() { }

    public static class Factory
    {
        public static DeletedUser Create(string name, string email, string reason)
        {
            return new DeletedUser()
                .SetName(name)
                .SetReason(reason)
                .SetEmail(email);
        }
    }
}
