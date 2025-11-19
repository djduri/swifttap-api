using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.ContactForms.ContactForms.Commands.SendContactForm
{
    public record SendContactFormCommand(string Name,
                                         string Email,
                                         string Phone,
                                         string Company,
                                         string Quantity,
                                         string Message) : ICommand<bool>;
}