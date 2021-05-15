using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissSunlight : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.summonedLights.Count > 0)
                {
                    Thing sunlight = comp.summonedLights[0];
                    sunlight.Destroy();
                }
                else
                {

                }
            }
            return true;
        }
    }
}
