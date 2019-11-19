using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Tiles
{
    public abstract class BetterGlobalTile : GlobalTile
    {
        public sealed override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            Vector2 position = new Vector2(i, j);
            Player player = position.GetNearestMiningPlayer();

            if (Vector2.Distance(position, player.position / 16) > MiningLookupRange)
                return false;

            return CanAnythingKillTile(i, j, type, ref blockDamaged) && CanPlayerKillTile(player, i, j, type, ref blockDamaged);
        }

        public virtual bool CanAnythingKillTile(int i, int j, int type, ref bool blockDamaged) => true;

        public virtual bool CanPlayerKillTile(Player player, int i, int j, int type, ref bool blockDamaged) => true;


        /// <summary>The maximum range for which to trigger CanKillTile with Player. Set to int.MaxValue to not have a distance limit. Default is 25.</summary>
        public virtual int MiningLookupRange { get; } = 25;
    }
}