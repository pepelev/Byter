namespace Byter;

public sealed class RecordFormat : Format
{
    private readonly Scope scope;
    private readonly NamedRecord record;

    public RecordFormat(Scope scope, NamedRecord record)
    {
        this.scope = scope;
        this.record = record;
    }

    public override byte[] Write(object value)
    {
        throw new NotImplementedException();
    }

    public override object Read(byte[] bytes)
    {
        throw new NotImplementedException();
    }
}