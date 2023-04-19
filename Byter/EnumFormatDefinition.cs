namespace Byter;

public sealed class EnumFormatDefinition : FormatDefinition
{
    private readonly Alias tagFormat;
    private readonly IEnumerable<EnumVariant> variants;

    public EnumFormatDefinition(Alias tagFormat, IEnumerable<EnumVariant> variants)
    {
        this.tagFormat = tagFormat;
        this.variants = variants;
    }

    public override string ToString() => $"enum<{tagFormat}> {{ {string.Join(", ", variants)} }}";
}