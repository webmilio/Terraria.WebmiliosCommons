using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ModLoader;

namespace WebCom.AI;

public partial class IntelligenceBuilder
{
    public class Entity<TEntity> : IIntelligenceAction
    {
        // Only exposed for early development stages.
        public readonly List<IIntelligenceAction> actions = new();
        public int actionIndex;

        public Entity<TEntity> Action(Action action, string name = null) => Action(_ => action(), name);
        public Entity<TEntity> Condition(Func<bool> condition, string name = null) => Condition(_ => condition(), name);

        public Entity<TEntity> Action(Action<IntelligenceAction.Context> action, string name = null)
        {
            AddAction(new IntelligenceAction.ActionWrapper(action)
            {
                Name = name
            });
            return this;
        }

        public Entity<TEntity> Condition(Predicate<IntelligenceAction.Context> condition, string name = null)
        {
            AddAction(new IntelligenceAction.PredicateWrapper(condition)
            {
                Name = name
            });
            return this;
        }

        public Entity<TEntity> Sub()
        {
            var sub = CreateNew();

            AddAction(sub);
            return sub;
        }

        protected virtual void AddAction(IIntelligenceAction action)
        {
            actions.Add(action);
        }

        protected Entity<TEntity> CreateNew()
        {
            return new Entity<TEntity>();
        }

        public virtual void Finish() { }

        public virtual void Update(IntelligenceAction.Context context)
        {
            actions[actionIndex].Update(context);
            ++actionIndex;
        }
    }

    /// <summary></summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class EntityBuilderCollection<T>
    {
        private readonly Entity<T> preAI, ai, postAI;

        public EntityBuilderCollection(IIntelligenceGlobal<T> global)
        {
            Global = global;

            preAI = new();
            ai = new();
            postAI = new();
        }

        public Entity<T> PreAI() => preAI;
        public Entity<T> AI() => ai;
        public Entity<T> PostAI() => preAI;

        public void Finish()
        {
            Global.Intelligence = new EntityBuilderProvider<T>(new[] { preAI, ai, postAI });
        }

        internal IIntelligenceGlobal<T> Global { get; set; }
    }

    public class EntityBuilderProvider<T>
    {
        private readonly Entity<T>[] ai; // [0] : PreAI, [1]: AI, [2]: PostAI
        private readonly IntelligenceAction.Context context = new();

        public EntityBuilderProvider(Entity<T>[] ai)
        {
            this.ai = ai;
        }

        public bool PreAI()
        {
            context.Reset();

            ai[0].Update(context);
            return context.ContinueGlobal;
        }

        public void AI()
        {
            ai[1].Update(context);
        }

        public void PostAI()
        {
            ai[2].Update(context);
        }
    }
}