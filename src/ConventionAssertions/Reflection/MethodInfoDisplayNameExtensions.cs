using System.Reflection;
using System.Text;

namespace ConventionAssertions.Reflection;

public static class MethodInfoDisplayNameExtensions
{
    public static string DisplayName(this MethodInfo self)
    {
        GuardAgainst.Null(self);

        var builder = new StringBuilder();

        if (self.DeclaringType != null)
        {
            builder.Append(self.DeclaringType.DisplayName());
            builder.Append('.');
        }

        // TODO: This doesn't handle generics correctly yet
        builder.Append(self.Name);
        builder.Append('(');
        builder.Append(string.Join(',', self.GetParameters().Select(x => x.ParameterType.DisplayName())));
        builder.Append(')');

        return builder.ToString();
    }
}
