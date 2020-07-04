namespace WebmilioCommons.Items.Starting
{
    /// <summary>Implements a property which specifies how big the starting item's stack should be.</summary>
    public interface IPlayerStartsWithStack : IPlayerStartsWith
    {
        int StartStack { get; }
    }
}