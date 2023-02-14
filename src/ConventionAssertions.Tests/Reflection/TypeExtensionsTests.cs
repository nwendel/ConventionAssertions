using ConventionAssertions.Reflection;

namespace ConventionAssertions.Tests.Reflection;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(bool), "bool")]
    [InlineData(typeof(byte), "byte")]
    [InlineData(typeof(decimal), "decimal")]
    [InlineData(typeof(double), "double")]
    [InlineData(typeof(float), "float")]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(long), "long")]
    [InlineData(typeof(object), "object")]
    [InlineData(typeof(short), "short")]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(TypeExtensionsTests), "ConventionAssertions.Tests.Reflection.TypeExtensionsTests")]
    [InlineData(typeof(int?), "int?")]
    [InlineData(typeof(List<int?>), "System.Collections.Generic.List<int?>")]
    [InlineData(typeof(int[]), "int[]")]
    [InlineData(typeof(int[,]), "int[,]")]
    [InlineData(typeof(Dictionary<int, int>), "System.Collections.Generic.Dictionary<int,int>")]
    [InlineData(typeof(List<>), "System.Collections.Generic.List<T>")]
    public void Can_get_display_name(Type type, string displayName)
    {
        Assert.Equal(displayName, type.DisplayName());
    }
}
