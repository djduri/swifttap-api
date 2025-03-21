using FluentValidation;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetUser;
public sealed class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {    
    }
}
