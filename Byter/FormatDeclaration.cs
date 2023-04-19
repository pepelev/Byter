using System.Collections.Immutable;

namespace Byter;

public sealed record FormatDeclaration(
    string Name,
    ImmutableArray<string> GenericParameters,
    ImmutableArray<string> RegularParameters
)
{
    public override string ToString()
    {
        var generics = GenericParameters.Length == 0
            ? ""
            : $"<{string.Join(", ", GenericParameters)}>";
        var regular = RegularParameters.Length == 0
            ? ""
            : $"({string.Join(", ", RegularParameters)})";

        return $"{Name}{generics}{regular}";
    }
}