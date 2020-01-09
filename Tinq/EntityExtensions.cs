using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Tinq
{
    public static class EntityExtensions
    {
        #region TINQ

        private static bool Active(this Entity entity, int ignoreWhoAmI) => Active(entity) && !IsDummyEntity(entity, ignoreWhoAmI);

        public static bool Active(this Entity entity) => entity != null && entity.active && !IsDummyEntity(entity);

        public static List<T> Active<T>(this IEnumerable<T> entities) where T : Entity
        {
            List<T> activeEntities = new List<T>();

            int dummyIndex = GetDummyIndex(entities);

            void Filter(T entity)
            {
                if (Active(entity, dummyIndex))
                    activeEntities.Add(entity);
            }

            if (entities is List<T> l)
                l.ForEach(Filter);
            else
                foreach (T entity in entities)
                    Filter(entity);

            return activeEntities;
        }


        public static bool AllActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            int dummyIndex = GetDummyIndex(entities);

            foreach (T entity in entities)
                if (Active(entity, dummyIndex) && !predicate(entity))
                    return false;

            return true;
        }

        public static bool AnyActive<T>(this IEnumerable<T> entities) where T : Entity => entities.GetEnumerator().Current != null;
        public static bool AnyActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity => FirstActiveOrDefault(entities, predicate) != default;


        public static int CountActive<T>(this IEnumerable<T> entities) where T : Entity
        {
            int count = 0;

            DoActive(entities, t => count++);

            return count;
        }

        public static int CountActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            int count = 0;

            DoActive(entities, t =>
            {
                if (predicate(t))
                    count++;
            });

            return count;
        }


        public static void Do<T>(this IEnumerable<T> source, Action<T> action) where T : Entity
        {
            if (source is List<T> l)
                l.ForEach(action);
            else
                foreach (T t in source)
                    action(t);
        }

        public static void DoActive<T>(this IEnumerable<T> entities, Action<T> action) where T : Entity => entities.Active().Do(action);


        /// <summary>Generates a </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static List<T> ExceptActive<T>(this IEnumerable<T> first, IEnumerable<T> second) where T : Entity
        {
            List<T>
                activeFirst = first.Active(),
                activeSecond = second.Active(),
                except = new List<T>();

            except.AddRange(activeFirst);
            except.AddRange(activeSecond);

            except.RemoveAll(IsDummyEntity);

            for (int i = 0; i < activeSecond.Count; i++)
                if (activeFirst.Contains(activeSecond[i]))
                    except.Remove(activeSecond[i]);

            return except;
        }


        public static T FirstActive<T>(this IEnumerable<T> entities) where T : Entity => FirstActive(entities, t => true);
        public static T FirstActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (Active(entity) && predicate(entity))
                    return entity;

            throw new InvalidOperationException($"No element satisfies the condition in {nameof(predicate)}.");
        }


        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities) where T : Entity => FirstActiveOrDefault(entities, t => true);
        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            int dummyIndex = GetDummyIndex(entities);

            foreach (T entity in entities)
                if (Active(entity, dummyIndex) && predicate(entity))
                    return entity;

            return default;
        }


        public static T NearestActive<T>(this IEnumerable<T> entities, Vector2 position, int divider = 16) where T : Entity
        {
            T nearestPlayer = default;
            float nearestDistance = float.MaxValue;

            entities.DoActive(player =>
            {
                float distance = Vector2.Distance(position, player.position / divider);

                if (distance < nearestDistance)
                {
                    nearestPlayer = player;
                    nearestDistance = distance;
                }
            });

            return nearestPlayer;
        }


        public static List<T> WhereActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            List<T> result = new List<T>();

            DoActive(entities, t =>
            {
                if (predicate(t))
                    result.Add(t);
            });

            return result;
        }


        public static int GetDummyIndex<T>(this IEnumerable<T> enumerable) where T : Entity
        {
            switch (enumerable)
            {
                case Player[] playerArray when playerArray == Main.player:
                    return Main.maxPlayers;
                case NPC[] npcArray when npcArray == Main.npc:
                    return Main.maxNPCs;
                case Projectile[] projectileArray when projectileArray == Main.projectile:
                    return Main.maxProjectiles;
                case Item[] itemArray when itemArray == Main.item:
                    return Main.maxItems;
                default:
                    return -1;
            }
        }

        public static int GetDummyIndex<T>(T entity) where T : Entity
        { 
            switch (entity)
            {
                case Player player:
                    return Main.maxPlayers;
                case NPC npc:
                    return Main.maxNPCs;
                case Projectile projectile:
                    return Main.maxProjectiles;
                case Item item:
                    return Main.maxItems;
                default:
                    return -1;
            };
        }


        public static bool IsDummyEntity(this Entity entity) => IsDummyEntity(entity, GetDummyIndex(entity));

        private static bool IsDummyEntity(Entity entity, int dummyIndex) => entity.whoAmI == dummyIndex;

        #endregion
    }
}