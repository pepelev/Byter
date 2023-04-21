namespace Byter;

public sealed record RecordField(string Format, string Name)
{
    public override string ToString() => $"{Format} {Name}";
}