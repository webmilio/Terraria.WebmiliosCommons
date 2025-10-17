using System.Collections.Generic;

namespace WebCom.Resolvers;

public interface IProvider<T>
{
    public IEnumerable<T> Provide();
}