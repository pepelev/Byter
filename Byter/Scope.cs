using System.Collections.Immutable;

namespace Byter;

public sealed class Scope
{
    private readonly ImmutableDictionary<string, Format> scope;

    private Scope(ImmutableDictionary<string, Format> scope)
    {
        this.scope = scope;
    }

    public bool Contains(string name) => scope.ContainsKey(name);
    public Format Get(string name) => scope[name];

    public static Scope Default => new(
        ImmutableDictionary<string, Format>.Empty
            .Add("Int32", null)
            .Add("String", null)
    );

    public Scope Add(string name, Format format) => new(
        scope.SetItem(name, format)
    );
}