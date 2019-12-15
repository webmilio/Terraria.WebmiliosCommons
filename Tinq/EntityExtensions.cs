using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Tinq
{
    public static class EntityExtensions
    {
        #region TINQ

        public static List<T> Active<T>(this IEnumerable<T> entities) where T : Entity
        {
            List<T> activeEntities = new List<T>();

            foreach (T entity in entities)
                if (entity != null && entity.active)
                    activeEntities.Add(entity);

            return activeEntities;
        }


        public static bool AllActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (entity != null && entity.active)
                    if (!predicate(entity))
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


        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T t in source)
                action(t);
        }

        public static void DoActive<T>(this IEnumerable<T> entities, Action<T> action) where T : Entity => entities.Active().Do(action);


        public static IEnumerable<T> ExceptActive<T>(this IEnumerable<T> first, IEnumerable<T> second) where T : Entity
        {
            List<T>
                activeFirst = first.Active(),
                activeSecond = second.Active(),
                except = new List<T>();

            except.AddRange(activeFirst);
            except.AddRange(activeSecond);

            for (int i = 0; i < activeSecond.Count; i++)
                if (activeFirst.Contains(activeSecond[i]))
                    except.Remove(activeSecond[i]);

            return except;
        }


        public static T FirstActive<T>(this IEnumerable<T> entities) where T : Entity => FirstActive(entities, t => true);
        public static T FirstActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (entity != null && entity.active)
                    if (predicate(entity))
                        return entity;

            throw new InvalidOperationException($"No element satisfies the condition in {nameof(predicate)}.");
        }


        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities) where T : Entity => FirstActiveOrDefault(entities, t => true);
        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (entity != null && entity.active)
                    if (predicate(entity))
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

        #endregion
    }
}