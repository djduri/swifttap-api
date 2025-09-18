using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterConfirm;
public sealed record RegisterConfirmCommand(string Email,
                                            string Token) : ICommand<long>;
