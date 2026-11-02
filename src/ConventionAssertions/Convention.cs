using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void For<TTarget>(
        IConventionTargets<TTarget> targets,
        Action<IConventionAssert<TTarget>> assertAction)
        where TTarget : MemberInfo
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        var assert = new ConventionAssert<TTarget>(targets);
        assertAction(assert);
    }
}
