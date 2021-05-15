using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissHeater : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.summonedHeaters.Count > 0)
                {
                    Thing heater = comp.summonedHeaters[0];
                    heater.Destroy();
                }
                else
                {

                }
            }
            return true;
        }
    }
}
