namespace Byter;

public sealed record NamedRecord(
    FormatDeclaration Declaration,
    (string Format, string Name)[] Record
);