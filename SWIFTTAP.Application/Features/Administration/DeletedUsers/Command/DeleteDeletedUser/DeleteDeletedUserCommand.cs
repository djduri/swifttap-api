using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Command.DeleteDeletedUser;
// Include properties to be used as input for the command
public sealed record DeleteDeletedUserCommand(long Id) : ICommand<long>;