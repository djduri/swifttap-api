using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.API.Attributes;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.Administration.Users.Commands.ChangePassword;
using SWIFTTAP.Application.Features.Administration.Users.Commands.CreateAdmin;
using SWIFTTAP.Application.Features.Administration.Users.Commands.CreateUser;
using SWIFTTAP.Application.Features.Administration.Users.Commands.DeleteUser;
using SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterConfirm;
using SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterUser;
using SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPassword;
using SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordAsAdmin;
using SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordConfirm;
using SWIFTTAP.Application.Features.Administration.Users.Commands.SelfDeleteUser;
using SWIFTTAP.Application.Features.Administration.Users.Commands.UpdateUser;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Application.Features.Administration.Users.Queries.GetAllUsers;
using SWIFTTAP.Application.Features.Administration.Users.Queries.GetUser;
using SWIFTTAP.Application.Features.Administration.Users.Queries.GetUserAsAdmin;
using SWIFTTAP.Application.Features.Administration.Users.Queries.IsUniqueNameAvailable;

namespace SWIFTTAP.API.Areas.Cms;

public class UserController : CmsController
{
    private readonly ISender _sender;

    public UserController(ISender sender) =>
        _sender = sender;

    [HttpGet()]
    [SwaggerOperation(OperationId = "GetUser")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser() =>    
       Ok(await _sender.Send(new GetUserQuery()));

    [AuthorizeRole(Roles.Admin)]
    [HttpGet("Id/AsAdmin")]
    [SwaggerOperation(OperationId = "GetUserAsAdmin")]
    public async Task<ActionResult<UserDetailsDTO>> GetUserAsAdmin([FromQuery] GetUserAsAdminQuery query) =>
        Ok(await _sender.Send(query));

    [AuthorizeRole(Roles.Admin)]
    [HttpGet("List")]
    [SwaggerOperation(OperationId = "ListUsers")]
    public async Task<ActionResult<RangedDTO<UserSimpleDTO>>> GetAllUsers([FromQuery] GetAllUsersQuery query) =>
        Ok(await _sender.Send(query));

    [AllowAnonymous]
    [HttpGet("IsUniqueNameAvailable")]
    [SwaggerOperation(OperationId = "GetIsUniqueNameAvailable")]
    public async Task<ActionResult<bool>> GetIsUniqueNameAvailable([FromQuery] IsUniqueNameAvailableQuery query) =>
        Ok(await _sender.Send(query));

    [AllowAnonymous]
    [HttpPost("User/Register")]
    [SwaggerOperation(OperationId = "PostUserRegister")]
    public async Task<ActionResult<long>> PostUserRegister(RegisterUserCommand command) =>
        Ok(await _sender.Send(command));

    [AuthorizeRole(Roles.Admin)]
    [HttpPost("User/Create")]
    [SwaggerOperation(OperationId = "PostUserCreate")]
    public async Task<ActionResult<long>> PostUserCreate(CreateUserCommand command) =>
     Ok(await _sender.Send(command));

    [AuthorizeRole(Roles.Admin)]
    [HttpPost("Admin/Create")]
    [SwaggerOperation(OperationId = "PostAdminCreate")]
    public async Task<ActionResult<long>> PostAdminCreate(CreateAdminCommand command) =>
        Ok(await _sender.Send(command));

    [HttpPut]
    [SwaggerOperation(OperationId = "PutUser")]
    public async Task<ActionResult<long>> PutUser(UpdateUserCommand command) =>
       Ok(await _sender.Send(command));

    [HttpPut("Password")]
    [SwaggerOperation(OperationId = "PutPassword")]
    public async Task<ActionResult<long>> PutPassword(ChangePasswordCommand command) =>
        Ok(await _sender.Send(command));

    [AuthorizeRole(Roles.Admin)]
    [HttpPut("Password/ResetAsAdmin")]
    [SwaggerOperation(OperationId = "PutResetPasswordAsAdmin")]
    public async Task<ActionResult<long>> PutResetPasswordAsAdmin(ResetPasswordAsAdminCommand command) =>
        Ok(await _sender.Send(command));

    /// <summary>
    /// [AllowAnonymous] Send reset token to email.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut("Password/Reset")]
    [SwaggerOperation(OperationId = "PutResetPassword")]
    public async Task<ActionResult<long>> PutResetPassword(ResetPasswordCommand command) =>
        Ok(await _sender.Send(command));

    /// <summary>
    /// [AllowAnonymous]
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut("Password/Reset/Confirm")]
    [SwaggerOperation(OperationId = "PutResetPasswordConfirm")]
    public async Task<ActionResult<long>> PutResetPasswordConfirm(ResetPasswordConfirmCommand command) =>
        Ok(await _sender.Send(command));

    /// <summary>
    /// [AllowAnonymous]
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut("User/Register/Confirm")]
    [SwaggerOperation(OperationId = "PutRegisterConfirm")]
    public async Task<ActionResult<long>> PutRegisterConfirm(RegisterConfirmCommand command) =>
        Ok(await _sender.Send(command));

    [AuthorizeRole(Roles.Admin)]
    [HttpDelete]
    [SwaggerOperation(OperationId = "DeleteUser")]
    public async Task<ActionResult<long>> DeleteUser(DeleteUserCommand command) =>
        Ok(await _sender.Send(command));

    [HttpDelete("SelfDelete")]
    [SwaggerOperation(OperationId = "SelfDeleteUser")]
    public async Task<ActionResult<long>> SelfDeleteUser(SelfDeleteUserCommand command) =>
        Ok(await _sender.Send(command));
}
