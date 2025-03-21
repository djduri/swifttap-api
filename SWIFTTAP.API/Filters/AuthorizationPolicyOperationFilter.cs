using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SWIFTTAP.API.Filters;

internal sealed class AuthorizationPolicyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            return;

        var controllerPolicies = ExtractPolicies(actionDescriptor.ControllerTypeInfo);
        var methodPolicies = ExtractPolicies(actionDescriptor.MethodInfo);

        var policies = controllerPolicies.Union(methodPolicies).Distinct();

        if (policies.Any())
        {
            operation.Description += $"<b>Required permissions:</b> {string.Join(", ", policies)}<br>";
        }
    }

    private static IEnumerable<string> ExtractPolicies(MemberInfo member)
    {
        return member
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Where(attr => !string.IsNullOrWhiteSpace(attr.Policy))
            .Select(attr => attr.Policy);
    }
}
