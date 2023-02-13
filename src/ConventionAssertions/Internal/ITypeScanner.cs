namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();

    ITypeScannerFilter FromTypeSource(ConventionTypeSource typeSource);
}
