# Writing your own conventions

There are two ways to write a convention: as a class you can reuse across tests and projects, or inline as a lambda for a rule only one test needs.

## Convention classes

A convention is a class with a parameterless constructor that implements `IConvention<TTarget>`, where `TTarget` is `Type`, `MethodInfo` or `PropertyInfo`. Call `context.Fail` once for each problem you find:

```csharp
using System.Reflection;
using ConventionAssertions;

public class ReturnsTask : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        if (!typeof(Task).IsAssignableFrom(target.ReturnType))
        {
            context.Fail("must return a Task");
        }
    }
}
```

```csharp
Convention.ForMethods(
    s => s
        .FromAssemblyContaining<OrderService>()
        .Where(t => t.IsAssignableTo<ICommandHandler>()),
    m => m.Where(x => x.Method.Name == "Handle"),
    a => a.Assert<ReturnsTask>());
```

The failure message is built from the kind of target, its display name and your text, so `context.Fail("must return a Task")` reads as:

```text
Method MyApp.Orders.PlaceOrderHandler.Handle(MyApp.Orders.PlaceOrder) must return a Task.
```

Write the message as the end of that sentence, without a trailing period.

One class can implement the interface for several target kinds, like `NameIsSnakeCase` does for both `Type` and `MethodInfo`.

### Configurable conventions

A convention with settings implements `IConfigurableConvention<TTarget>` instead and exposes the settings as properties. `Assert<T>()` without a configure action only accepts `IConvention<TTarget>`, so a configurable convention can't be used by mistake without its settings. Throw `ConventionNotConfiguredException` if a required setting is missing anyway:

```csharp
public class HasMaxParameterCount : IConfigurableConvention<MethodInfo>
{
    public int? Max { get; set; }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        if (Max == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Max)} not specified");
        }

        if (target.GetParameters().Length > Max)
        {
            context.Fail($"must have at most {Max} parameters");
        }
    }
}
```

```csharp
Convention.ForMethods(
    s => s.FromAssemblyContaining<OrderService>().Where(t => t.IsConcrete),
    a => a.Assert<HasMaxParameterCount>(c => c.Max = 4));
```

## Inline conventions

For a rule that only one test needs, pass a lambda or a method with the same shape as `Assert` instead of writing a class. It reports failures the same way, through `context.Fail`:

```csharp
[Fact]
public void Value_object_properties_are_value_types()
{
    Convention.ForProperties(
        s => s.FromTargets(_valueObjects),
        a => a.Assert((property, context) =>
        {
            if (!property.PropertyType.IsValueType)
            {
                context.Fail("must be a value type");
            }
        }));
}
```

```csharp
[Fact]
public void Value_object_static_methods_return_value()
{
    Convention.ForMethods(
        s => s.FromTargets(_valueObjects),
        m => m.Where(x => x.Method.IsStatic),
        a => a.Assert(ReturnsValue));
}

private static void ReturnsValue(MethodInfo method, ConventionContext context)
{
    if (method.ReturnType == typeof(void))
    {
        context.Fail("must return a value");
    }
}
```
