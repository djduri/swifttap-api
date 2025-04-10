using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.Cards.Logo.Commands.UpdateLogo;
using SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogo;
using SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByGuid;
using SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByUniqueName;
using System.Net;

namespace SWIFTTAP.API.Areas.Cms;

public class LogoController: CmsController
{
    private readonly ISender _sender;

    public LogoController(ISender sender) =>
        _sender = sender;

    [HttpGet("Logo/CardId")]
    [SwaggerOperation(OperationId = "GetLogo")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogo", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogo", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogo", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogo", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetLogo([FromQuery] GetLogoQuery command) =>
        File(await _sender.Send(command));

    [AllowAnonymous]
    [HttpGet("Logo/Guid")]
    [SwaggerOperation(OperationId = "GetLogoByGuid")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByGuid", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByGuid", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByGuid", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByGuid", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetLogoByGuid([FromQuery] GetLogoByGuidQuery command) =>
        File(await _sender.Send(command));


    [AllowAnonymous]
    [HttpGet("Logo/UniqueName")]
    [SwaggerOperation(OperationId = "GetLogoByUniqueName")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByUniqueName", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByUniqueName", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByUniqueName", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLogoByUniqueName", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetLogoByUniqueName([FromQuery] GetLogoByUniqueNameQuery command) =>
        File(await _sender.Send(command));

    [HttpPut("Logo")]
    [SwaggerOperation(OperationId = "PutUserLogo")]
    public async Task<ActionResult<long>> PutUserLogo([FromForm] UpdateLogoCommand command) =>
        Ok(await _sender.Send(command));

}
