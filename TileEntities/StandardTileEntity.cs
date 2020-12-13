using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Commons;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.TileEntities
{
    public abstract class StandardTileEntity : ModTileEntity, IStandardTileEntity
    {
        public const int SideCount = 4;

        public StandardTileEntity[] neighbors = new StandardTileEntity[SideCount];


        protected StandardTileEntity()
        {
        }


        public override bool ValidTile(int i, int j)
        {
            return true;
        }


        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (!Mod_AfterPlacement(i, j, type, style, direction))
                return -1;

            var te = (StandardTileEntity)ByID[Place(i, j)];
            te.InternalPlaced(i, j, type, style, direction);

            return te.ID;
        }

        public virtual bool Mod_AfterPlacement(int i, int j, int type, int style, int direction) => true;

        public sealed override void OnKill()
        {
            ForNeighbors((te, index) =>
            {
                var opposite = ((Directions)index).GetOpposite();

                te.neighbors[(int)opposite] = default;
                te.NeighborCount--;

                te.OnNeighborKilled(te, opposite);
                te.OnNeighborChange(default, opposite);
            });

            Kill();
        }


        internal virtual void InternalPlaced(int x, int y, int type, int style, int direction)
        {
            neighbors = GetNeighboringTiles(Pos, out var neighborCount);
            NeighborCount = neighborCount;

            OnPlaced(x, y, type, style, direction);

            ForNeighbors((n, i) =>
            {
                var opposite = ((Directions)i).GetOpposite();

                n.neighbors[(int)opposite] = this;
                n.NeighborCount++;

                n.OnNeighborPlaced(this, opposite, style, direction);
                n.OnNeighborChange(this, opposite);
            });
        }

        public virtual void OnPlaced(int x, int y, int type, int style, int direction) { }

        /// <summary>Called when the tile is being called. Replaces <see cref="ModTileEntity.OnKill"/>.</summary>
        public virtual void Kill() { }

        // public virtual void OnNeighborUpdated(ModTileEntity tileEntity, Directions direction, int i, int j) { }
        // public virtual void OnNeighborUpdated(StandardTileEntity tileEntity, Directions direction, int i, int j) { }

        public virtual void OnNeighborPlaced(StandardTileEntity te, Directions side, int style, int direction) { }

        public virtual void OnNeighborKilled(StandardTileEntity te, Directions side) { }

        public virtual void OnNeighborChange(StandardTileEntity changed, Directions side) { }


        public virtual bool OnAnyTilePlaced(int i, int j, int type, bool mute, bool forced, int plr, int style) => true;


        public static StandardTileEntity[] GetNeighboringTiles(Point16 position)
        {
            var x = position.X;
            var y = position.Y;

            var neighbors = new StandardTileEntity[SideCount];

            if (y > 0)
                neighbors[(int)Directions.Up] = GetOrDefault(new Point16(x, y - 1));

            if (y < Main.maxTilesY)
                neighbors[(int)Directions.Down] = GetOrDefault(new Point16(x, y + 1));

            if (x > 0)
                neighbors[(int)Directions.Left] = GetOrDefault(new Point16(x - 1, y));

            if (x < Main.maxTilesX)
                neighbors[(int)Directions.Right] = GetOrDefault(new Point16(x + 1, y));

            return neighbors;
        }

        public static StandardTileEntity[] GetNeighboringTiles(Point16 position, out int neighborCount)
        {
            neighborCount = 0;
            var n = GetNeighboringTiles(position);

            for (var i = 0; i < n.Length; i++)
                if (n[i] != default)
                    neighborCount++;

            return n;
        }

        public Directions GetSide(object tileEntity)
        {
            if (tileEntity == default)
                return Directions.Up;

            for (var i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == tileEntity)
                    return (Directions)i;
            }

            return Directions.Up;
        }


        public static StandardTileEntity GetOrDefault(Point16 position) => GetOrDefault<StandardTileEntity>(position);

        public static T GetOrDefault<T>(int id)
        {
            ByID.TryGetValue(id, out var te);

            return te is T t ? t : default;
        }

        public static T GetOrDefault<T>(Point16 position)
        {
            ByPosition.TryGetValue(position, out var te);

            return te is T t ? t : default;
        }


        /// <summary>Iterates through all known <see cref="StandardTileEntity"/> neighbors of this tile..</summary>
        /// <param name="action">The action to take the neighboring tile.</param>
        protected void ForNeighbors(Action<StandardTileEntity, int> action)
        {
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];

                if (neighbor == default)
                    continue;

                action(neighbor, i);
            }
        }

        /// <summary>Iterates through all known neighbors of this tile matching the specified type.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action to take the neighboring tile.</param>
        protected void ForNeighbors<T>(Action<T, int> action)
        {
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];

                if (!(neighbor is T t))
                    continue;

                action(t, i);
            }
        }

        protected bool ForAnyNeighbor<T>(Func<T, int, bool> action)
        {
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];

                if (!(neighbor is T t))
                    continue;

                if (action(t, i))
                    return true;
            }

            return false;
        }

        protected bool ForAllNeighbors<T>(Func<T, int, bool> action)
        {
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];

                if (!(neighbor is T t))
                    continue;

                if (!action(t, i))
                    return false;
            }

            return true;
        }


        public void PostWorldLoad()
        {
            neighbors = GetNeighboringTiles(Pos, out var neighborCount);
            NeighborCount = neighborCount;

            ModPostWorldLoad();
        }

        /// <summary>Called after the world has been loaded and the neighbors assigned.</summary>
        public virtual void ModPostWorldLoad() { }


        /// <summary>The amount of neighboring <see cref="StandardTileEntity"/> tiles.</summary>
        public int NeighborCount { get; private set; }


        public int Id => ID;

        public Point16 Pos => Position;
    }
}