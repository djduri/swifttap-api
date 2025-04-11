using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Commands.UpdateCard;
// Include properties to be used as input for the command
public sealed record UpdateCardCommand(string? PhoneNumber,
                                       string? Email,
                                       bool IsPhoneNumberShareable,
                                       bool IsEmailShareable) : ICommand<long>;