# DoesNotExposeTypes

Checks that a method or property doesn't expose any of a set of types, for example that a module's public services never hand out its entities.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods, properties
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Types` | yes | The types that must not be exposed, as an `IConventionTargets<Type>`, so they can be picked with a type scan. |

## Example

The public services of the module must not expose its entities:

```csharp
private static readonly IConventionTargets<Type> Entities = ConventionTargets.FromTypes(s => s
    .FromAssemblyContaining<MeetingTemplate>()
    .Where(t => t.IsConcrete && t.IsAssignableTo<IEntity>()));

[Fact]
public void Services_do_not_expose_entities()
{
    Convention.For(
        ConventionTargets.FromMethods(s => s
            .FromAssemblyContaining<IMeetingTemplateService>()
            .Where(t => t.IsInterface && t.IsVisible)),
        a => a.Assert<DoesNotExposeTypes>(c => c.Types = Entities));
}
```

## Failure message

One line for each exposed type, saying where it was found:

```text
Method MyApp.Meetings.IMeetingTemplateService.Get(System.Guid) return type exposes MyApp.Meetings.MeetingTemplate.
Method MyApp.Meetings.IMeetingTemplateService.List() return type exposes MyApp.Meetings.MeetingTemplate (via MyApp.Meetings.MeetingTemplateDto.Template).
Method MyApp.Meetings.IMeetingTemplateService.Save(MyApp.Meetings.MeetingTemplate) parameter template exposes MyApp.Meetings.MeetingTemplate.
Property MyApp.Meetings.MeetingDto.Template exposes MyApp.Meetings.MeetingTemplate.
```

## Notes

- For a method, the return type and every parameter are checked. For a property, the property type is checked.
- A type is exposed when it can be assigned to one of `Types`, so a class derived from an entity is exposed too.
- Arrays, `ref`, `out` and `in` parameters, `Nullable<T>` and generic arguments are looked through, so `Task<MeetingTemplate>`, `IReadOnlyList<MeetingTemplate>` and `Dictionary<Guid, MeetingTemplate>` all expose `MeetingTemplate`.
- The public instance properties of your own types are checked as well, so a DTO with an entity property exposes the entity. Primitives, `string` and types in `System` or `Microsoft` namespaces are only looked through, not into.
- Each type is looked at once for each return type, parameter or property, so cycles between types don't loop.
- Leaving `Types` unset, or a `Types` scan that matches nothing, throws `ConventionNotConfiguredException`, so the check can't pass without anything to look for.
