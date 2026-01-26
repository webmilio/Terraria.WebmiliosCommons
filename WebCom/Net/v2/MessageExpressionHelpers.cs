using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebCom.Annotations;
using WebCom.Extensions;
using WebCom.Net.v2.Serialization;
using WebCom.Reflection;
using WebCom.Serializers;
using static WebCom.Net.v2.HandlerOrchestrator;

namespace WebCom.Net.v2;

public static class MessageExpressionHelpers
{
  internal static class Parameters
  {
    internal static readonly ParameterExpression writer = Expression.Parameter(typeof(BinaryWriter), "writer");
    internal static readonly ParameterExpression reader = Expression.Parameter(typeof(BinaryReader), "reader");
    internal static readonly ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
    internal static readonly ParameterExpression message = Expression.Parameter(typeof(IMessage), "message");
    internal static readonly ParameterExpression handler = Expression.Parameter(typeof(IMessageHandler), "handler");
    internal static readonly ParameterExpression fromWho = Expression.Parameter(typeof(int), "fromWho");
  }

  internal static (ReaderExpression Reader, WriterExpression Writer) DeserializerSerializer(Type type, Serializers<BinarySerializer> serializers)
  {
    var (readers, writers) = RawDeserializerSerializer(type, serializers);
    var reader = Expression.Lambda<ReaderExpression>(
      readers,
      Parameters.reader,
      Parameters.instance
    );
    var writer = Expression.Lambda<WriterExpression>(
      writers,
      Parameters.writer,
      Parameters.instance
    );

    return (reader.Compile(), writer.Compile());
  }

  internal static (BlockExpression Readers, BlockExpression Writers) RawDeserializerSerializer(Type type, Serializers<BinarySerializer> serializers)
  {
    var members =
      type.GetDataMembers(
          fieldFlags: BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField,
          propertyFlags: BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty
      )
      .Where(member =>
          serializers.Has(member.Type) &&
          !member.Member.HasCustomAttribute<SkipAttribute>()
      );

    var readers = Deserializers(members, serializers);
    var writers = Serializers(members, serializers);

    return (readers, writers);
  }

  internal static BlockExpression Serializers(IEnumerable<MemberInfoWrapper> members, Serializers<BinarySerializer> serializers)
  {
    var expressions = members.Select(m => Serializer(m, serializers));
    return Expression.Block(expressions);
  }

  internal static BlockExpression Deserializers(IEnumerable<MemberInfoWrapper> members, Serializers<BinarySerializer> serializers)
  {
    var expressions = members.Select(m => Deserializer(m, serializers));
    return Expression.Block(expressions);
  }

  internal static InvocationExpression Serializer(MemberInfoWrapper member, Serializers<BinarySerializer> serializers)
  {
    var serializer = GuardGetSerializer(member.Type, serializers);

    // ((T) instance)
    var cast = Expression.Convert(Parameters.instance, member.Member.DeclaringType!);

    // instance.Property
    var access = Expression.PropertyOrField(cast, member.Member.Name);

    // Property.Type -> object, since BinaryWriter(..., OBJECT value)
    var value = Expression.Convert(access, typeof(object));

    // writer(packet, (object)value);
    var invoke = Expression.Invoke(
        Expression.Constant(serializer.Writer),
        Parameters.writer,
        value
    );

    return invoke;
  }

  internal static BinaryExpression Deserializer(MemberInfoWrapper member, Serializers<BinarySerializer> serializers)
  {
    var serializer = GuardGetSerializer(member.Type, serializers);
    var invoke = Expression.Invoke(
      Expression.Constant(serializer.Reader),
      Parameters.reader
    );

    var cast = Expression.Convert(Parameters.instance, member.Member.DeclaringType!);
    var target = Expression.PropertyOrField(cast, member.Member.Name);
    var assign = Expression.Assign(target, Expression.Convert(invoke, member.Type));

    return assign;
  }

  private static BinarySerializer GuardGetSerializer(Type type, Serializers<BinarySerializer> serializers)
  {
    if (serializers.TryGet(type, out var serializer))
    {
      return serializer;
    }

    throw new NotSupportedException($"No serializer defined for data type {type}");
  }

  internal static T ForDelegate<T>(IMessageHandler handler, MethodInfo method) where T : Delegate
  {
    var invoke = typeof(T).GetMethod(nameof(MethodInfo.Invoke));
    var delParams = invoke.GetParameters();
    var mtdParams = method.GetParameters();

    // Delegate parameters
    var ldaParams = delParams
      .Select(p => Expression.Parameter(p.ParameterType, p.Name))
      .ToArray();

    // Instance (handler)
    var ins = Expression.Constant(handler);

    // Method arguments
    var calArgs = new Expression[mtdParams.Length];
    for (int i = 0; i < calArgs.Length; i++)
    {
      calArgs[i] = Expression.Convert(
        ldaParams[i],
        mtdParams[i].ParameterType
      );
    }

    // ins.Method(a, b, c)
    var call = Expression.Call(ins, method, calArgs);

    // (a, b, c) => ins.Method(a, b, c)
    var lda = Expression.Lambda<T>(call, ldaParams);

    return lda.Compile();
  }

  internal static CreatorExpression GetCreator(Type type)
  {
    var @new = Expression.New(type);
    var converted = Expression.Convert(@new, typeof(IMessage));

    return Expression.Lambda<CreatorExpression>(converted).Compile();
  }
}