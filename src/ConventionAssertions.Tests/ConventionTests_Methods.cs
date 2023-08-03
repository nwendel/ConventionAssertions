using ConventionAssertions.Internal;
using ConventionAssertions.Tests.TestHelpers;

namespace ConventionAssertions.Tests;

public class ConventionTests_Methods
{
    [Fact]
    public void Throws_on_null_types_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForMethods(
            (Action<ITypeScanner>)null!,
            m => { },
            x => x.Assert<NothingMethodConvention>()));
        Assert.Equal("scannerAction", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_method_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForMethods(
            t => { },
            null!,
            x => x.Assert<NothingMethodConvention>()));
        Assert.Equal("scanner", ex.ParamName);
    }
}
