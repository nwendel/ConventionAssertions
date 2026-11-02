# VoidMethodIsNotAsync

Checks that a method is not `async void`. Exceptions thrown from an `async void` method can't be caught by the caller and usually crash the process.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods

## Example

```csharp
Convention.ForMethods(
    s => s.FromAssemblyContaining<OrderService>(),
    a => a.Assert<VoidMethodIsNotAsync>());
```

## Failure message

```text
Method MyApp.Orders.OrderService.Notify() must not be async void.
```

## Notes

- A method fails when it returns `void` and was compiled from an `async` method. The check uses the `AsyncStateMachineAttribute` that the C# compiler adds to `async` methods.
- Event handlers are a legitimate use of `async void`. Suppress them with `[SuppressConvention]` or filter them out with a method `Where(...)`.
- Method scans only return public methods, so `private async void` methods aren't checked.
