using System.Reflection;
using Terraria.ID;
using Terraria.ModLoader;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

[TestClass]
public class MessageLoaderTests
{
  private static readonly Mod mod;

  static MessageLoaderTests()
  {
    MessageLoader.netMode = () => NetmodeID.SinglePlayer;
    MessageLoader.myPlayer = () => 0;

    mod = new Mod();

    var code = mod
      .GetType()
      .GetProperty(nameof(Mod.Code), BindingFlags.Instance | BindingFlags.Public);

    if (code == null)
    {
      throw new Exception($"Failed initializing mock {nameof(Mod)}: can't find property {nameof(Mod.Code)}");
    }

    code.SetValue(mod, typeof(MessageLoaderTests).Assembly);
  }

  [TestMethod]
  public void SendingMessage_WritesToStream()
  {
    var loader = MessageLoader.Get(mod);
    loader.Send(new XMessage()
    {
      AProperty = "test"
    });
  }

  [TestMethod("Validating a class with no parameterless constructor should throw.")]
  public void ValidateType_ShouldThrowOnNoParameterlessCtor()
  {
    Assert.ThrowsException<NotSupportedException>(() => MessageLoader.ValidateMessageType(typeof(YMessage)));
  }

  [TestMethod("Validating a class with a parameterless constructor shouldn't throw.")]
  public void ValidateType_ShouldntThrowOnParameterlessCtor()
  {
    MessageLoader.ValidateMessageType(typeof(XMessage));
  }

  [TestMethod]
  public void Orchestrator_SupportsMultipleMessageTypes()
  {
    var loader = MessageLoader.Get(mod);

    // These message types are only supported by one Handler (which supports both) and throw InvalidOperationException upon receiving.
    Assert.ThrowsException<InvalidOperationException>(() => loader.Send(new MMessage()));
    Assert.ThrowsException<InvalidOperationException>(() => loader.Send(new NMessage()));
  }
}
