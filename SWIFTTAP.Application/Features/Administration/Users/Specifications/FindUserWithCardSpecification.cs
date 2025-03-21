using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Specifications;
internal class FindUserWithCardSpecification : Specification<User>
{
    public FindUserWithCardSpecification(long id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Card);
    }
}
