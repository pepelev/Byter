using System.Collections.Immutable;

namespace Byter;

public sealed class RecordFormatDefinition : FormatDefinition
{
    public FormatDeclaration Declaration { get; }
    public NamedRecord Record { get; }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters, ImmutableArray<long> regularParameters)
    {
        if (Declaration.GenericParameters.Length != genericParameters.Length) throw new InvalidOperationException();

        throw new NotImplementedException();
    }
}