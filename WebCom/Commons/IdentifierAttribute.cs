using System;

namespace WebCom.Commons;

public class IdentifierAttribute<T> : Attribute, IIdentifiable<T>
{
	public IdentifierAttribute(T identifier)
	{
		Identifier = identifier;
	}

    public T Identifier { get; }
}