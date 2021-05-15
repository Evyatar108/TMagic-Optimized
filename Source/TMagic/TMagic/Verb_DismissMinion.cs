using RimWorld;
using AbilityUser;
using Verse;
using System.Linq;

namespace TorannMagic
{
    public class Verb_DismissMinion : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.summonedMinions.Count > 0)
                {
                    TMPawnSummoned minion = (TMPawnSummoned)comp.summonedMinions.First();
                    minion.TicksLeft = 1;
                }
                else
                {
                    Messages.Message("TM_NoMinionToDismiss".Translate(
                            this.CasterPawn.LabelShort
                        ), MessageTypeDefOf.RejectInput);
                }
            }
            return true;
        }
    }
}
