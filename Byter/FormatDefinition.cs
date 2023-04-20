namespace Byter;

public abstract class FormatDefinition
{
    public abstract T Accept<T>(Visitor<T> visitor);

    public abstract class Visitor<T>
    {
        public abstract T Visit(Alias alias);
        public abstract T Visit(Const @const);
        public abstract T Visit(EnumFormatDefinition @enum);
        public abstract T Visit(TwosComplementNumber number);
        public abstract T Visit(RecordFormatDefinition record);
    }
}