using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissCooler : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.summonedCoolers.Count > 0)
                {
                    Thing cooler = comp.summonedCoolers[0];
                    cooler.Destroy();
                }
                else
                {

                }
            }
            return true;
        }
    }
}
