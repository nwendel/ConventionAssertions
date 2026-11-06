# NameIsSnakeCase

Checks that the name of a type or method is written in `lower_snake_case`. A common use is test method names.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types, methods

## Example

```csharp
var testMethods = ConventionTargets.FromMethods(
    a => a.FromAssemblyContaining<OrderTests>(),
    t => t.Where(x => x.Type.Name.EndsWith("Tests", StringComparison.Ordinal)),
    m => m.Where(x => x.HasCustomAttribute<FactAttribute>()));

Convention.For(
    testMethods,
    a => a.Assert<NameIsSnakeCase>());
```

## Failure message

```text
Method MyApp.Tests.OrderTests.CanPlaceOrder() is not snake case.
```

## Notes

A name passes when:

- it contains only lowercase ASCII letters, digits and underscores
- it doesn't start or end with an underscore
- it doesn't contain two underscores in a row

Generic type names include an arity marker such as `` `1 ``, so generic types always fail.
