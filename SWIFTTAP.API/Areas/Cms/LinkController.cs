using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
using SWIFTTAP.Application.Features.Cards.Links.Commands.DeleteLink;
using SWIFTTAP.Application.Features.Cards.Links.Commands.UpdateLink;
using SWIFTTAP.Application.Features.Cards.Links.DTOs;
using SWIFTTAP.Application.Features.Cards.Links.Queries.GetLink;

namespace SWIFTTAP.API.Areas.Cms;

public class LinkController: CmsController
{
    private readonly ISender _sender;

    public LinkController(ISender sender) =>
        _sender = sender;

    [HttpGet()]
    [SwaggerOperation(OperationId = "GetLink")]
    public async Task<ActionResult<LinkDetailsDTO>> GetLink([FromQuery] GetLinkQuery query) =>
        Ok(await _sender.Send(query));      
 
    [HttpPost()]
    [SwaggerOperation(OperationId = "PostLink")]
    public async Task<ActionResult<long>> PostLink(CreateLinkCommand command) =>
        Ok(await _sender.Send(command));   

    [HttpPut]
    [SwaggerOperation(OperationId = "PutLink")]
    public async Task<ActionResult<long>> PutLink(UpdateLinkCommand command) =>
       Ok(await _sender.Send(command));    

    [HttpDelete]
    [SwaggerOperation(OperationId = "DeleteLink")]
    public async Task<ActionResult<long>> DeleteLink(DeleteLinkCommand command) =>
        Ok(await _sender.Send(command));
}
