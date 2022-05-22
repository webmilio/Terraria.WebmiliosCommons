using Microsoft.Xna.Framework;
using Terraria;
using WebmilioCommons.Extensions;
using WebmilioCommons.Proxies;
using WorldGen = On.Terraria.WorldGen;

namespace WebmilioCommons.Tiles;

public abstract class GlobalTile : Terraria.ModLoader.GlobalTile
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

    public virtual void PostTreeGrow(int x, int y, Terraria.WorldGen.GrowTreeSettings? settings) { }

    /// <summary>The maximum range for which to trigger CanKillTile with Player. Set to -1 to not have a distance limit. Default is 25.</summary>
    public virtual int MiningLookupRange { get; } = 25;


    // Hooks for extra methods
    internal static void Hook()
    {
        WorldGen.GrowTree += WorldGen_OnGrowTree;
        WorldGen.GrowTreeWithSettings += WorldGen_OnGrowTreeWithSettings;
    }

    internal static void Free()
    {
        WorldGen.GrowTree -= WorldGen_OnGrowTree;
        WorldGen.GrowTreeWithSettings -= WorldGen_OnGrowTreeWithSettings;
    }

    private static bool WorldGen_OnGrowTree(WorldGen.orig_GrowTree orig, int x, int y)
    {
        if (!orig(x, y)) return false;

        GlobalTilesProxy.Do<GlobalTile>(t => t.PostTreeGrow(x, y, null));
        return true;
    }

    private static bool WorldGen_OnGrowTreeWithSettings(WorldGen.orig_GrowTreeWithSettings orig, int x, int y, Terraria.WorldGen.GrowTreeSettings settings)
    {
        if (!orig(x, y, settings)) return false;

        GlobalTilesProxy.Do<GlobalTile>(t => t.PostTreeGrow(x, y, settings));
        return true;
    }
}