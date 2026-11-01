using ConventionAssertions.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConventionAssertions.Tests.Conventions;

public class ControllerActionHasAuthorizationTests
{
    private readonly ControllerActionHasAuthorization _tested = new();

    [Fact]
    public void Can_assert_class_authorize()
    {
        var method =
            typeof(AuthorizeController)
            .GetMethod(nameof(AuthorizeController.Method))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_class_anonymous()
    {
        var method =
            typeof(AnonymousController)
            .GetMethod(nameof(AnonymousController.Method))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_method_authorize()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.AuthorizeMethod))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_method_anonymous()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.AnonymousMethod))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_method_non_action()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.NonActionMethod))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_method_unspecified()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.Method))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);
        Assert.Single(context.Messages);
    }

    [Authorize]
    private class AuthorizeController
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used for testing")]
        public ActionResult Method()
        {
            return null!;
        }
    }

    [AllowAnonymous]
    private class AnonymousController
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used for testing")]
        public ActionResult Method()
        {
            return null!;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used for testing")]
    private class Controller
    {
        [Authorize]
        public ActionResult AuthorizeMethod()
        {
            return null!;
        }

        [AllowAnonymous]
        public ActionResult AnonymousMethod()
        {
            return null!;
        }

        [NonAction]
        public ActionResult NonActionMethod()
        {
            return null!;
        }

        public ActionResult Method()
        {
            return null!;
        }
    }
}
