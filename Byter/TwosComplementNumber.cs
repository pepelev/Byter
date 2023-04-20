namespace Byter;

public sealed class TwosComplementNumber : FormatDefinition
{
    public enum Endianness : byte
    {
        Little,
        Big
    }

    public enum Type : byte
    {
        Signed,
        Unsigned
    }

    private readonly Endianness endianness;
    private readonly int size;
    private readonly Type type;

    public TwosComplementNumber(Type type, int size, Endianness endianness)
    {
        this.type = type;
        this.size = size;
        this.endianness = endianness;
    }

    public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
}