using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SWIFTTAP.Application.Authorization;

namespace SWIFTTAP.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    public AuthorizeRoleAttribute(params Roles[] roles)
    {
        Roles = string.Join(",", roles.Select(role => role.ToString()));
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
