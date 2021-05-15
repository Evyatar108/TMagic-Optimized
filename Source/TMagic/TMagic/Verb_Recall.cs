using Verse;
using AbilityUser;

namespace TorannMagic
{
    class Verb_Recall : Verb_UseAbility
    {
        CompAbilityUserMagic comp;
        Map map;

        protected override bool TryCastShot()
        {
            bool result = false;
            map = this.CasterPawn.Map;
            comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();

            if (this.CasterPawn != null && !this.CasterPawn.Downed && comp != null && comp.recallSet)
            {
                TM_Action.DoRecall(this.CasterPawn, comp, false);
            }
            else
            {
                Log.Warning("failed to TryCastShot");
            }

            this.burstShotsLeft = 0;
            return result;
        }


    }
}
