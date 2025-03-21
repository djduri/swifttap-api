using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SWIFTTAP.API.Filters;

internal sealed class RequiredRolesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            return;

        var controllerRoles = ExtractRoles(actionDescriptor.ControllerTypeInfo);
        var methodRoles = ExtractRoles(actionDescriptor.MethodInfo);

        var roles = controllerRoles.Union(methodRoles).Distinct();

        if (roles.Any())
        {
            operation.Description += $"<b>Required roles:</b> {string.Join(", ", roles)}<br>";
        }
    }

    private static IEnumerable<string> ExtractRoles(MemberInfo member)
    {
        return member
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Where(attr => !string.IsNullOrWhiteSpace(attr.Roles))
            .Select(attr => attr.Roles);
    }
}
