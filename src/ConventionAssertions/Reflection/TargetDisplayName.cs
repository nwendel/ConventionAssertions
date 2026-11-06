namespace ConventionAssertions.Reflection;

internal static class TargetDisplayName
{
    public static string DisplayName(object target)
    {
        GuardAgainst.Null(target);

        return target switch
        {
            MemberInfo memberInfo => memberInfo.DisplayName(),
            Assembly assembly => assembly.DisplayName(),
            _ => throw new NotSupportedException($"DisplayName for {target.GetType().FullName} is not supported"),
        };
    }

    public static string KindName(object target)
    {
        GuardAgainst.Null(target);

        return target switch
        {
            MemberInfo memberInfo => memberInfo.KindName(),
            Assembly => "Assembly",
            _ => throw new NotSupportedException($"KindName for {target.GetType().FullName} is not supported"),
        };
    }
}
