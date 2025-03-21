using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Specifications;
internal sealed class FindUserWithNonRevokedRefreshTokensSpecification : Specification<User>
{
    public FindUserWithNonRevokedRefreshTokensSpecification(string email)
        : base(x => x.Email == email)
    {
        AddInclude(x => x.RefreshTokens.Where(x => x.RevokedAt == null));
    }

    public FindUserWithNonRevokedRefreshTokensSpecification(long userId)
    : base(x => x.Id == userId)
    {
        AddInclude(x => x.RefreshTokens.Where(x => x.RevokedAt == null));
    }
}
