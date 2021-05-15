using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissPowerNode : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.summonedPowerNodes.Count > 0)
                {
                    Thing powernode = comp.summonedPowerNodes[0];
                    powernode.Destroy();
                }
                else
                {

                }
            }
            return true;
        }
    }
}
