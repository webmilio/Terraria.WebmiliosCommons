using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Tiles
{
    [Obsolete("Work-in-progress, might not behave as expected. Feedback is appreciated.")]
    public abstract class BetterGlobalTile : GlobalTile
    {
        public sealed override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            bool originalResult = CanAnythingKillTile(i, j, type, ref blockDamaged);

            Vector2 position = new Vector2(i, j);
            Player player = position.GetNearestMiningPlayer();

            if (MiningLookupRange < 0 || Vector2.Distance(position, player.position / 16) > MiningLookupRange)
                return originalResult;

            return originalResult && CanAnythingKillTile(i, j, type, ref blockDamaged) && CanPlayerKillTile(player, i, j, type, ref blockDamaged);
        }


        public virtual bool CanAnythingKillTile(int i, int j, int type, ref bool blockDamaged) => true;

        public virtual bool CanPlayerKillTile(Player player, int i, int j, int type, ref bool blockDamaged) => true;


        /// <summary>The maximum range for which to trigger CanKillTile with Player. Set to -1 to not have a distance limit. Default is 25.</summary>
        public virtual int MiningLookupRange { get; } = 25;
    }
}