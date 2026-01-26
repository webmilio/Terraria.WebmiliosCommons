using System.Linq.Expressions;
using System.Reflection;
using WebCom.Extensions;
using WebCom.Net.v2;
using WebCom.Net.v2.Serialization;

namespace WebCom.Tests.Net.v2;

[TestClass]
public class MessageExpressionHelpersTests
{
  private const string Reading = "Reading";
  private const string Writing = "Writing";

  private static readonly BinarySerializers serializers = new BinarySerializers.DefaultPacketSerializers();

  #region Creator
  [TestMethod]
  public void GetCreator_ReturnsNotNull()
  {
    var lambda = MessageExpressionHelpers.GetCreator(typeof(XMessage));
    Assert.IsNotNull(lambda);
  }

  [TestMethod]
  public void GetCreator_LambdaReturnsInstance()
  {
    var lambda = MessageExpressionHelpers.GetCreator(typeof(XMessage));
    var instance = lambda();

    Assert.IsInstanceOfType<XMessage>(instance);
  }
  #endregion

  #region Sending
  [TestMethod]
  public void PreSendExpression_ReturnsTrue()
  {
    var handler = new EmptyMessageHandler();
    var method = handler.GetType().GetMethod(nameof(EmptyMessageHandler.PreSend));
    var expr = MessageExpressionHelpers.ForDelegate<HandlerOrchestrator.PreSendExpression>(handler, method);

    Assert.IsNotNull(expr);
    Assert.IsTrue(expr(null, null));
  }

  [TestMethod]
  public void SendExpression_Throws()
  {
    var handler = new EmptyMessageHandler();
    var method = handler.GetType().GetMethod(nameof(EmptyMessageHandler.Send));
    var expr = MessageExpressionHelpers.ForDelegate<HandlerOrchestrator.SendExpression>(handler, method);

    Assert.IsNotNull(expr);
    Assert.ThrowsException<Exception>(() => expr(null, null));
  }
  #endregion
  
  #region Receiving
  [TestMethod]
  public void PreReceiveExpression_ReturnsTrue()
  {
    var handler = new EmptyMessageHandler();
    var method = handler.GetType().GetMethod(nameof(EmptyMessageHandler.PreReceive));
    var expr = MessageExpressionHelpers.ForDelegate<HandlerOrchestrator.PreReceiveExpression>(handler, method);

    Assert.IsNotNull(expr);
    Assert.IsTrue(expr(null, 0, null));
  }

  [TestMethod]
  public void ReceiveExpression_Throws()
  {
    var handler = new EmptyMessageHandler();
    var method = handler.GetType().GetMethod(nameof(EmptyMessageHandler.Receive));
    var expr = MessageExpressionHelpers.ForDelegate<HandlerOrchestrator.ReceiveExpression>(handler, method);

    Assert.IsNotNull(expr);
    Assert.ThrowsException<Exception>(() => expr(null, 0, null));
  }
  #endregion

  #region Reading/Writing
  private static BlockExpression GetWriteExpression(object instance)
  {
    var members = instance.GetType().GetDataMembers();
    var expr = MessageExpressionHelpers.Serializers(members, serializers);

    return expr;
  }
  
  private static HandlerOrchestrator.WriterExpression GetWriteMethod(Expression expression)
  {
    var lambda = Expression.Lambda<HandlerOrchestrator.WriterExpression>(
      expression,
      MessageExpressionHelpers.Parameters.writer,
      MessageExpressionHelpers.Parameters.instance
    );

    return lambda.Compile();
  }

  [TestCategory(Writing)]
  [TestMethod]
  public void Serializer_WrittenIsValid()
  {
    var message = new XMessage
    {
      AProperty = Guid.NewGuid().ToString()
    };

    var expression = GetWriteExpression(message);
    Assert.IsNotNull(expression);

    // TODO Make sure this doesn't fail in the future, because it sure as hell could.
    using var stream = new MemoryStream(255);
    using var writer = new BinaryWriter(stream);

    var func = GetWriteMethod(expression);
    func(writer, message);

    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new BinaryReader(stream);

    var str = reader.ReadString();
    Assert.AreEqual(message.AProperty, str);
  }
  #endregion

  #region Reading
  private static BlockExpression GetReadExpression(object instance)
  {
    var members = instance.GetType().GetDataMembers();
    var expr = MessageExpressionHelpers.Deserializers(members, serializers);

    return expr;
  }

  private static HandlerOrchestrator.ReaderExpression GetReadMethod(Expression expression)
  {
    var lambda = Expression.Lambda<HandlerOrchestrator.ReaderExpression>(
      expression,
      MessageExpressionHelpers.Parameters.reader,
      MessageExpressionHelpers.Parameters.instance
    );

    return lambda.Compile();
  }

  [TestCategory(Reading)]
  [TestMethod]
  public void Deserializer_ReadIsValid()
  {
    var str = Guid.NewGuid().ToString();
    var nbr = 42069;

    var message = new XMessage
    {
      AProperty = str,
      NumericalProperty = nbr
    };

    var expression = GetReadExpression(message);
    Assert.IsNotNull(expression);

    // TODO Make sure this doesn't fail in the future, because it sure as hell could.
    using var stream = new MemoryStream(255);
    using var writer = new BinaryWriter(stream);

    writer.Write(message.AProperty);
    writer.Write(message.NumericalProperty);

    writer.Flush();

    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new BinaryReader(stream);

    var func = GetReadMethod(expression);
    func(reader, message);

    Assert.AreEqual(str, message.AProperty);
    Assert.AreEqual(nbr, message.NumericalProperty);
  }
  #endregion
}
