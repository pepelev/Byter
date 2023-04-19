namespace Byter;

public sealed class Alias : FormatDefinition
{
    private readonly FormatDeclaration declaration;

    public Alias(FormatDeclaration declaration)
    {
        this.declaration = declaration;
    }

    public override string ToString() => declaration.ToString();
}