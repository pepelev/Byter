using System.Collections.Immutable;

namespace Byter;

public sealed class Const : FormatDefinition
{
    private readonly string hex;

    public Const(string hex)
    {
        this.hex = hex;
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters)
    {
        throw new NotImplementedException();
    }
}