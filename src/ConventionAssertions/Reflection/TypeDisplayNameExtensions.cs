using System.Text;

namespace ConventionAssertions.Reflection;

public static class TypeDisplayNameExtensions
{
    // TODO: More?
    private static Dictionary<Type, string> _simpleNames = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(decimal)] = "decimal",
        [typeof(double)] = "double",
        [typeof(float)] = "float",
        [typeof(int)] = "int",
        [typeof(long)] = "long",
        [typeof(object)] = "object",
        [typeof(short)] = "short",
        [typeof(string)] = "string",
    };

    // TODO: Will this handle nested types correctly?
    // TODO: Will this handle open generic types correctly?
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

        // TODO: Also check nullable reference types here?
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
