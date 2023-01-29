namespace ConventionAssertions.Internal;

public interface ITypeScanner
{
    ITypeScannerFilter FromAssemblyContaining<T>();
}
