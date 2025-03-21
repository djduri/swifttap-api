using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.System.Translations.Commands.CreateTranslation;
using SWIFTTAP.Application.Features.System.Translations.Commands.DeleteTranslation;
using SWIFTTAP.Application.Features.System.Translations.Commands.UpdateTranslation;
using SWIFTTAP.Application.Features.System.Translations.DTOs;
using SWIFTTAP.Application.Features.System.Translations.Queries.GetAllTranslations;
using SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslation;
using SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslationByName;

namespace SWIFTTAP.API.Areas.Cms;

public class TranslationController : CmsController
{
    private readonly ISender _sender;

    public TranslationController(ISender sender) =>
        _sender = sender;

    [HttpGet("Id")]
    [SwaggerOperation(OperationId = "GetTranslationById")]
    public async Task<ActionResult<TranslationDetailsDTO>> GetTranslationById([FromQuery] GetTranslationQuery query) =>
        Ok(await _sender.Send(query));

    [AllowAnonymous]
    [HttpGet("Name")]
    [SwaggerOperation(OperationId = "GetTranslationByName")]
    public async Task<ActionResult<TranslationDetailsDTO>> GetTranslationByName([FromQuery] GetTranslationByNameQuery query) =>
        Ok(await _sender.Send(query));

    [HttpGet("List")]
    [SwaggerOperation(OperationId = "ListTranslations")]
    public async Task<ActionResult<RangedDTO<TranslationSimpleDTO>>> GetAllTranslations([FromQuery] GetAllTranslationsQuery query) =>
        Ok(await _sender.Send(query));

    [HttpPost]
    [SwaggerOperation(OperationId = "PostTranslation")]
    public async Task<ActionResult<long>> PostTranslation(CreateTranslationCommand command) =>
        Ok(await _sender.Send(command));

    [HttpPut]
    [SwaggerOperation(OperationId = "PutTranslation")]
    public async Task<ActionResult<long>> PutTranslation(UpdateTranslationCommand command) =>
        Ok(await _sender.Send(command)); 

    [HttpDelete]
    [SwaggerOperation(OperationId = "DeleteTranslation")]
    public async Task<ActionResult<long>> DeleteTranslation(DeleteTranslationCommand command) =>
        Ok(await _sender.Send(command));
}
