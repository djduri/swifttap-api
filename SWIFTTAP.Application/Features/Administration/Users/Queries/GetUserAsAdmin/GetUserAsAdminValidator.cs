using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetUserAsAdmin;
public sealed class GetUserAsAdminValidator : AbstractValidator<GetUserAsAdminQuery>
{
    public GetUserAsAdminValidator()
    {
        RuleFor(x => x.Id).IsIdentifier();
    }
}
