using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebCom.Annotations;
using WebCom.Extensions;
using WebCom.Net.v2.Serialization;
using WebCom.Reflection;
using WebCom.Serializers;
using static WebCom.Net.v2.MessageHandler;

namespace WebCom.Net.v2;

public static class MessageHandlerHelpers
{
    internal static CreatorExpression GetCreator(Type type)
    {
        var @new = Expression.New(type);
        var converted = Expression.Convert(@new, typeof(IMessage));

        return Expression.Lambda<CreatorExpression>(converted).Compile();
    }

    internal static (ReaderExpression reader, WriterExpression writer) CompileExpressions(Type type, Serializers<BinarySerializer> serializers)
    {
        var (reader, writer) = MapExpressions(type, serializers);
        return (reader.Compile(), writer.Compile());
    }

    internal static (Expression<ReaderExpression> reader, Expression<WriterExpression> writer) MapExpressions(Type type, Serializers<BinarySerializer> serializers)
    {
        var (writers, readers) = MapMemberExpressions(type, serializers);

        var blockReader = Expression.Block(readers.Select(r => Expression.Invoke(r, paramReader, paramInstance)));
        var reader = Expression.Lambda<ReaderExpression>(blockReader, paramReader, paramInstance);

        var blockWriter = Expression.Block(writers.Select(w => Expression.Invoke(w, paramWriter, paramInstance)));
        var writer = Expression.Lambda<WriterExpression>(blockWriter, paramWriter, paramInstance);

        return (reader, writer);
    }

    internal static (IList<Expression<WriterExpression>> writers, IList<Expression<ReaderExpression>> readers) MapMemberExpressions(Type type, Serializers<BinarySerializer> serializers)
    {
        var members = 
            type.GetDataMembers(
                fieldFlags: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField,
                propertyFlags: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty
            )
            .Where(member => 
                serializers.Has(member.Type) && 
                !member.Member.HasCustomAttribute<SkipAttribute>()
            )
            .ToArray();

        var writers = new List<Expression<WriterExpression>>(members.Length);
        var readers = new List<Expression<ReaderExpression>>(members.Length);

        foreach (var member in members)
        {
            writers.Add(MapMemberWriteToStream(member, serializers));
            readers.Add(MapMemberReadFromStream(member, serializers));
        }

        return (writers, readers);
    }

    private static BinarySerializer GuardGetSerializer(Type type, Serializers<BinarySerializer> serializers)
    {
        if (serializers.TryGet(type, out var serializer))
        {
            return serializer;
        }

        throw new NotSupportedException($"No serializer defined for data type {type}");
    }

    internal static Expression<ReaderExpression> MapMemberReadFromStream(MemberInfoWrapper member, Serializers<BinarySerializer> serializers)
    {
        var serializer = GuardGetSerializer(member.Type, serializers);

        var invoke = Expression.Invoke(
            Expression.Constant(serializer.Reader),
            paramReader
        );

        var boxedInstance = Expression.Convert(paramInstance, member.Member.DeclaringType!);

        var target = Expression.PropertyOrField(boxedInstance, member.Member.Name);
        var assign = Expression.Assign(target, Expression.Convert(invoke, member.Type));

        var lambda = Expression.Lambda<ReaderExpression>(assign, paramReader, paramInstance);
        return lambda;
    }

    internal static Expression<WriterExpression> MapMemberWriteToStream(MemberInfoWrapper member, Serializers<BinarySerializer> serializers)
    {
        var writer = GuardGetSerializer(member.Type, serializers).Writer;

        // ((T) instance)
        var boxedInstance = Expression.Convert(paramInstance, member.Member.DeclaringType!);

        // instance.Property
        var access = Expression.PropertyOrField(boxedInstance, member.Member.Name);

        // Property.Type -> object, since BinaryWriter(..., OBJECT value)
        var value = Expression.Convert(access, typeof(object));

        // writer(packet, (object)value);
        var invoke = Expression.Invoke(
            Expression.Constant(writer),
            paramWriter,
            value
        );

        var lambda = Expression.Lambda<WriterExpression>(
            invoke,
            paramWriter,
            paramInstance
         );

        return lambda;
    }
}
