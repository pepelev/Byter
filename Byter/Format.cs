namespace Byter;

public abstract class Format
{
    public abstract byte[] Write(object value);
    public abstract object Read(byte[] bytes);
}