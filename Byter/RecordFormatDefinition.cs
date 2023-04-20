namespace Byter;

public sealed class RecordFormatDefinition : FormatDefinition
{
    public RecordFormatDefinition(IReadOnlyCollection<RecordField> fields)
    {
        Fields = fields;
    }

    public IReadOnlyCollection<RecordField> Fields { get; }
    public override string ToString() => $"record {{ {string.Join(", ", Fields)} }}";
}