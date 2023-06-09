﻿namespace Byter;

public sealed record EnumVariant(
    string Name,
    int Tag,
    FormatDefinition FormatDefinition
)
{
    public override string ToString() => $"{Name} {Tag} => {FormatDefinition}";
}