using FluentValidation;

namespace SWIFTTAP.Application.Features.ContactForms.ContactForms.Commands.SendContactForm
{
    public class SendContactFormValidator : AbstractValidator<SendContactFormCommand>
    {
        public SendContactFormValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(250);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Company)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Quantity)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Message)
                .MaximumLength(1000);
        }
    }
}
