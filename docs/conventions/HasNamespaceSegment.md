# HasNamespaceSegment

Checks that a type's namespace contains a given segment, optionally at the start or end.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Segment` | yes | The namespace segment to look for, such as `Domain`. A segment is one dot-separated part of the namespace. |
| `SegmentPosition` | no | Where the segment must be: `Anywhere` (the default), `First` or `Last`. |

## Example

```csharp
Convention.For(
    _valueObjects,
    a => a.Assert<HasNamespaceSegment>(c => c.Segment = "Domain"));

Convention.For(
    _valueObjects,
    a => a.Assert<HasNamespaceSegment>(c =>
    {
        c.Segment = "ValueObjects";
        c.SegmentPosition = SegmentPosition.Last;
    }));
```

## Failure message

The wording follows `SegmentPosition`:

```text
Type MyApp.Orders.OrderId must belong to a namespace which contains a segment Domain.
Type MyApp.Orders.OrderId must belong to a namespace which starts with the segment MyApp.
Type MyApp.Orders.OrderId must belong to a namespace which ends with the segment ValueObjects.
```

## Notes

- Segments are matched whole and case-sensitively, so `Domain` doesn't match `MyApp.DomainModel`.
- A type in the global namespace always fails.
- Leaving `Segment` unset throws `ConventionNotConfiguredException`.
