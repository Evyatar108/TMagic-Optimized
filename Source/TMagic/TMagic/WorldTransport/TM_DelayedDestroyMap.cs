using RimWorld.Planet;
using Verse;

namespace TorannMagic.WorldTransport
{
    public class TM_DelayedDestroyMap : WorldObjectComp
    {
        public Settlement parent;

        public int delayTicks = 100;
        private int age = 0;

        public override void CompTick()
        {
            base.CompTick();
            age++;
            if (age >= delayTicks && parent.Map != null)
            {
                this.parent.AllComps.Remove(this);
                Current.Game.DeinitAndRemoveMap(parent.Map);
            }
        }

    }
}
