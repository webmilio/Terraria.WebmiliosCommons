using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebmilioCommons.Registries;

public class Registry<T>
{
    protected readonly List<T> items = new();

    public Registry()
    {
        Items = items.AsReadOnly();
    }

    public virtual void Add(T item) => items.Add(item);
    public virtual void AddRange(IEnumerable<T> source) => items.AddRange(source);

    public ReadOnlyCollection<T> Items { get; }
}