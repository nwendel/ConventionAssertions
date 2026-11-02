namespace ConventionAssertions.Conventions;

[Obsolete("Use ConventionAssertions.AspNetCore.Conventions.ControllerActionHasAuthorization from the ConventionAssertions.AspNetCore package instead.")]
public class ControllerActionHasAuthorization : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        context.Fail("uses obsolete ControllerActionHasAuthorization, use ConventionAssertions.AspNetCore.Conventions.ControllerActionHasAuthorization from the ConventionAssertions.AspNetCore package instead");
    }
}
