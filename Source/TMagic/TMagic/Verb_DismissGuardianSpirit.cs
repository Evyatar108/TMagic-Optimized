using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DismissGuardianSpirit : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;

            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            if (comp.IsMagicUser)
            {
                if (comp.bondedSpirit != null)
                {
                    comp.bondedSpirit.SetFaction(Find.FactionManager.FirstFactionOfDef(TorannMagicDefOf.TM_SkeletalFaction), null);
                    comp.bondedSpirit.Kill(null);
                }
                else
                {
                    Messages.Message("TM_NoGuardianSpiritToDismiss".Translate(
                            this.CasterPawn.LabelShort
                        ), MessageTypeDefOf.RejectInput);
                }
            }
            return true;
        }
    }
}
