using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.API.Attributes;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Application.Features.Cards.Cards.Commands.UpdateCard;
using SWIFTTAP.Application.Features.Cards.Cards.DTOs;
using SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCard;
using SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCardByUniqueName;
using SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLink;
using SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLinkAsAdmin;
using SWIFTTAP.Application.Features.Cards.Links.Commands.UpdateLink;

namespace SWIFTTAP.API.Areas.Cms;

public class CardController : CmsController
{
    private readonly ISender _sender;

    public CardController(ISender sender) =>
        _sender = sender;

    [HttpPut]
    [SwaggerOperation(OperationId = "PutCard")]
    public async Task<ActionResult<long>> PutCard(UpdateCardCommand command) =>
        Ok(await _sender.Send(command));

    [AllowAnonymous]
    [HttpGet("{guid:guid}")]
    [SwaggerOperation(OperationId = "GetCard")]
    public async Task<ActionResult<string>> GetCard([FromRoute] Guid guid) =>
        Redirect(await _sender.Send(new GetCardQuery(guid)));

    [AllowAnonymous]
    [HttpGet("UniqueName")]
    [SwaggerOperation(OperationId = "GetCardByUniqueName")]
    public async Task<ActionResult<CardDetailsDTO>> GetCardByUniqueName([FromQuery] GetCardByUniqueNameQuery query) =>
        Ok(await _sender.Send(query));

    [HttpGet("NfcLink")]
    [SwaggerOperation(OperationId = "GetNfcLink")]
    public async Task<ActionResult<string>> GetNfcLink([FromQuery] GetNfcLinkQuery query) =>
        Ok(await _sender.Send(query));

    [AuthorizeRole(Application.Authorization.Roles.Admin)]
    [HttpGet("NfcLink/AsAdmin")]
    [SwaggerOperation(OperationId = "GetNfcLinkAsAdmin")]
    public async Task<ActionResult<string>> GetNfcLinkAsAdmin([FromQuery] GetNfcLinkAsAdminQuery query) =>
        Ok(await _sender.Send(query));
}


