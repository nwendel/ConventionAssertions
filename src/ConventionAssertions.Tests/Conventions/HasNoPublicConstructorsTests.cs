using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoPublicConstructorsTests
{
    private readonly HasNoPublicConstructors _tested = new();

    [Fact]
    public void Can_assert()
    {
        var target = typeof(ProtectedConstructor);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_public_constructor()
    {
        var target = typeof(PublicConstructor);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    private class ProtectedConstructor
    {
        protected ProtectedConstructor()
        {
        }
    }

    private class PublicConstructor
    {
        public PublicConstructor()
        {
        }
    }
}
