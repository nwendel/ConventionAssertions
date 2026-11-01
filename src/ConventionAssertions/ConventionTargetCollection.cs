namespace ConventionAssertions;

internal sealed class ConventionTargetCollection<T> : IConventionTargets<T>
    where T : MemberInfo
{
    private readonly List<T> _items;

    public ConventionTargetCollection(IEnumerable<T> items)
    {
        GuardAgainst.Null(items);

        _items = items.ToList();
    }

    IEnumerable<T> IConventionTargets<T>.Targets => _items;
}
