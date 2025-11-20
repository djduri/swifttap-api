using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using SWIFTTAP.API.Areas.Abstractions;
using SWIFTTAP.Application.Features.ContactForms.ContactForms.Commands.SendContactForm;

namespace SWIFTTAP.API.Areas.Cms; 

public class ContactFormController : CmsController
{
    private readonly ISender _sender;

    public ContactFormController(ISender sender) =>
        _sender = sender;

    [EnableRateLimiting("ContactFormLimiter")]
    [AllowAnonymous]
    [HttpPost()]
    [SwaggerOperation(OperationId = "SendContactForm")]
    public async Task<ActionResult<bool>> SendContactForm(SendContactFormCommand command) =>
        Ok(await _sender.Send(command));

}
