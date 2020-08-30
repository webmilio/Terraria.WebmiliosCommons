using Terraria;
using WebmilioCommons.Commons.Players;
using WebmilioCommons.Players;
using WebmilioCommons.Systems;

namespace WebmilioCommons.Transformations
{
    public abstract class PlayerTransformation : IPlayerSystem, IPlayerLinked
    {
        #region BadLifeRegen

        public virtual bool PreUpdateBadLifeRegen() => true;

        public virtual void UpdateBadLifeRegen() { }

        #endregion

        #region LifeRegen

        public virtual bool PreUpdateLifeRegen() => true;

        public virtual void UpdateLifeRegen() { }

        #endregion


        #region Transformation

        public virtual bool PreTransform() => true;

        public virtual void PostTransform() { }


        public virtual bool PreAnyTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation) => true;

        public virtual void PostAnyTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation) { }


        public virtual bool PreDeTransform(bool death) => true;

        public virtual void PostDeTransform(bool death) { }


        public virtual bool PreAnyDeTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation, bool death) => true;

        public virtual void PostAnyDeTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation, bool death) { }


        public virtual bool DeTransformOnDeath() => true;

        #endregion


        /// <summary>
        /// <c>true</c> if the only one instance of the transformation is allowed at a time per play; <c>false</c> otherwise.
        /// Defaults to <c>true</c>.
        /// </summary>
        public virtual bool Unique { get; set; } = true;

        public Player Player { get; internal set; }
        public WCPlayer WCPlayer { get; internal set; }
    }
}