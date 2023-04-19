﻿using System.Collections.Immutable;

namespace Byter;

public sealed class EnumFormatDefinition : FormatDefinition
{
    private readonly Alias tagFormat;
    private readonly IEnumerable<EnumVariant> variants;

    public EnumFormatDefinition(Alias tagFormat, IEnumerable<EnumVariant> variants)
    {
        this.tagFormat = tagFormat;
        this.variants = variants;
    }

    public override FormatDefinition Construct(ImmutableArray<string> genericParameters, ImmutableArray<long> regularParameters)
    {
        throw new NotImplementedException();
    }
}