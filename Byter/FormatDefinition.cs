using System.Collections.Immutable;

namespace Byter;

public abstract class FormatDefinition
{
    public abstract FormatDefinition Construct(ImmutableArray<string> genericParameters);
}