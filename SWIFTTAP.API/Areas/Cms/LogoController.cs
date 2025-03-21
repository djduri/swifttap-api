using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.Cards.Logo.Commands.UpdateLogo;
using SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogo;
using SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByGuid;
using System.Net;

namespace SWIFTTAP.API.Areas.Cms;

public class LogoController: CmsController
{
    private readonly ISender _sender;

    public LogoController(ISender sender) =>
        _sender = sender;

    [HttpGet("Logo/UserId")]
    [SwaggerOperation(OperationId = "GetUserLogo")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetUserLogo([FromQuery] GetLogoQuery command) =>
        File(await _sender.Send(command));

    [AllowAnonymous]
    [HttpGet("Logo/Guid")]
    [SwaggerOperation(OperationId = "GetUserLogoByGuid")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetIndustryLogo", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetUserLogoByGuid([FromQuery] GetLogoByGuidQuery command) =>
    File(await _sender.Send(command));

    [HttpPut("Logo")]
    [SwaggerOperation(OperationId = "PutUserLogo")]
    public async Task<ActionResult<long>> PutUserLogo([FromForm] UpdateLogoCommand command) =>
        Ok(await _sender.Send(command));

}
