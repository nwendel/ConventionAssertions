﻿using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static ConventionTypeSource ForTypes(Action<ITypeScanner> scanner)
    {
        GuardAgainst.Null(scanner);

        var typeSource = new TypeScanner();
        scanner(typeSource);
        return typeSource;
    }

    public static void ForTypes(
        Action<ITypeScanner> scanner,
        Action<ITypeAssert> assert)
    {
        var typeSource = ForTypes(scanner);
        ForTypes(typeSource, assert);
    }

    public static void ForTypes(
        ConventionTypeSource typeSource,
        Action<ITypeAssert> assert)
    {
        GuardAgainst.Null(assert);

        var typeAssert = new TypeAssert(typeSource);
        assert(typeAssert);
    }
}