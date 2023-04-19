using System.Collections.Immutable;
using FluentAssertions;
using NUnit.Framework;
using Sprache;

namespace Byter;

public sealed class Tests
{
    [Test]
    public void Field()
    {
        var grammar = new Grammar();
        var field = grammar.Field.Parse("String name");
        Assert.AreEqual(
            ("String", "name"),
            field
        );
    }

    [Test]
    public void Empty_Record()
    {
        var grammar = new Grammar();
        var fields = grammar.Record.Parse("record{}");
        CollectionAssert.IsEmpty(fields);
    }

    [Test]
    public void Record_With_Single_Field()
    {
        var grammar = new Grammar();
        var fields = grammar.Record.Parse("record{String name}");
        CollectionAssert.AreEqual(
            new[] { ("String", "name") },
            fields
        );
    }

    [Test]
    public void Several_Fields_With_No_Spaces()
    {
        var grammar = new Grammar();
        var fields = grammar.Record.Parse("record{String name,Int age}");
        CollectionAssert.AreEqual(
            new[] { ("String", "name"), ("Int", "age") },
            fields
        );
    }

    [Test]
    public void Several_Fields()
    {
        var grammar = new Grammar();
        var fields = grammar.Record.Parse(@"record {
    String name,
    Int age
}");
        CollectionAssert.AreEqual(
            new[] { ("String", "name"), ("Int", "age") },
            fields
        );
    }

    [Test]
    public void Several_Fields_With_Trailing_Comma()
    {
        var grammar = new Grammar();
        var fields = grammar.Record.Parse(@"record {
    String name,
    Int age,
}");
        CollectionAssert.AreEqual(
            new[] { ("String", "name"), ("Int", "age") },
            fields
        );
    }

    [Test]
    public void Named_Record()
    {
        var grammar = new Grammar();
        var namedRecord = grammar.NamedRecord.Parse(@"Person = record {
    String name,
    Int age,
}");
        namedRecord.Should().BeEquivalentTo(
            new NamedRecord(
                new FormatDeclaration(
                    "Person",
                    ImmutableArray<string>.Empty,
                    ImmutableArray<string>.Empty
                ),
                new[] { ("String", "name"), ("Int", "age") }
            )
        );
    }

    [Test]
    public void Several_Named_Records()
    {
        var grammar = new Grammar();
        var records = grammar.File.Parse(@"
Person = record {
    String name,
    Int age,
}
Sell = record {
    Person buyer,
    String product,
    Int price
}
");
        records.Should().BeEquivalentTo(
            new[]
            {
                new NamedRecord(
                    new FormatDeclaration(
                        "Person",
                        ImmutableArray<string>.Empty,
                        ImmutableArray<string>.Empty
                    ),
                    new[] { ("String", "name"), ("Int", "age") }
                ),
                new NamedRecord(
                    new FormatDeclaration(
                        "Sell",
                        ImmutableArray<string>.Empty,
                        ImmutableArray<string>.Empty
                    ),
                    new[] { ("Person", "buyer"), ("String", "product"), ("Int", "price") }
                )
            }
        );
    }

    [Test]
    public void Several_Record_Formats()
    {
        var grammar = new Grammar();
        var records = grammar.File.Parse(@"
Person = record {
    String name,
    Int32 age,
}
Sell = record {
    Person buyer,
    String product,
    Int32 price
}
");
        // var scope = Scope.Default;
        // foreach (var namedRecord in records)
        // {
        //     if (scope.Contains(namedRecord.Declaration.Name))
        //     {
        //         throw new Exception("Duplicate name");
        //     }
        //
        //     foreach (var (format, fieldName) in namedRecord.Record)
        //     {
        //         if (!scope.Contains(format))
        //         {
        //             throw new Exception($"Unknown format name: {format}");
        //         }
        //     }
        //
        //     var newFormat = new RecordFormat(scope, namedRecord);
        //     scope = scope.Add(namedRecord.Declaration.Name, newFormat);
        // }
    }

    [Test]
    public void Generic_Type()
    {
        var grammar = new Grammar();
        var records = grammar.File.Parse(@"Named<T> = record { String name, T content }");
        // var scope = Scope.Default;
        // foreach (var namedRecord in records)
        // {
        //     if (scope.Contains(namedRecord.Declaration.Name))
        //     {
        //         throw new Exception("Duplicate name");
        //     }
        //
        //     var innerScope = scope;
        //     foreach (var parameter in namedRecord.Declaration.GenericParameters)
        //     {
        //         innerScope = innerScope.Add(parameter, new GenericPlaceholder());
        //     }
        //
        //     foreach (var (format, fieldName) in namedRecord.Record)
        //     {
        //         if (!innerScope.Contains(format))
        //         {
        //             throw new Exception($"Unknown format name: {format}");
        //         }
        //     }
        //
        //     var newFormat = new RecordFormat(scope, namedRecord);
        //     scope = scope.Add(namedRecord.Declaration.Name, newFormat);
        // }
    }

    [Test]
    public void Regular_Parameter()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescription.Parse(@"Named(size) = Int<Int>(size)");
    }

    [Test]
    [Explicit]
    public void Failure_Regular_Parameter()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescription.Parse(@"Named( size ) = 0x1234");
    }

    [Test]
    [Explicit]
    public void Failure_Generic_Type()
    {
        var grammar = new Grammar();
        var records = grammar.File.Parse(@"Named< T> = record { String name, T content }");
    }

    [Test]
    public void Alias()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescriptions.Parse(@"Number<T> = Int32 A = Number<Int32> B = A C = record { String name }");
    }

    [Test]
    public void Enum()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescriptions.Parse(@"Maybe<T> = enum<Byte> { nothing 0 => Unit, just 1 => T }");
    }

    [Test]
    public void Hex()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescriptions.Parse(@"Signature = 0xDEADBEEF");
    }

    [Test]
    public void Numbers()
    {
        var grammar = new Grammar();
        var records = grammar.FormatDescriptions.Parse(@"
Byte   = byte
Int32  = number   signed 4@bytes little endian
UInt32 = number unsigned 4@bytes little endian
Int64  = number   signed 8@bytes big    endian
UInt64 = number unsigned 8@bytes big    endian");
    }

    // Description:
    // - V records
    // - V generic parameters
    // - V enums
    // - V aliases
    // - V regular parameters
    // -   uft8 encoding
    // - V hex literals
    // - V arbitrary number sizes
    // - V number endianness
    // -   7bit encoding
    // -   anonymous types (use general format description instead of alias only)
    // -   comments
    // -   surrogate pair first name letter
    // -   checks: endless format (cycles)

    // Write-Read:
    // -    fixed numbers
    // -    uft8 number format
    // -    fixed length arrays
    // -    variable length arrays
    // -    records
    // -    generic parameters
    // -    enums
    // -    regular parameters
    // -    hex
    // -    arbitrary number sizes
    // -    7bit encoding
}