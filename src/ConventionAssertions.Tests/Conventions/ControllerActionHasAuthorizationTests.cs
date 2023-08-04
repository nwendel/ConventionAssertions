using ConventionAssertions.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConventionAssertions.Tests.Conventions;

public class ControllerActionHasAuthorizationTests
{
    private readonly ControllerActionHasAuthorization _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert_class_authorize()
    {
        var method =
            typeof(AuthorizeController)
            .GetMethod(nameof(AuthorizeController.Method))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_class_anonymous()
    {
        var method =
            typeof(AnonymousController)
            .GetMethod(nameof(AnonymousController.Method))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_method_authorize()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.AuthorizeMethod))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_method_anonymous()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.AnonymousMethod))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_method_non_action()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.NonActionMethod))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_method_unspecified()
    {
        var method =
            typeof(Controller)
            .GetMethod(nameof(Controller.Method))!;

        Assert.Throws<ConventionFailedException>(() => _tested.Assert(method, _context));
        Assert.Single(_context.Messages);
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
