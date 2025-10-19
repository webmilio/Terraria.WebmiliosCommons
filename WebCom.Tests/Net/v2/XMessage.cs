using WebCom.Annotations;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

internal class XMessage : IMessage
{
    [Skip] internal Func<BinaryReader, int, bool> OverridePreReceive { get; init; } = (reader, fromWho) => true;
    [Skip] internal Func<bool> OverridePreSend { get; init; } = () => true;
    [Skip] internal Action<BinaryReader, int> OverrideReceive { get; init; } = (reader, fromWho) => { };

    public XMessage()
    { 
    }

    public string? AProperty { get; set; }

    public int NumericalProperty { get; set; }

    public bool PreReceive(BinaryReader reader, int fromWho)
    {
        return OverridePreReceive(reader, fromWho);
    }

    public bool PreSend()
    {
        return OverridePreSend();
    }

    public void Receive(BinaryReader reader, int fromWho)
    {
        OverrideReceive(reader, fromWho);
    }
}