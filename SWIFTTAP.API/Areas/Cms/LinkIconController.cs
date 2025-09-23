using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.Cards.LinkIcons.Commands.UpdateLinkIcon;
using SWIFTTAP.Application.Features.Cards.LinkIcons.Queries.GetLinkIcon;
using System.Net;

namespace SWIFTTAP.API.Areas.Cms;

public class LinkIconController : CmsController
{
    private readonly ISender _sender;

    public LinkIconController(ISender sender) =>
        _sender = sender;

    [AllowAnonymous]
    [HttpGet("LinkId")]
    [SwaggerOperation(OperationId = "GetLinkIcon")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLinkIcon", typeof(FileResult), "image/png")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLinkIcon", typeof(FileResult), "image/svg+xml")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLinkIcon", typeof(FileResult), "image/jpeg")]
    [SwaggerResponse((int)HttpStatusCode.OK, "GetLinkIcon", typeof(FileResult), "image/bmp")]

    public async Task<FileResult> GetLinkIcon([FromQuery] GetLinkIconQuery command) =>
        File(await _sender.Send(command));    

    [HttpPut()]
    [SwaggerOperation(OperationId = "PutLinkIcon")]
    public async Task<ActionResult<long>> PutLinkIcon([FromForm] UpdateLinkIconCommand command) =>
        Ok(await _sender.Send(command));

}
