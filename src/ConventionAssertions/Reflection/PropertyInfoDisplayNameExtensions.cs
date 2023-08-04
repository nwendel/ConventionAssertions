using System.Reflection;
using System.Text;

namespace ConventionAssertions.Reflection;

public static class PropertyInfoDisplayNameExtensions
{
    public static string DisplayName(this PropertyInfo self)
    {
        GuardAgainst.Null(self);

        var builder = new StringBuilder();

        if (self.DeclaringType != null)
        {
            builder.Append(self.DeclaringType.DisplayName());
            builder.Append('.');
        }

        // TODO: This doesn't handle indexers correctly yet
        builder.Append(self.Name);

        return builder.ToString();
    }
}
