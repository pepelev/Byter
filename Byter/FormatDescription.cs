namespace Byter;

public sealed class FormatDescription
{
    public FormatDescription(FormatDeclaration declaration, FormatDefinition definition)
    {
        Declaration = declaration;
        Definition = definition;
    }

    public FormatDeclaration Declaration { get; }
    public FormatDefinition Definition { get; }
}