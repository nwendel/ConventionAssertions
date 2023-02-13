using ConventionAssertions.Reflection;

namespace ConventionAssertions.Tests.Reflection;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(int?), "int?")]
    [InlineData(typeof(List<int?>), "System.Collections.Generic.List<int?>")]
    [InlineData(typeof(object[]), "object[]")]
    [InlineData(typeof(TypeExtensionsTests[,]), "ConventionAssertions.Tests.Reflection.TypeExtensionsTests[,]")]
    [InlineData(typeof(Dictionary<string, double>), "System.Collections.Generic.Dictionary<string,double>")]
    public void Can_get_display_name(Type type, string displayName)
    {
        Assert.Equal(displayName, type.DisplayName());
    }
}
