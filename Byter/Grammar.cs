using Sprache;

namespace Byter;

public sealed class Grammar
{
    public Parser<None> Whitespace => Parse.WhiteSpace.Many().IgnoreResult();
    public Parser<None> Comma => Parse.Char(',').IgnoreResult();
    public Parser<None> OpenCurlyBracket => Parse.Char('{').IgnoreResult();
    public Parser<None> CloseCurlyBracket => Parse.Char('}').IgnoreResult();
    public Parser<None> EqualSign => Parse.Char('=').IgnoreResult();

    public Parser<string> FormatName =>
        from first in Parse.Upper
        from rest in Parse.LetterOrDigit.Many().Text()
        select first + rest;

    public Parser<string> FieldName =>
        from first in Parse.Lower
        from rest in Parse.LetterOrDigit.Many().Text()
        select first + rest;

    public Parser<(string Format, string Name)> Field =>
        from format in FormatName
        from space in Whitespace
        from name in FieldName
        select (format, name);

    public Parser<None> RecordKeyword = Parse.String("record").IgnoreResult();

    public Parser<(string Format, string Name)[]> Record =>
        from keyword in RecordKeyword.Token()
        from open in OpenCurlyBracket
        from fields in Field.Token().SeparatedBy(Comma).Token()
        from close in CloseCurlyBracket
        select fields.ToArray();

    public Parser<NamedRecord> NamedRecord =>
        from name in FormatName
        from equal in EqualSign.Token()
        from record in Record
        select new NamedRecord(name, record);

    public Parser<IEnumerable<NamedRecord>> File =>
        NamedRecord.Token().Many().End();
}