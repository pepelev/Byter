namespace Byter;

public sealed class FormatDescription
{
    public FormatDescription(Scope scope, FormatDeclaration declaration, FormatDefinition definition)
    {
        Declaration = declaration;
        Definition = definition;
        Scope = scope;
    }

    public Scope Scope { get; }
    public FormatDeclaration Declaration { get; }
    public FormatDefinition Definition { get; }

    public override string ToString() => $"{Declaration} = {Definition}";
}