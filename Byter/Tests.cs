﻿using FluentAssertions;
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
                "Person",
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
                    "Person",
                    new[] { ("String", "name"), ("Int", "age") }
                ),
                new NamedRecord(
                    "Sell",
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
        var scope = Scope.Default;
        foreach (var namedRecord in records)
        {
            if (scope.Contains(namedRecord.Name))
            {
                throw new Exception("Duplicate name");
            }

            foreach (var (format, fieldName) in namedRecord.Record)
            {
                if (!scope.Contains(format))
                {
                    throw new Exception($"Unknown format name: {format}");
                }
            }

            var newFormat = new RecordFormat(scope, namedRecord);
            scope = scope.Add(namedRecord.Name, newFormat);
        }
    }
    // Features:
    // - V records
    // -   generic parameters
    // -   variable length arrays
    // -   enums
    // -   aliases
    // -   regular parameters
    // -   fixed length arrays
    // -   uft8 number format
    // -   arbitrary number sizes
    // -   number endianness
    // -   7bit encoding
    // -   anonymous types
    // -   comments
}