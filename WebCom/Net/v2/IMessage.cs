namespace WebCom.Net.v2;

public interface IMessage
{
    bool PreSend();

    bool PreReceive();

    void Receive();
}
