using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetAllUsers;
public class GetAllUsersValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersValidator()
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
