namespace Byter;

public abstract class Format
{
    public abstract byte[] Write(object value);
    public abstract (int Read, object Value) Read(byte[] bytes);
}