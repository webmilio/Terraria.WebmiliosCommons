using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using WebmilioCommons.Entities;
using WebmilioCommons.Tiles;

namespace WebmilioCommons.Statues
{
    /*
     * Most of this code (if not all of it) is from ExampleStatue from tModLoader.
     * https://github.com/tModLoader/tModLoader/blob/master/ExampleMod/Tiles/ExampleStatue.cs
    */
    public abstract class QuickSpawningStatueTile : StandardTile
    {
        public const int
            NPC_SPAWN_DELAY = 30,
            ITEM_SPAWN_DELAY = 60;


        protected QuickSpawningStatueTile(int droppedItemType)
        {
            DroppedItemType = droppedItemType;
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.addTile(Type);

            AddMapEntry(MapEntryColor, GetMapEntryName());
            dustType = 11;

            disableSmartCursor = true;
        }


        public virtual ModTranslation GetMapEntryName()
        {
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Statue");

            return name;
        }


        public override void KillMultiTile(int i, int j, int frameX, int frameY) =>
            Item.NewItem(i * 16, j * 16, 32, 48, DroppedItemType);


        public override void HitWire(int wireHitX, int wireHitY)
        {
            int
                x = wireHitX - Main.tile[wireHitX, wireHitY].frameX / 18,
                y = wireHitY - Main.tile[wireHitX, wireHitY].frameY / 18;


            for (int i = 0; i <= 1; i++)
                for (int j = 0; j <= 2; j++)
                    Wiring.SkipWire(x + i, y + j);


            int 
                spawnX = x * 16 + 16,
                spawnY = (y + 3) * 16;


            if (!PreWiringMechCheck(wireHitX, wireHitY, spawnX, spawnY))
                return;


            switch (Spawns)
            {
                case StatueSpawneableEntityType.NPC:
                    SpawnNPC(wireHitX, wireHitY, spawnX, spawnY);
                    break;

                case StatueSpawneableEntityType.Item:
                    SpawnItem(wireHitX, wireHitY, spawnX, spawnY);
                    break;
                default:
                    throw new NotImplementedException($"{nameof(QuickSpawningStatueTile)} does not support {StatueSpawneableEntityType.Projectile} yet.");
            }
        }


        #region Items

        public virtual bool PreSpawnItem(int wireHitX, int wireHitY, int spawnX, int spawnY) => EntityMechSpawn(Item.MechSpawn, spawnX, spawnY);

        public virtual void SpawnItem(int wireHitX, int wireHitY, int spawnX, int spawnY)
        {
            if (!PreSpawnItem(wireHitX, wireHitY, spawnX, spawnY))
                return;


            int itemIndex = DoSpawnItem(wireHitX, wireHitY, spawnX, spawnY, GetRandomSpawnedType());

            if (itemIndex < 0)
                return;


            PostSpawnItem(Main.item[itemIndex]);
        }

        public int DoSpawnItem(int wireHitX, int wireHitY, int spawnX, int spawnY, int itemType) => Item.NewItem(spawnX, spawnY - 20, 0, 0, itemType, GetItemStack(itemType), false, GetItemPrefix(itemType), false);

        public virtual void PostSpawnItem(Item item) { }


        public virtual int GetItemStack(int itemType) => 1;
        public virtual int GetItemPrefix(int itemType) => 0;

        #endregion


        #region NPCs

        public virtual bool PreSpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY) => EntityMechSpawn(NPC.MechSpawn, spawnX, spawnY, -12);

        public virtual void SpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY)
        {
            if (!PreSpawnNPC(wireHitX, wireHitY, spawnX, spawnY)) 
                return;


            int npcIndex = DoSpawnNPC(wireHitX, wireHitY, spawnX, spawnY, GetRandomSpawnedType());

            if (npcIndex < 0)
                return;


            PostSpawnNPC(Main.npc[npcIndex]);
        }

        /// <summary></summary>
        /// <param name="wireHitX"></param>
        /// <param name="wireHitY"></param>
        /// <param name="spawnX"></param>
        /// <param name="spawnY"></param>
        /// <param name="npcType"></param>
        /// <returns>-1 to stop the execution of <see cref="SpawnNPC"/>; otherwise the NPC index in the world.</returns>
        public virtual int DoSpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY, int npcType) => NPC.NewNPC(spawnX, spawnY - 12, npcType);

        public virtual void PostSpawnNPC(NPC npc)
        {
            npc.value = GetNPCValue(npc);
            npc.npcSlots = GetNPCSlots(npc);
            npc.SpawnedFromStatue = GetNPCSpawnedFromStatue(npc);
        }


        public virtual float GetNPCValue(NPC npc) => 0f;
        public virtual float GetNPCSlots(NPC npc) => 0f;
        public virtual bool GetNPCSpawnedFromStatue(NPC npc) => true;

        #endregion


        public virtual bool PreWiringMechCheck(int wireHitX, int wireHitY, int spawnX, int spawnY) => Wiring.CheckMech(wireHitX, wireHitY, SpawnDelay);

        protected bool EntityMechSpawn(Func<float, float, int, bool> mechSpawn, int spawnX, int spawnY, int spawnYOffset = 0)
        {
            for (int i = 0; i < SpawnedTypes.Length; i++)
                if (!mechSpawn(spawnX, spawnY + spawnYOffset, SpawnedTypes[i]))
                    return false;

            return true;
        }


        protected int GetRandomSpawnedType() => SpawnedTypes.Length == 1 ? SpawnedTypes[0] : Main.rand.Next(SpawnedTypes);


        public virtual Color MapEntryColor { get; } = new Color(144, 148, 144);


        public int DroppedItemType { get; }


        public virtual StatueSpawneableEntityType Spawns { get; } = StatueSpawneableEntityType.NPC;

        public virtual int SpawnDelay { get; } = NPC_SPAWN_DELAY;

        public abstract int[] SpawnedTypes { get; }
    }
}