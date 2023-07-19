using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConventionAssertions.Rules;

// TODO: This adds a AspNetCore reference, perhaps move to a different assembly?
// TODO: Name of this rule? HasControllerAuthorization/HasAspNetCoreAuthorization?
public class HasAuthorization : IMethodConvention
{
    public void Assert(MethodInfo method, ConventionContext context)
    {
        GuardAgainst.Null(method);
        GuardAgainst.Null(context);

        if (method.DeclaringType != null &&
            HasAnyAttribute(method.DeclaringType, typeof(AuthorizeAttribute), typeof(AllowAnonymousAttribute)))
        {
            return;
        }

        if (HasAnyAttribute(method, typeof(NonActionAttribute)))
        {
            return;
        }

        if (!HasAnyAttribute(method, typeof(AuthorizeAttribute), typeof(AllowAnonymousAttribute)))
        {
            context.Fail(method, "No auth for controller method");
        }
    }

    private static bool HasAnyAttribute(MemberInfo memberInfo, params Type[] attributeTypes)
    {
        var attributes = memberInfo.GetCustomAttributes(true)
            .Select(x => x.GetType())
            .ToList();
        foreach (var attributeType in attributeTypes)
        {
            if (attributes.Any(x => x.IsAssignableTo(attributeType)))
            {
                return true;
            }
        }

        return false;
    }
}
