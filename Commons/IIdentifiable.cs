namespace WebmilioCommons.Commons;

public interface IIdentifiable<out T>
{
    public T Identifier { get; }
}