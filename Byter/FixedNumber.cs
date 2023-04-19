using System.Collections.Immutable;

namespace Byter;

public sealed class FixedNumber : FormatDefinition
{
    private readonly Type type;
    private readonly int size;
    private readonly Endianness endianness;

    public FixedNumber(Type type, int size, Endianness endianness)
    {
        this.type = type;
        this.size = size;
        this.endianness = endianness;
    }

    public enum Type : byte
    {
        Signed,
        Unsigned
    }
    
    public enum Endianness : byte
    {
        Little,
        Big
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters, ImmutableArray<long> regularParameters)
    {
        throw new NotImplementedException();
    }
}