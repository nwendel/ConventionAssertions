﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConventionAssertions.Conventions;

// TODO: This adds a AspNetCore reference, perhaps move to a different assembly?
public class ControllerActionHasAuthorization : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (target.DeclaringType != null &&
            HasAnyAttribute(target.DeclaringType, typeof(AuthorizeAttribute), typeof(AllowAnonymousAttribute)))
        {
            return;
        }

        if (HasAnyAttribute(target, typeof(NonActionAttribute)))
        {
            return;
        }

        if (!HasAnyAttribute(target, typeof(AuthorizeAttribute), typeof(AllowAnonymousAttribute)))
        {
            context.Fail(target, "no authorization specified for method");
        }

        if (target.DeclaringType != null &&
            HasAnyAttribute(target.DeclaringType, typeof(AllowAnonymousAttribute)) &&
            HasAnyAttribute(target, typeof(AuthorizeAttribute)))
        {
            context.Fail(target, "method has authorization but controller allows anonymous access");
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
