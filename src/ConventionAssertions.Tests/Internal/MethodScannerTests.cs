using System.Reflection;

namespace ConventionAssertions.Tests.Internal;

public class MethodScannerTests
{
    private readonly MethodScanner _tested = new(new[] { typeof(MethodScannerTests) });

    private readonly MethodInfo _publicMethod;
    private readonly MethodInfo _anotherPublicMethod;

    public MethodScannerTests()
    {
        _publicMethod = typeof(MethodScannerTests).GetMethod(nameof(PublicMethod))!;
        _anotherPublicMethod = typeof(MethodScannerTests).GetMethod(nameof(AnotherPublicMethod))!;
    }

    [Fact]
    public void Can_GetMethods()
    {
        Assert.Contains(_publicMethod, _tested);
        Assert.Contains(_anotherPublicMethod, _tested);
    }

    [Fact]
    public void Can_Where()
    {
        _tested
            .Where(m => m.Method.Name == nameof(PublicMethod));

        Assert.Contains(_publicMethod, _tested);
        Assert.DoesNotContain(_anotherPublicMethod, _tested);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1013:Public method should be marked as test", Justification = "Used in testing")]
    public void PublicMethod()
    {
        throw new NotImplementedException();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1013:Public method should be marked as test", Justification = "Used in testing")]
    public void AnotherPublicMethod()
    {
        throw new NotImplementedException();
    }
}
