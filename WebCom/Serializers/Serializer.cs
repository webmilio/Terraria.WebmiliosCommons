using System;

namespace WebCom.Serializers;

public class Serializer<TR, TW> where TR : Delegate where TW : Delegate
{
    public Serializer() { }

    public Serializer(TR reader, TW writer)
    {
        Reader = reader;
        Writer = writer;
    }

    public TR Reader { get; set; }

    public TW Writer { get; set; }
}
