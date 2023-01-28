namespace ConventionAsserts.Internal;

public interface ITypeScanner
{
    ITypeScannerFilter FromAssemblyContaining<T>();
}
