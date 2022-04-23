using System;
using System.Collections.Generic;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Registries;

public class IdentifierRegistry<K, V> : Registry<V> where V : IIdentifiable<K>
{
    protected readonly Dictionary<K, V> identifiables;

    protected IdentifierRegistry(Dictionary<K, V> identifiables)
    {
        this.identifiables = identifiables;
    }

    public IdentifierRegistry() : this(new()) { }

    public override void Add(V item)
    {
        base.Add(item);
        identifiables.Add(item.Identifier, item);
    }

    public V this[K identifier] => identifiables[identifier];
}

public class StringIdentifierRegistry<T> : IdentifierRegistry<string, T> where T : IIdentifiable<string>
{
    public StringIdentifierRegistry() : base(new(StringComparer.OrdinalIgnoreCase))
    {
    }
}
