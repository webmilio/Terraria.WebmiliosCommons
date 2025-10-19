using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using WebCom.Extensions;
using WebCom.Net.v2;
using WebCom.Net.v2.Serialization;

namespace WebCom.Tests.Net.v2;

[TestClass]
public class MessageHandlerHelpersTests
{
    private const string Reading = "Reading";
    private const string Writing = "Writing";

    private static readonly BinarySerializers _serializers = new BinarySerializers.DefaultPacketSerializers();

    [TestMethod]
    public void GetCreator_ReturnsNotNull()
    {
        var lambda = MessageHandlerHelpers.GetCreator(typeof(XMessage));
        Assert.IsNotNull(lambda);
    }

    [TestMethod]
    public void GetCreator_LambdaReturnsInstance()
    {
        var lambda = MessageHandlerHelpers.GetCreator(typeof(XMessage));
        var instance = lambda();

        Assert.IsInstanceOfType<XMessage>(instance);
    }

    #region Writing
    private static Expression<MessageHandler.WriterExpression> GetWriteExpression(object instance)
    {
        var member = instance.GetType().GetDataMembers(
            BindingFlags.Instance | BindingFlags.Public,
            BindingFlags.Instance | BindingFlags.Public
        ).First();
        return MessageHandlerHelpers.MapMemberWriteToStream(member, _serializers);
    }

    [TestCategory(Writing)]
    [TestMethod]
    public void MapMemberWriteToStream_CreatesExpression()
    {
        var message = new XMessage();
        var expression = GetWriteExpression(message);

        Assert.IsNotNull(expression);
    }

    [TestCategory(Writing)]
    [TestMethod]
    public void MapMemberWriteToStream_ExpressionWrites()
    {
        var message = new XMessage
        {
            AProperty = Guid.NewGuid().ToString()
        };
        var expression = GetWriteExpression(message);

        // TODO Make sure this doesn't fail in the future, because it sure as hell could.
        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        var lambda = expression.Compile();
    }

    [TestCategory(Writing)]
    [TestMethod]
    public void MapMemberWriteToStream_WrittenIsValid()
    {
        var message = new XMessage
        {
            AProperty = Guid.NewGuid().ToString()
        };
        var expression = GetWriteExpression(message);

        // TODO Make sure this doesn't fail in the future, because it sure as hell could.
        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        var lambda = expression.Compile();
        lambda(writer, message);

        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new BinaryReader(stream);

        var str = reader.ReadString();
        Assert.AreEqual(message.AProperty, str);
    }
    #endregion

    #region Reading
    private static Expression<MessageHandler.ReaderExpression> GetReadExpression(object instance)
    {
        var member = instance.GetType().GetDataMembers(
            BindingFlags.Instance | BindingFlags.Public,
            BindingFlags.Instance | BindingFlags.Public
        ).First();
        return MessageHandlerHelpers.MapMemberReadFromStream(member, _serializers);
    }

    [TestCategory(Reading)]
    [TestMethod]
    public void MapMemberReadFromStream_CreatesExpression()
    {
        var message = new XMessage();
        var expression = GetReadExpression(message);

        Assert.IsNotNull(expression);
    }

    [TestCategory(Reading)]
    [TestMethod]
    public void MapMemberReadFromStream_CreatesNotNull()
    {
        var message = new XMessage();
        var expression = GetReadExpression(message);

        // TODO Make sure this doesn't fail in the future, because it sure as hell could.
        using var stream = new MemoryStream(255);
        using var reader = new BinaryReader(stream);

        var lambda = expression.Compile();
    }

    [TestCategory(Reading)]
    [TestMethod]
    public void MapMemberReadFromStream_ReadIsValid()
    {
        var str = Guid.NewGuid().ToString();
        var message = new XMessage
        {
            AProperty = str
        };
        var expression = GetReadExpression(message);

        // TODO Make sure this doesn't fail in the future, because it sure as hell could.
        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        writer.Write(message.AProperty);
        
        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new BinaryReader(stream);

        var lambda = expression.Compile();
        lambda(reader, message);
        
        Assert.AreEqual(str, message.AProperty);
    }
    #endregion

    #region Packing
    [TestCategory("Packaging")]
    [TestMethod]
    public void MapExpressions_CreatesValid()
    {
        var message = new XMessage();
        var expressions = MessageHandlerHelpers.MapExpressions(message.GetType(), _serializers);

        Assert.IsNotNull(expressions.reader);
        Assert.IsNotNull(expressions.writer);
    }

    [TestCategory("Packaging")]
    [TestMethod]
    public void CompileExpressions_CreatesNotNull()
    {
        var message = new XMessage();
        var (reader, writer) = MessageHandlerHelpers.CompileExpressions(message.GetType(), _serializers);

        Assert.IsNotNull(reader);
        Assert.IsNotNull(writer);
    }

    [TestCategory("Packaging")]
    [TestMethod]
    public void CompileExpressions_WriteDoesSomething()
    {
        var message = new XMessage
        {
            AProperty = Guid.NewGuid().ToString(),
            NumericalProperty = RandomNumberGenerator.GetInt32(int.MaxValue)
        };
        var (_, exprWriter) = MessageHandlerHelpers.CompileExpressions(message.GetType(), _serializers);

        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        exprWriter(writer, message);

        Assert.AreNotEqual(0, stream.Position);
    }

    [TestCategory("Packaging")]
    [TestMethod]
    public void CompileExpressions_ReadWrite()
    {
        var aProperty = Guid.NewGuid().ToString();
        var numericalProperty = RandomNumberGenerator.GetInt32(int.MaxValue);

        var message = new XMessage
        {
            AProperty = aProperty,
            NumericalProperty = numericalProperty
        };
        var (exprReader, exprWriter) = MessageHandlerHelpers.CompileExpressions(message.GetType(), _serializers);

        Assert.IsNotNull(exprReader);
        Assert.IsNotNull(exprWriter);

        using var stream = new MemoryStream(255);
        using var writer = new BinaryWriter(stream);

        exprWriter(writer, message);
        writer.Seek(0, SeekOrigin.Begin);

        message.AProperty = string.Empty;
        message.NumericalProperty = 0;

        using var reader = new BinaryReader(stream);

        exprReader(reader, message);

        Assert.AreEqual(aProperty, message.AProperty);
        Assert.AreEqual(numericalProperty, message.NumericalProperty);
    }
    #endregion
}
