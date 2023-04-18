using System.Collections.Generic;

namespace WebCom.Resolvers;

public interface IResolver<T>
{
    public IEnumerable<T> Resolve();
}

public abstract class Resolver<T> : IResolver<T>
{
    public abstract IEnumerable<T> Resolve();
}