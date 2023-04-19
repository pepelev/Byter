using System.Collections.Immutable;

namespace Byter;

public sealed class Alias : FormatDefinition
{
    private readonly FormatDeclaration declaration;

    public Alias(FormatDeclaration declaration)
    {
        this.declaration = declaration;
    }


    public override FormatDefinition Construct(ImmutableArray<string> genericParameters, ImmutableArray<long> regularParameters)
    {
        throw new NotImplementedException();
    }
}