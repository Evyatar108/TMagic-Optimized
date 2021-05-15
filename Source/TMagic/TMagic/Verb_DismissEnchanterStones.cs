using RimWorld;
using AbilityUser;
using Verse;
using System.Linq;

namespace TorannMagic
{
    public class Verb_DismissEnchanterStones : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.enchanterStones.Count > 0)
                {
                    Thing stone = comp.enchanterStones.First();
                    stone.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("Found no enchanter stones to destroy.", MessageTypeDefOf.RejectInput);
                }
            }
            return true;
        }
    }
}
