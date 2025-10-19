using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using WebCom.Net.v2;
using WebCom.Net.v2.Serialization;

namespace WebCom.Tests.Net.v2;

[TestClass]
public class MessageHandlerTests
{
    private static readonly BinarySerializers _serializers = new BinarySerializers.DefaultPacketSerializers();

    [TestMethod]
    public void MessageHandler_SendWritesToBuffer()
    {
        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        var message = new XMessage()
        {
            AProperty = "Adam",
            NumericalProperty = 69420
        };
        var messageHandler = new MessageHandler(typeof(XMessage), _serializers);

        messageHandler.Send(message, writer);

        Assert.AreNotEqual(0, stream.Position);
    }

    [TestMethod]
    public void MessageHandler_ReadReadsFromBuffer()
    {
        var mapped = new MessageHandler(typeof(XMessage), _serializers);

        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        var src = new XMessage()
        {
            AProperty = Guid.NewGuid().ToString(),
            NumericalProperty = RandomNumberGenerator.GetInt32(int.MaxValue)
        };

        mapped.Send(src, writer);

        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new BinaryReader(stream);

        var message = (XMessage) mapped.Receive(reader, 0);
        
        Assert.AreEqual(src.AProperty, message.AProperty);
        Assert.AreEqual(src.NumericalProperty, message.NumericalProperty);
    }
}
