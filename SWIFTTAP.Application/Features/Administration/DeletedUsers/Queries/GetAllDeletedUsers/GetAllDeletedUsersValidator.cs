using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Queries.GetAllDeletedUsers;
public sealed class GetAllDeletedUsersValidator : AbstractValidator<GetAllDeletedUsersQuery>
{
    public GetAllDeletedUsersValidator()
    {
        When(x => x.SortingArguments is not null, () =>
        {
            RuleFor(x => x.SortingArguments!).SetValidator(new SortingArgumentsValidator());
        });
        When(x => x.PaginationArguments is not null, () =>
        {
            RuleFor(x => x.PaginationArguments!).SetValidator(new PaginationArgumentsValidator());
        });
    }
}
