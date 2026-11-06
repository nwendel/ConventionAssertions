# HasNoAssemblyReferenceWithNameSegment

Checks that an assembly, and every assembly it references directly or indirectly, references no assembly with a given segment in its name, for example that a module never ends up depending on another product's assemblies.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** assemblies
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Segment` | yes | The assembly name segment that must not be referenced, such as `Blaappen`. A segment is one dot-separated part of the assembly name. |
| `SegmentPosition` | no | Where the segment must be for a reference to count: `Anywhere` (the default), `First` or `Last`. |

## Example

The Amni.Appointments assemblies, leaving out the test projects, must not reference any Blaappen assembly, not even through another assembly:

```csharp
Convention.For(
    ConventionTargets.FromAssemblies(x => x
        .FromDefaultDependencyContext(a => a.HasNamePrefix("Amni.Appointments") && !a.HasNameSegment("Tests"))),
    a => a.Assert<HasNoAssemblyReferenceWithNameSegment>(c => c.Segment = "Blaappen"));
```

## Failure message

One line for each offending assembly, with the chain of references that leads to it:

```text
Assembly Amni.Appointments must not reference Blaappen.Core (Amni.Appointments -> Care.Data -> Blaappen.Core).
```

A reference that can't be loaded fails too, so the check can't pass without having looked:

```text
Assembly Amni.Appointments references Care.Data which could not be loaded (Amni.Appointments -> Care.Data).
```

## Notes

- References are followed transitively, breadth first, using the references compiled into each assembly. Each offending assembly is reported once, with the shortest chain found.
- The search doesn't continue past an offending assembly, so its own references aren't reported as well.
- Framework assemblies (`System`, `System.*`, `Microsoft.*`, `netstandard`, `mscorlib` and `WindowsBase`) are matched by name but not searched further, so a `Microsoft.*` package's own references aren't checked.
- Segments are matched whole and case-sensitively, the same way as [HasNamespaceSegment](HasNamespaceSegment.md), so `Blaappen` doesn't match `BlaappenCore`.
- Leaving `Segment` unset throws `ConventionNotConfiguredException`.
