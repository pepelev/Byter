using System.Collections.Immutable;
using Sprache;

namespace Byter;

public sealed class Grammar
{
    public Parser<Unit> Whitespace => Parse.WhiteSpace.Many().IgnoreResult();
    public Parser<Unit> Comma => Parse.Char(',').IgnoreResult();
    public Parser<Unit> OpenCurlyBracket => Parse.Char('{').IgnoreResult();
    public Parser<Unit> CloseCurlyBracket => Parse.Char('}').IgnoreResult();
    public Parser<Unit> OpenBracket => Parse.Char('(').IgnoreResult();
    public Parser<Unit> CloseBracket => Parse.Char(')').IgnoreResult();
    public Parser<Unit> LessThan => Parse.Char('<').IgnoreResult();
    public Parser<Unit> GreaterThan => Parse.Char('>').IgnoreResult();
    public Parser<Unit> EqualSign => Parse.Char('=').IgnoreResult();
    public Parser<Unit> Arrow => Parse.String("=>").IgnoreResult();
    public Parser<Unit> UnitOfMeasure => Parse.Char('@').IgnoreResult();

    public Parser<string> FormatName =>
        from first in Parse.Upper
        from rest in Parse.LetterOrDigit.Many().Text()
        select first + rest;

    public Parser<ImmutableArray<string>> GenericParameters =>
        from open in LessThan
        from parameters in FormatName.SeparatedBy(Comma.Token())
        let parameterArray = parameters.ToImmutableArray()
        where parameterArray.Length > 0 && parameterArray.Distinct().Count() == parameterArray.Length
        from close in GreaterThan
        select parameterArray;

    public Parser<ImmutableArray<string>> RegularParameters =>
        from open in OpenBracket
        from parameters in FieldName.SeparatedBy(Comma.Token())
        let parameterArray = parameters.ToImmutableArray()
        where parameterArray.Length > 0 && parameterArray.Distinct().Count() == parameterArray.Length
        from close in CloseBracket
        select parameterArray;

    public Parser<FormatDeclaration> FormatDeclaration =>
        from name in FormatName
        from whitespace in Whitespace
        from genericParameters in GenericParameters.Optional()
        from regularParameters in RegularParameters.Optional()
        select new FormatDeclaration(
            name,
            genericParameters.GetOrElse(ImmutableArray<string>.Empty),
            regularParameters.GetOrElse(ImmutableArray<string>.Empty)
        );

    public Parser<FormatDefinition> FormatDefinition =>
        Record.Select(fields => new RecordFormatDefinition())
            .Or<FormatDefinition>(Alias)
            .Or(Enum)
            .Or(Const)
            .Or(Byte)
            .Or(FixedNumber);

    public Parser<Alias> Alias =>
        FormatDeclaration
            .Select(declaration => new Alias(declaration));

    public Parser<string> FieldName =>
        from first in Parse.Lower
        from rest in Parse.LetterOrDigit.Many().Text()
        select first + rest;

    public Parser<(string Format, string Name)> Field =>
        from format in FormatName
        from space in Whitespace
        from name in FieldName
        select (format, name);

    public Parser<(string Format, string Name)[]> Record =>
        from keyword in Keywords.Record.Token()
        from open in OpenCurlyBracket
        from fields in Field.SeparatedBy(Comma.Token()).Token()
        from close in CloseCurlyBracket
        select fields.ToArray();

    public Parser<EnumVariant> EnumVariant =>
        from name in FieldName
        from space in Whitespace
        from tag in Parse.Number.Select(int.Parse) // todo overflow
        from arrow in Arrow.Token()
        from formatDefinition in FormatDefinition
        select new EnumVariant(name, tag, formatDefinition);

    public Parser<EnumFormatDefinition> Enum =>
        from keyword in Keywords.Enum
        from whitespace in Whitespace
        from openTag in LessThan
        from tagFormat in Alias.Token()
        from closeTag in GreaterThan
        from whitespace2 in Whitespace
        from openVariants in OpenCurlyBracket
        from variants in EnumVariant.SeparatedBy(Comma.Token()).Token() 
        from closeVariants in CloseCurlyBracket
        select new EnumFormatDefinition(tagFormat, variants);

    public Parser<Const> Const =>
        from zero in Parse.Char('0')
        from x in Parse.Char('x')
        from hex in Parse.Chars("0123456789ABCDEFabcdef").Repeat(2).AtLeastOnce()
        select new Const(new string(hex.SelectMany(x => x).ToArray()));

    public Parser<FixedNumber> Byte =>
        Keywords.Byte.Select(_ => new FixedNumber(Byter.FixedNumber.Type.Unsigned, 1, Byter.FixedNumber.Endianness.Little));

    private Parser<T> Case<T>(params (Parser<Unit> Parser, T Result)[] variants)
    {
        if (variants.Length == 0)
        {
            return input => Result.Failure<T>(input, "no variants", Array.Empty<string>());
        }

        var parser = variants[0].Parser.Select(_ => variants[0].Result);
        for (var i = 1; i < variants.Length; i++)
        {
            var variant = variants[i];
            var next = variant.Parser.Select(_ => variant.Result);
            parser = parser.Or(next);
        }

        return parser;
    }

    public Parser<FixedNumber> FixedNumber =>
        from number in Keywords.Number
        from whitespace in Whitespace
        from type in Case(
            (Keywords.Signed, Byter.FixedNumber.Type.Signed),
            (Keywords.Unsigned, Byter.FixedNumber.Type.Unsigned)
        )
        from whitespace2 in Whitespace
        from size in Parse.Number.Select(int.Parse) // todo overflow
        from unitOfMeasure in UnitOfMeasure
        from bytes in Keywords.Bytes
        from whitespace3 in Whitespace
        from endianness in Case(
            (Keywords.Little, Byter.FixedNumber.Endianness.Little),
            (Keywords.Big, Byter.FixedNumber.Endianness.Big)
        )
        from whitespace4 in Whitespace
        from endian in Keywords.Endian.Select(y => y)
        select new FixedNumber(type, size, endianness);

    // todo outdated
    public Parser<NamedRecord> NamedRecord =>
        from declaration in FormatDeclaration
        from equal in EqualSign.Token()
        from record in Record
        select new NamedRecord(declaration, record);

    public Parser<FormatDescription> FormatDescription =>
        from declaration in FormatDeclaration
        from equal in EqualSign.Token()
        from definition in FormatDefinition
        select new FormatDescription(declaration, definition);

    public Parser<IEnumerable<FormatDescription>> FormatDescriptions =>
        FormatDescription.Token().Many().End();

    public Parser<IEnumerable<NamedRecord>> File =>
        NamedRecord.Token().Many().End();

    public static class Keywords
    {
        public static Parser<Unit> Record => Parse.String("record").IgnoreResult();
        public static Parser<Unit> Enum => Parse.String("enum").IgnoreResult();
        public static Parser<Unit> Byte => Parse.String("byte").IgnoreResult();
        public static Parser<Unit> Bytes => Parse.String("bytes").IgnoreResult();
        public static Parser<Unit> Number => Parse.String("number").IgnoreResult();
        public static Parser<Unit> Signed => Parse.String("signed").IgnoreResult();
        public static Parser<Unit> Unsigned => Parse.String("unsigned").IgnoreResult();
        public static Parser<Unit> Little => Parse.String("little").IgnoreResult();
        public static Parser<Unit> Big => Parse.String("big").IgnoreResult();
        public static Parser<Unit> Endian => Parse.String("endian").IgnoreResult();
    }
}