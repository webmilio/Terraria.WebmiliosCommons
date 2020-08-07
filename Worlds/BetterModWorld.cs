using Terraria.ModLoader;
using WebmilioCommons.Hooks.Wiring;

namespace WebmilioCommons.Worlds
{
    public abstract class BetterModWorld : ModWorld
    {
        #region Wiring

        public virtual bool PrePlaceWire(WireColor color, int i, int j, ref bool giveWireBack) => true;
        public virtual void PostPlaceWire(WireColor color, int i, int j) { }


        public virtual bool PreKillWire(WireColor color, int i, int j) => true;
        public virtual void PostKillWire(WireColor color, int i, int j) { }

        #endregion


        #region Trees

        public virtual bool PreAddTrees() => true;
        public virtual void PostAddTrees() { }


        public virtual bool PreGrowTree(int i, int j) => true;
        public virtual void PostGrowTree(int i, int j) { }

        #endregion
    }
}