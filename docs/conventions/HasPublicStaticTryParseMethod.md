# HasPublicStaticTryParseMethod

Checks that a type has a method with the signature `public static bool TryParse(string, out T)`, where `T` is the type itself.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.For(
    ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass)),
    a => a.Assert<HasPublicStaticTryParseMethod>());
```

## Failure message

```text
Type MyApp.Domain.OrderId must have a public static method with signature: bool TryParse(string, out MyApp.Domain.OrderId).
```

## Notes

- The method must take exactly a `string` and an `out` parameter of the type itself, and return `bool`. Overloads with other parameters, such as an `IFormatProvider`, don't count.
- The companion check is [HasPublicStaticParseMethod](HasPublicStaticParseMethod.md).
