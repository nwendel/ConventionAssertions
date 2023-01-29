namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();
}
