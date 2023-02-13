using System.Text;

namespace ConventionAssertions.Reflection;

public static class TypeExtensions
{
    // TODO: More?
    private static Dictionary<Type, string> _simpleNames = new()
    {
        [typeof(int)] = "int",
        [typeof(string)] = "string",
        [typeof(bool)] = "bool",
        [typeof(long)] = "long",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(object)] = "object",
    };

    // TODO: Will this handle nested types correctly?
    public static string DisplayName(this Type self)
    {
        GuardAgainst.Null(self);

        if (_simpleNames.TryGetValue(self, out var simpleName))
        {
            return simpleName;
        }

        if (self.IsArray)
        {
            return $"{self.GetElementType()!.DisplayName()}[{new string(',', self.GetArrayRank() - 1)}]";
        }

        var nullableType = Nullable.GetUnderlyingType(self);
        if (nullableType != null)
        {
            return $"{nullableType.DisplayName()}?";
        }

        // TODO: Is this correct?
        var typeName = self.FullName ?? self.Name;

        if (self.IsGenericType)
        {
            var builder = new StringBuilder();

            builder.Append(typeName.TakeUntil('`'));
            builder.Append('<');
            var genericArguments = self.GetGenericArguments()
                .Select(x => x.DisplayName())
                .ToList();
            builder.Append(string.Join(',', genericArguments));
            builder.Append('>');

            return builder.ToString();
        }

        return typeName;
    }
}
