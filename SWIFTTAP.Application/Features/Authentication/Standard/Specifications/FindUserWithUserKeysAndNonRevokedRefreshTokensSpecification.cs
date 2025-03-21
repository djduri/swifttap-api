using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
internal sealed class FindUserWithUserKeysAndNonRevokedRefreshTokensSpecification : Specification<User>
{
    public FindUserWithUserKeysAndNonRevokedRefreshTokensSpecification(string email)
        : base(x => x.Email == email)
    {
        AddInclude(x => x.UserKeys);
        AddInclude(x => x.RefreshTokens.Where(x => x.RevokedAt == null));
    }
}
