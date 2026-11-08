// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Unit test naming standard")]
[assembly: SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Required by some unit tests")]
[assembly: SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Required by some unit tests")]
[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Required by some unit tests")]
[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Required by some unit tests")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Required by some unit tests")]
