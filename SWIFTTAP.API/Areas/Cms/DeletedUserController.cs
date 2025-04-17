using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.API.Attributes;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.Command.DeleteDeletedUser;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.Queries.GetAllDeletedUsers;

namespace SWIFTTAP.API.Areas.Cms;

public class DeletedUserController : CmsController
{
    private readonly ISender _sender;

    public DeletedUserController(ISender sender) =>
        _sender = sender;


    [AuthorizeRole(Roles.Admin)]
    [HttpGet("List")]
    [SwaggerOperation(OperationId = "ListDeletedUsers")]
    public async Task<ActionResult<RangedDTO<DeletedUserDTO>>> ListDeletedUsers([FromQuery] GetAllDeletedUsersQuery query) =>
        Ok(await _sender.Send(query));

    [AuthorizeRole(Roles.Admin)]
    [HttpDelete]
    [SwaggerOperation(OperationId = "DeleteDeletedUser")]
    public async Task<ActionResult<long>> DeleteDeletedUser(DeleteDeletedUserCommand command) =>
        Ok(await _sender.Send(command));

}
