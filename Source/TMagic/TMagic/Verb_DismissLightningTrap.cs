using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissLightningTrap : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.lightningTraps.Count > 0)
                {
                    Thing lightningTrap = comp.lightningTraps[0];
                    lightningTrap.Destroy();
                }
                else
                {

                }
            }
            return true;
        }
    }
}
