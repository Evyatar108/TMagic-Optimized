using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_Hex_CriticalFail : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn pawn = this.currentTarget.Thing as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            int verVal = TM_Calc.GetMagicSkillLevel(CasterPawn, comp.MagicData.MagicPowerSkill_Hex, "TM_Hex", "_ver", true);
            var hexedPawns = comp.HexedPawns;
            if (comp != null && hexedPawns.Count > 0)
            {
                foreach (Pawn p in hexedPawns)
                {
                    HealthUtility.AdjustSeverity(p, TorannMagicDefOf.TM_Hex_CriticalFailHD, .6f + (.1f * verVal));
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_BlackSmoke, p.DrawPos, p.Map, .7f, .1f, .1f, .2f, Rand.Range(-50, 50), Rand.Range(.5f, 1f), Rand.Range(-90, 90), Rand.Range(0, 360));
                }
            }
            return true;
        }
    }
}
