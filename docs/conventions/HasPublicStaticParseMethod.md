# HasPublicStaticParseMethod

Checks that a type has a method with the signature `public static T Parse(string)`, where `T` is the type itself.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.ForTypes(
    s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass),
    a => a.Assert<HasPublicStaticParseMethod>());
```

## Failure message

```text
Type MyApp.Domain.OrderId must have a public static method with signature: MyApp.Domain.OrderId Parse(string).
```

## Notes

- The method must take exactly one `string` parameter and return the type itself. Overloads with other parameters, such as an `IFormatProvider`, don't count.
- The companion check is [HasPublicStaticTryParseMethod](HasPublicStaticTryParseMethod.md).
