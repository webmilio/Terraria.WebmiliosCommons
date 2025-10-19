using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

[TestClass]
public class MessageLoaderTests
{
    public class XMessage
    {
        public XMessage(int x) { }
    }

    public class YMessage 
    {
        public YMessage() { }
        public YMessage(int x) { }
    }

    [TestMethod]
    public void SendingMessage_WritesToStream()
    {

    }

    [TestMethod("Validating a class with no parameterless constructor should throw.")]
    public void ValidateType_ShouldThrowOnNoParameterlessCtor()
    {
        Assert.ThrowsException<NotSupportedException>(() => MessageLoader.ValidateMessageType(typeof(XMessage)));
    }

    [TestMethod("Validating a class with a parameterless constructor shouldn't throw.")]
    public void ValidateType_ShouldntThrowOnParameterlessCtor()
    {
        MessageLoader.ValidateMessageType(typeof(YMessage));
    }
}
