namespace Byter;

public sealed class RecordFormatDefinition : FormatDefinition
{
    public FormatDeclaration Declaration { get; }
    public NamedRecord Record { get; }
}