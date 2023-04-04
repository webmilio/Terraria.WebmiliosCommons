using System;
using Terraria.ModLoader;

namespace WebCom.AI;

public partial class IntelligenceAction
{
    public class Delay : IntelligenceAction
    {
        public readonly int sourceTime;
        public int time;

        public Delay(int time)
        {
            sourceTime = time;
            this.time = time;
        }

        public override void Update(Context context)
        {
            --time;
            context.ContinueScope = false;
        }

        public override void Restart()
        {
            time = sourceTime;
        }

        public override bool Done => time < 0;
    }

    public class ActionWrapper : IntelligenceAction
    {
        public ActionWrapper(Action<Context> action)
        {
            Action = action;
        }

        public override void Update(Context context)
        {
            Action(context);
        }

        public Action<Context> Action { get; }
    }

    public class PredicateWrapper : IntelligenceAction
    {
        public PredicateWrapper(Predicate<Context> condition)
        {
            Condition = condition;
        }

        public override void Update(Context context)
        {
            context.ContinueScope = Condition(context);
        }

        public Predicate<Context> Condition { get; }
    }

    public class Context
    {
        public virtual void Reset()
        {
            ContinueScope = ContinueGlobal = true;
        }

        /// <summary><c>false</c> to stop the AI method's execution, for example <see cref="ModNPC.PreAI"/>; otherwise <c>true</c>.</summary>
        public bool ContinueScope { get; set; } = true;

        /// <summary>
        ///   <c>false</c> to stop all AI method's execution; otherwise <c>true</c>.
        ///   For example, returning <c>false</c> in the <see cref="ModNPC.PreAI"/> method will stop the executing of 
        ///   <see cref="ModNPC.PreAI"/>, <see cref="ModNPC.AI"/> and <see cref="ModNPC.PostAI"/>.
        /// </summary>
        public bool ContinueGlobal { get; set; } = true;
    }
}