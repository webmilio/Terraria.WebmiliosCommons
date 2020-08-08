using Terraria;
using WebmilioCommons.Commons.Players;
using WebmilioCommons.Players;
using WebmilioCommons.Systems;

namespace WebmilioCommons.Transformations
{
    public class PlayerTransformation : IPlayerSystem, IPlayerLinked
    {
        #region BadLifeRegen

        public virtual bool PreUpdateBadLifeRegen(WCPlayer wcPlayer, Player player) => true;

        public virtual void UpdateBadLifeRegen(WCPlayer wcPlayer, Player player) { }

        #endregion


        #region LifeRegen

        public virtual bool PreUpdateLifeRegen(WCPlayer wcPlayer, Player player) => true;

        public virtual void UpdateLifeRegen(WCPlayer wcPlayer, Player player) { }

        #endregion


        #region Transformation

        public virtual bool PreTransform(WCPlayer wcPlayer, Player player) => true;

        public virtual void PostTransform(WCPlayer wcPlayer, Player player) { }


        public virtual bool PreAnyTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation) => true;

        public virtual void PostAnyTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation) { }


        public virtual bool PreDeTransform(WCPlayer wcPlayer, Player player, bool death) => true;

        public virtual void PostDeTransform(WCPlayer wcPlayer, Player player, bool death) { }


        public virtual bool PreAnyDeTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation, bool death) => true;

        public virtual void PostAnyDeTransform(WCPlayer wcPlayer, Player player, PlayerTransformation transformation, bool death) { }


        public virtual bool DeTransformOnDeath(WCPlayer wcPlayer, Player player) => true;

        #endregion


        /// <summary>
        /// <c>true</c> if the only one instance of the transformation is allowed at a time per play; <c>false</c> otherwise.
        /// Defaults to <c>true</c>.
        /// </summary>
        public virtual bool Unique { get; set; } = true;

        public Player Player { get; internal set; }
    }
}