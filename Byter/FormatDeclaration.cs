using System.Collections.Immutable;

namespace Byter;

public sealed record FormatDeclaration(
    string Name,
    ImmutableArray<string> GenericParameters,
    ImmutableArray<string> RegularParameters
);