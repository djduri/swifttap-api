using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
internal sealed class FindUserWithUserKeysSpecification : Specification<User>
{
    public FindUserWithUserKeysSpecification(string email)
        : base(x => x.Email == email)
    {
        AddInclude(x => x.UserKeys);
        AddInclude(x => x.Card); // wazne dla dopisywania CardId do tokena
    }
}
