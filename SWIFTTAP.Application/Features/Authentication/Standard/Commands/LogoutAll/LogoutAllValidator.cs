using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.LogoutAll;
public sealed class LogoutAllValidator : AbstractValidator<LogoutAllCommand>
{
    public LogoutAllValidator()
    {
        // Add validation rules here
    }
}
