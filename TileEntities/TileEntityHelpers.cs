using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.TileEntities
{
    public static class TileEntityHelpers
    {
        private static int[]
            _nearestX = { -1, 1, 0, 0 },
            _nearestY = { 0, 0, -1, 1 };


        public static bool All<T>(Func<T, bool> predicate) where T : TileEntity
        {
            foreach (var te in TileEntity.ByID.Values)
                if (te is T t && !predicate(t))
                    return false;

            return true;
        }

        public static bool Any<T>(Func<T, bool> predicate) where T : TileEntity
        {
            foreach (var te in TileEntity.ByID.Values)
                if (te is T t && predicate(t))
                    return true;

            return false;
        }


        public static int Count<T>() where T : TileEntity => Count<T>(te => true);

        public static int Count<T>(Func<T, bool> predicate) where T : TileEntity
        {
            int count = 0;

            foreach (var te in TileEntity.ByID.Values)
                if (te is T t && predicate(t))
                    count++;

            return count;
        }


        public static void Do<T>(Action<T> action)
        {
            foreach (var te in TileEntity.ByID.Values)
                if (te is T t)
                    action(t);
        }


        public static T First<T>() where T : TileEntity => First<T>(te => true);

        public static T First<T>(Func<T, bool> predicate) where T : TileEntity
        {
            foreach (var te in TileEntity.ByID.Values)
                if (te is T t && predicate(t))
                    return t;

            throw new InvalidOperationException($"No tile-entity found for type {typeof(T).Name} that satisfies {nameof(predicate)}.");
        }


        public static T FirstOrDefault<T>() where T : TileEntity => FirstOrDefault<T>(te => true);

        public static T FirstOrDefault<T>(Func<T, bool> predicate) where T : TileEntity
        {
            foreach (var te in TileEntity.ByID.Values)
                if (te is T t && predicate(t))
                    return t;

            return default;
        }


        public static T Nearest<T>(Point16 position, int radius) where T : TileEntity => Nearest<T>(position, radius, out _, te => true);
        public static T Nearest<T>(Point16 position, int radius, out double dist) where T : TileEntity => Nearest<T>(position, radius, out dist, te => true);
        public static T Nearest<T>(Point16 position, int radius, Func<T, bool> predicate) where T : TileEntity => Nearest<T>(position, radius, out _, predicate);

        /// <summary>Searches for the nearest tile entity in a radius that fits the predicate and type.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="dist"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T Nearest<T>(Point16 position, int radius, out double dist, Func<T, bool> predicate) where T : TileEntity
        {
            dist = -1;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    var current = new Point16(position.X + i, position.Y + j);

                    if (current == position)
                        continue;

                    if (TileEntity.ByPosition.TryGetValue(current, out var te) && te is T t && predicate(t))
                    {
                        dist = Math.Sqrt(Math.Pow(current.X - position.X, 2) + Math.Pow(current.Y - position.Y, 2));
                        return t;
                    }
                }
            }

            return default;
        }


        public static T Single<T>() where T : TileEntity => Single<T>(te => true);

        public static T Single<T>(Func<T, bool> predicate) where T : TileEntity
        {
            T found = SingleOrDefault<T>(predicate);

            if (found == default)
                throw new InvalidOperationException($"No tile-entity satisfies the condition in {nameof(predicate)}.");

            return found;
        }


        public static T SingleOrDefault<T>() where T : TileEntity => SingleOrDefault<T>(te => true);

        public static T SingleOrDefault<T>(Func<T, bool> predicate) where T : TileEntity
        {
            T found = default;

            foreach (TileEntity te in TileEntity.ByID.Values)
            {
                if (te is T t && predicate(t))
                {
                    if (found != default)
                        throw new InvalidOperationException($"More than one element satisfies the condition in {nameof(predicate)}.");

                    found = t;
                }
            }

            return found;
        }


        public static List<T> Where<T>(Func<T, bool> predicate) where T : TileEntity
        {
            List<T> result = new List<T>(TileEntity.ByID.Count);

            Do<T>(te =>
            {
                if (predicate(te))
                    result.Add(te);
            });

            return result;
        }
    }
}
