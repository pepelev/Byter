using System.Collections.Immutable;

namespace Byter;

public sealed class Utf8Number : FormatDefinition
{
    public override FormatDefinition Construct(ImmutableArray<string> genericParameters, ImmutableArray<long> regularParameters)
    {
        throw new NotImplementedException();
    }
}