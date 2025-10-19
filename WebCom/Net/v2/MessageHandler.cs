using System;
using System.IO;
using System.Linq.Expressions;
using WebCom.Net.v2.Serialization;
using WebCom.Serializers;

namespace WebCom.Net.v2;

internal class MessageHandler
{
    internal delegate IMessage CreatorExpression();
    internal delegate void ReaderExpression(BinaryReader reader, object instance);
    internal delegate void WriterExpression(BinaryWriter writer, object instance);

    internal static readonly ParameterExpression paramWriter = Expression.Parameter(typeof(BinaryWriter), "writer");
    internal static readonly ParameterExpression paramReader = Expression.Parameter(typeof(BinaryReader), "reader");
    internal static readonly ParameterExpression paramInstance = Expression.Parameter(typeof(object), "instance");
    internal static readonly ParameterExpression paramValue = Expression.Parameter(typeof(object), "value");

    private readonly CreatorExpression _creator;
    private readonly ReaderExpression _reader;
    private readonly WriterExpression _writer;

    public MessageHandler(Type type, Serializers<BinarySerializer> serializers)
    {
        _creator = MessageHandlerHelpers.GetCreator(type);

        var (reader, writer) = MessageHandlerHelpers.CompileExpressions(type, serializers);
        _reader = reader;
        _writer = writer;
    }

    public bool Send(IMessage message, BinaryWriter writer)
    {
        if (!message.PreSend())
        {
            return false;
        }

        _writer(writer, message);
        return true;
    }

    public IMessage Receive(BinaryReader reader, int fromWho)
    {
        var message = _creator();

        if (message.PreReceive(reader, fromWho))
        {
            _reader(reader, message);
            message.Receive(reader, fromWho);
        }

        return message;
    }
}