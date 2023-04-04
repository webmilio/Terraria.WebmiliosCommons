namespace WebCom.Serializers;

public class XmlSerializer : Serializer<XmlSerializer.DataReader, XmlSerializer.DataWriter>
{
    public delegate object DataReader(object data);
    public delegate void DataWriter(object data);
}

public class XmlSerializers : Serializers<XmlSerializer> { }
