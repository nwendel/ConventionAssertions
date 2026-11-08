namespace ConventionAssertions.Tests.TestHelpers;

public class PublicDeclaringClass
{
    public static Type NestedPublicType => typeof(NestedPublicClass);

    public static Type NestedInternalType => typeof(NestedInternalClass);

    public static Type NestedProtectedInternalType => typeof(NestedProtectedInternalClass);

    public static Type NestedProtectedType => typeof(NestedProtectedClass);

    public static Type NestedPrivateProtectedType => typeof(NestedPrivateProtectedClass);

    public static Type NestedPrivateType => typeof(NestedPrivateClass);

    public class NestedPublicClass
    {
    }

    internal class NestedInternalClass
    {
    }

    protected internal class NestedProtectedInternalClass
    {
    }

    protected class NestedProtectedClass
    {
    }

    private protected class NestedPrivateProtectedClass
    {
    }

    private class NestedPrivateClass
    {
    }
}
