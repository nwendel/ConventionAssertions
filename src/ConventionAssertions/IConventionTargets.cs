﻿namespace ConventionAssertions;

public interface IConventionTargets<T> : IEnumerable<T>
    where T : MemberInfo
{
}
