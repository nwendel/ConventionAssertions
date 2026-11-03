# IsNotAsync

Checks that a method is not `async`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods

## Example

Methods must not be `async void`, because exceptions thrown from an `async void` method can't be caught by the caller and usually crash the process:

```csharp
Convention.For(
    ConventionTargets.FromMethods(
        s => s.FromAssemblyContaining<OrderService>(),
        m => m.Where(x => x.IsVoid)),
    a => a.Assert<IsNotAsync>());
```

## Failure message

```text
Method MyApp.Orders.OrderService.Notify() must not be async.
```

## Notes

- The check uses the `AsyncStateMachineAttribute` that the C# compiler adds to `async` methods. A method that returns a `Task` without the `async` keyword passes.
- Event handlers are a legitimate use of `async void`. Suppress them with `[SuppressConvention]` or filter them out with a method `Where(...)`.
- Method scans only return public methods, so `private async void` methods aren't checked.
