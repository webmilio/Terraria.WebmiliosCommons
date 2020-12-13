using Terraria.DataStructures;

namespace WebmilioCommons.TileEntities
{
    public interface IStandardTileEntity
    {
        Point16 Pos { get; }

        int Id { get; }
    }
}