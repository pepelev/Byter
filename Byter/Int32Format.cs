namespace Byter;

public sealed class Int32Format : Format
{
    public override byte[] Write(object value)
    {
        var result = new byte[4];
        // todo
        return result;
    }

    public override object Read(byte[] bytes)
    {
        throw new NotImplementedException();
    }
}