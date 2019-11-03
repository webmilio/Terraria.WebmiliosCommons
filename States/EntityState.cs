using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.States
{
    public abstract class EntityState<T> : State where T : Entity
    {
        protected EntityState(T entity) : base(entity)
        {
            Entity = entity;
            WhoAmI = entity.whoAmI;

            Position = entity.position;
            Velocity = entity.velocity;
        }


        public virtual void Restore()
        {
            Entity.position = Position;
            Entity.velocity = Velocity;
        }

        public virtual void PreAI(T entity)
        {
            entity.position = Position;
            entity.velocity = Vector2.Zero;
        }


        public T Entity { get; set; }
        public int WhoAmI { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }


        public int AccumulatedDamage { get; set; }
        public float AccumulatedKnockback { get; set; }
        public int AccumulatedHitDirection { get; set; }
    }
}