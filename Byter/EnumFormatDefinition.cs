namespace Byter;

public sealed class EnumFormatDefinition : FormatDefinition
{
    private readonly Alias tagFormat;
    private readonly IReadOnlyCollection<EnumVariant> variants;

    public EnumFormatDefinition(Alias tagFormat, IReadOnlyCollection<EnumVariant> variants)
    {
        this.tagFormat = tagFormat;
        this.variants = variants;
    }

    public override string ToString() => $"enum<{tagFormat}> {{ {string.Join(", ", variants)} }}";
    public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
}