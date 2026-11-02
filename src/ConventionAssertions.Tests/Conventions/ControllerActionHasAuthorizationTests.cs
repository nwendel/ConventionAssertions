using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

[Obsolete("Tests the obsolete ControllerActionHasAuthorization placeholder")]
public class ControllerActionHasAuthorizationTests
{
    private readonly ControllerActionHasAuthorization _tested = new();

    [Fact]
    public void Fails_on_assert()
    {
        var method =
            typeof(object)
            .GetMethod(nameof(object.ToString))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);
        Assert.Single(context.Messages);
    }
}
