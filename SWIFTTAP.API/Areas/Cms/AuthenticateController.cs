using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.Authenticate;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.AuthenticateTwoFactorConfirm;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.CreateAndSendTwoFactorCode;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.Logout;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.LogoutAll;
using SWIFTTAP.Application.Features.Authentication.Standard.Commands.RefreshToken;
using SWIFTTAP.Application.Features.Authentication.Standard.DTOs;

namespace SWIFTTAP.API.Areas.Cms;

public class AuthenticateController : CmsController
{
    private readonly ISender _sender;

    public AuthenticateController(ISender sender) =>
        _sender = sender;

    [AllowAnonymous]
    [HttpPost()]
    [SwaggerOperation(OperationId = "PostAuthenticate")]
    public async Task<ActionResult<AuthenticateDTO>> PostAuthenticate(AuthenticateCommand command) =>
        Ok(await _sender.Send(command));

    [AllowAnonymous]
    [HttpPost("TwoFactorConfirm")]
    [SwaggerOperation(OperationId = "PostAuthenticateTwoFactorConfirm")]
    public async Task<ActionResult<TokenDTO>> PostAuthenticateTwoFactorConfirm(AuthenticateTwoFactorConfirmCommand command) =>
        Ok(await _sender.Send(command));

    [AllowAnonymous]
    [HttpPost("CreateAndSendTwoFactorCode")]
    [SwaggerOperation(OperationId = "PostCreateAndSendTwoFactorCode")]
    public async Task<ActionResult<string>> PostCreateAndSendTwoFactorCode(CreateAndSendTwoFactorCodeCommand command) =>
        Ok(await _sender.Send(command));

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    [SwaggerOperation(OperationId = "PostRefreshToken")]
    public async Task<ActionResult<TokenDTO>> PostRefreshToken(RefreshTokenCommand command) =>
        Ok(await _sender.Send(command));

    [HttpPost("Logout")]
    [SwaggerOperation(OperationId = "PostLogout")]    
    public async Task<ActionResult<Unit>> PostLogout(LogoutCommand command) =>
        Ok(await _sender.Send(command));


    [HttpPost("LogoutAll")]
    [SwaggerOperation(OperationId = "PostLogoutAll")]    
    public async Task<ActionResult<Unit>> PostLogoutAll(LogoutAllCommand command) =>
        Ok(await _sender.Send(command));
}
