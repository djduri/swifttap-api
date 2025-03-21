using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.Cards.Themes.Commands.UpdateTheme;

namespace SWIFTTAP.API.Areas.Cms;

public class ThemeController: CmsController
{
    private readonly ISender _sender;

    public ThemeController(ISender sender) =>
        _sender = sender;       

    [HttpPut]
    [SwaggerOperation(OperationId = "PutTheme")]
    public async Task<ActionResult<long>> PutTheme(UpdateThemeCommand command) =>
       Ok(await _sender.Send(command));
}
