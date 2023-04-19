namespace Byter;

public sealed class Const : FormatDefinition
{
    private readonly string hex;

    public Const(string hex)
    {
        this.hex = hex;
    }

    public override string ToString() => $"0x{hex}";
}