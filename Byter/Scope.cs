using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using Optional;
using Optional.Collections;

namespace Byter;

public sealed class Scope
{
    private readonly ImmutableDictionary<Key, Element> elements;

    private Scope(ImmutableDictionary<Key, Element> elements)
    {
        this.elements = elements;
    }

    public static Scope Empty => new(
        ImmutableDictionary<Key, Element>.Empty
    );

    [Pure]
    public Option<Element> TryGet(string name, int genericParameters, int regularParameters)
    {
        var key = new Key(name, genericParameters, regularParameters);
        return elements.GetValueOrNone(key);
    }

    [Pure]
    public Scope AddFormatDescription(FormatDescription description)
    {
        var declaration = description.Declaration;
        var key = new Key(
            declaration.Name,
            declaration.GenericParameters.Length,
            declaration.RegularParameters.Length
        );
        var element = new Description(description);
        return Add(key, element);
    }

    [Pure]
    public Scope AddGenericParameter(string name)
    {
        var key = new Key(name, 0, 0);
        var element = new GenericParameter(name);
        return Add(key, element);
    }

    [Pure]
    public Scope AddRegularParameter(string name)
    {
        var key = new Key(name, 0, 0);
        var element = new RegularParameter(name);
        return Add(key, element);
    }

    [Pure]
    private Scope Add(Key key, Element element)
    {
        var newElements = elements.Add(key, element);
        return new Scope(newElements);
    }

    private readonly record struct Key(string Name, int GenericParameters, int RegularParameters);

    public abstract class Element
    {
    }

    public sealed class Description : Element
    {
        public Description(FormatDescription value)
        {
            Value = value;
        }

        public FormatDescription Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class GenericParameter : Element
    {
        public GenericParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class RegularParameter : Element
    {
        public RegularParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}