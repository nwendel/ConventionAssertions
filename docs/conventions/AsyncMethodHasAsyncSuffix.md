# AsyncMethodHasAsyncSuffix

Checks that a method which returns an awaitable type has a name ending with `Async`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<OrderService>()
        .Where(t => t.IsClass)
        .Methods(),
    a => a.Assert<AsyncMethodHasAsyncSuffix>());
```

## Failure message

```text
Method MyApp.Orders.OrderService.PlaceOrder(MyApp.Orders.PlaceOrder) returns an awaitable type and must have name ending with Async.
```

## Notes

- Awaitable types are `Task`, `Task<T>` and anything derived from `Task`, plus `ValueTask` and `ValueTask<T>`. Methods that return anything else pass.
- The check looks at the return type, not the `async` keyword, so a non-`async` method that returns a `Task` also needs the suffix.
- Methods that must keep a fixed name, such as ASP.NET Core controller actions or MediatR's `Handle`, can be filtered out with the `Methods(...)` predicate or suppressed with `[SuppressConvention]`.
