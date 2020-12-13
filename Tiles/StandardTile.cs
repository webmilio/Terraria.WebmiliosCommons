using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WebmilioCommons.Commons;
using WebmilioCommons.TileEntities;

namespace WebmilioCommons.Tiles
{
    public abstract class StandardTile : ModTile
    {
        protected StandardTile()
        {
        }


        /*public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            
        }



        public override void PlaceInWorld(int i, int j, Item item)
        {
            ModPlaceInWorld(i, j, item);
        }

        public virtual void ModPlaceInWorld(int i, int j, Item item)
        {
            if (i > 0)
                CheckAndNotifyNeighbor(Directions.Right, i - 1, j, item);

            if (i < Main.maxTilesX)
                CheckAndNotifyNeighbor(Directions.Left, i + 1, j, item);

            if (j > 0)
                CheckAndNotifyNeighbor(Directions.Up, i, j - 1, item);

            if (j < Main.maxTilesY)
                CheckAndNotifyNeighbor(Directions.Up, i, j + 1, item);
        }

        private void CheckAndNotifyNeighbor(Directions side, int i, int j, Item item)
        {
            var tile = ModContent.GetModTile(Main.tile[i, j].type);

            if (tile is StandardTile std)
            {
                std.OnStandardNeighborPlaced(this, side, item, i, j);
                
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var te) && te is StandardTileEntity stdTE)
                    stdTE.OnTile_NeighborPlaced();
            }
        }


        public virtual void OnStandardNeighborPlaced(StandardTile tile, Directions side, Item item, int i, int j)
        {
            
        }*/
    }
}