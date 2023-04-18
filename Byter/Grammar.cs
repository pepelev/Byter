using System.Collections.Immutable;
using Sprache;

namespace Byter;

public sealed class FormatDescription
{
    public FormatDescription(FormatDeclaration declaration, FormatDefinition definition)
    {
        Declaration = declaration;
        Definition = definition;
    }

    public FormatDeclaration Declaration { get; }
    public FormatDefinition Definition { get; }
}

public sealed record EnumVariant(
    string Name,
    int Tag,
    FormatDefinition FormatDefinition
);

public abstract class FormatDefinition
{
    public abstract FormatDefinition Construct(ImmutableArray<string> genericParameters);
}

public sealed class Alias : FormatDefinition
{
    private readonly FormatDeclaration declaration;

    public Alias(FormatDeclaration declaration)
    {
        this.declaration = declaration;
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters)
    {
        throw new NotImplementedException();
    }
}

public sealed class RecordFormatDefinition : FormatDefinition
{
    public FormatDeclaration Declaration { get; }
    public NamedRecord Record { get; }


    public override FormatDefinition Construct(ImmutableArray<string> genericParameters)
    {
        if (Declaration.GenericParameters.Length != genericParameters.Length) throw new InvalidOperationException();

        throw new NotImplementedException();
    }
}

public sealed class Grammar
{
    public Parser<None> Whitespace => Parse.WhiteSpace.Many().IgnoreResult();
    public Parser<None> Comma => Parse.Char(',').IgnoreResult();
    public Parser<None> OpenCurlyBracket => Parse.Char('{').IgnoreResult();
    public Parser<None> CloseCurlyBracket => Parse.Char('}').IgnoreResult();
    public Parser<None> LessThan => Parse.Char('<').IgnoreResult();
    public Parser<None> GreaterThan => Parse.Char('>').IgnoreResult();
    public Parser<None> EqualSign => Parse.Char('=').IgnoreResult();
    public Parser<None> Arrow => Parse.String("=>").IgnoreResult();

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

    public Parser<FormatDeclaration> FormatDeclaration =>
        from name in FormatName
        from whitespace in Whitespace
        from parameters in GenericParameters.Optional()
        select new FormatDeclaration(
            name,
            parameters.GetOrElse(ImmutableArray<string>.Empty)
        );

    public Parser<FormatDefinition> FormatDefinition =>
        Record.Select(fields => new RecordFormatDefinition())
            .Or<FormatDefinition>(Alias)
            .Or(Enum)
            .Or(Const);

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
        public static Parser<None> Record => Parse.String("record").IgnoreResult();
        public static Parser<None> Byte => Parse.String("byte").IgnoreResult();
        public static Parser<None> Enum => Parse.String("enum").IgnoreResult();
    }
}

public sealed class Const : FormatDefinition
{
    private readonly string hex;

    public Const(string hex)
    {
        this.hex = hex;
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters)
    {
        throw new NotImplementedException();
    }
}

public sealed class EnumFormatDefinition : FormatDefinition
{
    private readonly Alias tagFormat;
    private readonly IEnumerable<EnumVariant> variants;

    public EnumFormatDefinition(Alias tagFormat, IEnumerable<EnumVariant> variants)
    {
        this.tagFormat = tagFormat;
        this.variants = variants;
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters)
    {
        throw new NotImplementedException();
    }
}