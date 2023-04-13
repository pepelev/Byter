namespace Byter;

public sealed record NamedRecord(
    string Name,
    (string Format, string Name)[] Record
);