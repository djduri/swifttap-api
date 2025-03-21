using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SWIFTTAP.API.Areas.Abstractions;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/cms/[controller]")]
[ApiExplorerSettings(GroupName = "Cms")]
public abstract class CmsController : CoreControllerBase
{
}
