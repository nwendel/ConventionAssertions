# ControllerActionHasAuthorization

Checks that every ASP.NET Core controller action makes an explicit authorization decision with `[Authorize]` or `[AllowAnonymous]`, on the action or on its controller.

- **Package:** ConventionAssertions.AspNetCore
- **Namespace:** `ConventionAssertions.AspNetCore.Conventions`
- **Targets:** methods

## Example

```csharp
using ConventionAssertions.AspNetCore.Conventions;
using Microsoft.AspNetCore.Mvc;

[Fact]
public void Controller_actions_declare_authorization()
{
    Convention.For(
        s => s
            .FromAssemblyContaining<Program>()
            .Where(t => t.IsConcrete && t.IsAssignableTo<ControllerBase>())
            .Methods(m => m.Method.DeclaringType != typeof(ControllerBase)
                && m.Method.DeclaringType != typeof(Controller)),
        a => a.Assert<ControllerActionHasAuthorization>());
}
```

The `Methods(...)` predicate leaves out the public methods that controllers inherit from `ControllerBase` and `Controller`.

## Failure messages

```text
Method MyApp.Api.OrdersController.Get(int) no authorization specified for method.
Method MyApp.Api.PublicController.Admin() method has authorization but controller allows anonymous access.
```

## Notes

- An action passes when it, or the controller that declares it, has `[Authorize]` or `[AllowAnonymous]`. Attributes derived from those count too.
- Methods marked `[NonAction]` are skipped.
- An action with `[Authorize]` inside an `[AllowAnonymous]` controller fails, because `[AllowAnonymous]` on the controller overrides it.
- The convention used to be in the core package as `ConventionAssertions.Conventions.ControllerActionHasAuthorization`. That class is obsolete and now always fails, telling you to switch to this one.
