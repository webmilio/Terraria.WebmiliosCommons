using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Tinq
{
    public static class EntityExtensions
    {
        #region TINQ

        /// <summary>Checks if the entity is considered active.</summary>
        /// <param name="entity">The entity to verify.</param>
        /// <returns><c>true</c> if the entity is not null and <see cref="Entity.active"/> is <c>true</c>; otherwise <c>false</c>.</returns>
        public static bool Active(this Entity entity) => entity != null && entity.active;

        /// <summary>Filters all entities by their active state.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns>The filtered entities.</returns>
        public static List<T> Active<T>(this IEnumerable<T> entities) where T : Entity
        {
            List<T> activeEntities = new List<T>();

            void Filter(T entity)
            {
                if (Active(entity))
                    activeEntities.Add(entity);
            }

            if (entities is List<T> l)
                l.ForEach(Filter);
            else
                foreach (T entity in entities)
                    Filter(entity);

            return activeEntities;
        }


        /// <summary>Determines whether all entities of a sequence are active and satisfy a condition.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">An <see cref="IEnumerable{T}"/> that contains the entities to check for active and apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if every entity of the source sequence is active and passes the test in the specified predicate, or if the sequence is empty; otherwise, <c>false</c>.</returns>
        public static bool AllActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (Active(entity) && !predicate(entity))
                    return false;

            return true;
        }


        /// <summary>Determines whether a sequence contains any active entity.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <returns><c>true</c> if the sequence contains any active entities; otherwise, <c>false</c>.</returns>
        public static bool AnyActive<T>(this IEnumerable<T> entities) where T : Entity => entities.CountActive() > 0;

        /// <summary>Determines whether any entity of a sequence is active and satisfies a condition.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">An <see cref="IEnumerable{T}"/> that contains the entities to check for active and apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public static bool AnyActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity => FirstActiveOrDefault(entities, predicate) != default;


        /// <summary>Returns the number of elements in a sequence.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">A sequence that contains active elements to be counted.</param>
        /// <returns></returns>
        public static int CountActive<T>(this IEnumerable<T> entities) where T : Entity
        {
            int count = 0;

            DoActive(entities, t => count++);

            return count;
        }

        /// <summary>Returns a number that represents how many elements in the specified sequence are active and satisfy a condition.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">A sequence that contains active elements to be tested and counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
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


        /// <summary>Executes a provided action on a sequence of elements. If the provided sequence implements <see cref="IList{T}"/>, the iteration is done through a <c>for</c>, otherwise it is done through a <c>foreach</c>.</summary>
        /// <typeparam name="T">The entity type of <paramref name="source"/>.</typeparam>
        /// <param name="source"></param>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is IList<T> l)
            {
                for (int i = 0; i < l.Count; i++)
                    action(l[i]);
            }
            else
                foreach (T t in source)
                    action(t);
        }


        /// <summary>Executes a provided action on a sequence of entities. The sequence is filtered by active before the action is executed.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <param name="action">The action to execute on each active element of the sequence.</param>
        public static void DoActive<T>(this IEnumerable<T> entities, Action<T> action) where T : Entity => entities.Active().Do(action);


        /// <summary>Generates a list of active entities that are exclusive to one another. Note: I'm not really sure if this does what its supposed to.</summary>
        /// <typeparam name="T">The entity type of <paramref name="first"/> and <paramref name="second"/>.</typeparam>
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

            for (int i = 0; i < activeSecond.Count; i++)
                if (activeFirst.Contains(activeSecond[i]))
                    except.Remove(activeSecond[i]);

            return except;
        }


        /// <summary>Returns the first active entity of a sequence.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static T FirstActive<T>(this IEnumerable<T> entities) where T : Entity => FirstActive(entities, t => true);

        /// <summary>Returns the first active entity in a sequence that satisfies a specified condition.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The first active entity in the sequence that passes the test in the specified predicate function.</returns>
        public static T FirstActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (Active(entity) && predicate(entity))
                    return entity;

            throw new InvalidOperationException($"No element satisfies the condition in {nameof(predicate)}.");
        }


        /// <summary>Returns the first active entity of a sequence, or a default value if the sequence contains no elements.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <returns><c>default(T)</c> if <paramref name="entities"/> is empty; otherwise, the first active entity in <paramref name="entities"/>.</returns>
        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities) where T : Entity => FirstActiveOrDefault(entities, t => true);

        /// <summary>Returns the first active entity of the sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="predicate"></param>
        /// <returns><c>default(T)</c> if <paramref name="entities"/> is empty or if no entity is active and passes the test specified by <paramref name="predicate"/>; otherwise, the first active entity in <paramref name="entities"/> that passes the test specified by <paramref name="predicate"/>.</returns>
        public static T FirstActiveOrDefault<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            foreach (T entity in entities)
                if (Active(entity) && predicate(entity))
                    return entity;

            return default;
        }


        /// <summary></summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <param name="position"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
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


        /// <summary></summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T SingleActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T: Entity
        {
            T found = SingleActiveOrDefault(entities, predicate);

            if (found == default)
                throw new InvalidOperationException("No element satisfies the condition in predicate.");

            return found;
        }

        /// <summary></summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T SingleActiveOrDefault<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            T found = default;

            foreach (T entity in entities)
                if (Active(entity) && predicate(entity))
                {
                    if (found != default)
                        throw new InvalidOperationException("More than one element satisfies the condition in predicate.");

                    found = entity;
                }

            return found;
        }


        /// <summary></summary>
        /// <typeparam name="T">The entity type of <paramref name="entities"/>.</typeparam>
        /// <param name="entities"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<T> WhereActive<T>(this IEnumerable<T> entities, Func<T, bool> predicate) where T : Entity
        {
            var result = new List<T>();

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