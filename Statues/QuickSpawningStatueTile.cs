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
     * Some of this code (if not all of it) is from ExampleStatue from tModLoader.
     * https://github.com/tModLoader/tModLoader/blob/master/ExampleMod/Tiles/ExampleStatue.cs
    */
    public abstract class QuickSpawningStatueTile : StandardTile
    {
        public const int
            NPC_SPAWN_DELAY = 30,
            ITEM_SPAWN_DELAY = 60;


        protected QuickSpawningStatueTile(int droppedItemType) : this(droppedItemType, TileObjectData.Style2xX)
        {
        }

        protected QuickSpawningStatueTile(int droppedItemType, TileObjectData tileObjectData)
        {
            DroppedItemType = droppedItemType;
            TileObjectData = tileObjectData;
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData);
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
            spawnX += GetItemSpawnXOffset(wireHitX, wireHitY, spawnX, spawnY);
            spawnY += GetItemSpawnYOffset(wireHitX, wireHitY, spawnX, spawnY);


            if (!PreSpawnItem(wireHitX, wireHitY, spawnX, spawnY))
                return;


            int itemIndex = DoSpawnItem(wireHitX, wireHitY, spawnX, spawnY, GetSpawnedType());

            if (itemIndex < 0)
                return;


            PostSpawnItem(Main.item[itemIndex]);
        }

        public int DoSpawnItem(int wireHitX, int wireHitY, int spawnX, int spawnY, int itemType) => Item.NewItem(spawnX, spawnY, 0, 0, itemType, GetItemStack(itemType), false, GetItemPrefix(itemType), false);

        public virtual void PostSpawnItem(Item item) { }


        public virtual int GetItemSpawnXOffset(int wireHitX, int wireHitY, int spawnX, int spawnY) => 0;
        public virtual int GetItemSpawnYOffset(int wireHitX, int wireHitY, int spawnX, int spawnY) => -20;

        public virtual int GetItemStack(int itemType) => 1;
        public virtual int GetItemPrefix(int itemType) => 0;

        #endregion


        #region NPCs

        public virtual bool PreSpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY) => EntityMechSpawn(NPC.MechSpawn, spawnX, spawnY, spawnYOffset: -12);

        public virtual void SpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY)
        {
            spawnX += GetNPCSpawnXOffset(wireHitX, wireHitY, spawnX, spawnY);
            spawnY += GetNPCSpawnYOffset(wireHitX, wireHitY, spawnX, spawnY);


            if (!PreSpawnNPC(wireHitX, wireHitY, spawnX, spawnY)) 
                return;


            int npcIndex = DoSpawnNPC(wireHitX, wireHitY, spawnX, spawnY, GetSpawnedType());

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
        public virtual int DoSpawnNPC(int wireHitX, int wireHitY, int spawnX, int spawnY, int npcType) => 
            NPC.NewNPC(spawnX, spawnY, npcType);

        /// <summary>Called after <see cref="SpawnNPC"/>. Modifies the existing NPC by calling <see cref="GetNPCValue"/>, <see cref="GetNPCSlots"/> and <see cref="GetNPCSpawnedFromStatue"/>.</summary>
        /// <param name="npc"></param>
        /// <seealso cref="GetNPCValue"/>
        /// <seealso cref="GetNPCSlots"/>
        /// <seealso cref="GetNPCSpawnedFromStatue"/>
        public virtual void PostSpawnNPC(NPC npc)
        {
            npc.value = GetNPCValue(npc);
            npc.npcSlots = GetNPCSlots(npc);
            npc.SpawnedFromStatue = GetNPCSpawnedFromStatue(npc);
        }


        public virtual int GetNPCSpawnXOffset(int wireHitX, int wireHitY, int spawnX, int spawnY) => 0;
        public virtual int GetNPCSpawnYOffset(int wireHitX, int wireHitY, int spawnX, int spawnY) => -12;

        public virtual float GetNPCValue(NPC npc) => 0f;
        public virtual float GetNPCSlots(NPC npc) => 0f;
        public virtual bool GetNPCSpawnedFromStatue(NPC npc) => true;

        #endregion


        /// <summary>Place to have all your custom logic as to which entity to spawn. When not overriden, calls <see cref="GetRandomSpawnedType"/>.</summary>
        /// <returns></returns>
        /// <seealso cref="GetRandomSpawnedType"/>
        public virtual int GetSpawnedType() => GetRandomSpawnedType();


        public virtual bool PreWiringMechCheck(int wireHitX, int wireHitY, int spawnX, int spawnY) => Wiring.CheckMech(wireHitX, wireHitY, SpawnDelay);

        protected bool EntityMechSpawn(Func<float, float, int, bool> mechSpawn, int spawnX, int spawnY, int spawnXOffset = 0, int spawnYOffset = 0)
        {
            for (int i = 0; i < SpawnedTypes.Length; i++)
                if (!mechSpawn(spawnX + spawnXOffset, spawnY + spawnYOffset, SpawnedTypes[i]))
                    return false;

            return true;
        }


        /// <summary>Gets a random type from <see cref="SpawnedTypes"/>.</summary>
        /// <returns>The only element in <see cref="SpawnedTypes"/> if <see cref="SpawnedTypes"/> contains one element; otherwise a random type.</returns>
        /// <seealso cref="SpawnedTypes"/>
        protected int GetRandomSpawnedType()
        {
            int[] spawnedTypes = GetSpawnedTypesArray();
            int type = spawnedTypes.Length == 1 ? spawnedTypes[0] : Main.rand.Next(spawnedTypes);

            return type;
        }

        /// <summary>Used when you have different entity spawning chances.</summary>
        /// <returns></returns>
        protected virtual int[] GetSpawnedTypesArray() => SpawnedTypes;


        public virtual Color MapEntryColor { get; } = new Color(144, 148, 144);


        public int DroppedItemType { get; }

        public TileObjectData TileObjectData { get; }


        /// <summary>The type of entity the statue spawns. The method calls differ depending on this value.</summary>
        /// <seealso cref="SpawnItem"/>
        /// <seealso cref="SpawnNPC"/>
        public virtual StatueSpawneableEntityType Spawns { get; } = StatueSpawneableEntityType.NPC;


        /// <summary>The cooldown between each spawn for a given statue.</summary>
        public virtual int SpawnDelay { get; } = NPC_SPAWN_DELAY;
        

        /// <summary>Used in checks for max statue-spawned entities and default random implementation.</summary>
        public abstract int [] SpawnedTypes { get; }
    }
}