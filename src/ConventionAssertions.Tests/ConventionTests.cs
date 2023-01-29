namespace ConventionAssertions.Tests;

public class ConventionTests
{
    [Fact]
    public void Can_assert_type_source_single_type()
    {
        var asserted = new List<Type>();

        Convention.ForTypes(
            new DummyTypeSource(),
            x => x.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(DummyTypeSource), type);
    }

    private class DummyTypeSource : ConventionTypeSource
    {
        public DummyTypeSource()
        {
            Types = new[] { typeof(DummyTypeSource) };
        }
    }
}
