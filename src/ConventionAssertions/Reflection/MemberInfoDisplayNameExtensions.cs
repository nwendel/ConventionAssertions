using System.Reflection;

namespace ConventionAssertions.Reflection;

public static class MemberInfoDisplayNameExtensions
{
    public static string DisplayName(this MemberInfo self)
    {
        GuardAgainst.Null(self);

        var displayName = self switch
        {
            Type type => type.DisplayName(),
            MethodInfo methodInfo => methodInfo.DisplayName(),
            PropertyInfo propertyInfo => propertyInfo.Name, // TODO: This is wrong!
            _ => throw new NotSupportedException($"DisplayName for {self.GetType().FullName} is not supported"),
        };
        return displayName;
    }
}
