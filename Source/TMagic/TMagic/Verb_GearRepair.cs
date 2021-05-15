using System.Collections.Generic;
using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_GearRepair : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;

            bool flag = caster != null && !caster.Dead;
            if (flag)
            {
                if (caster.health.hediffSet.HasHediff(HediffDef.Named("TM_HediffGearRepair")))
                {
                    using (IEnumerator<Hediff> enumerator = caster.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (rec.def.defName.Contains("TM_HediffGearRepair"))
                            {
                                caster.health.RemoveHediff(rec);
                            }
                        }
                    }
                }
                else
                {
                    HealthUtility.AdjustSeverity(caster, HediffDef.Named("TM_HediffGearRepair"), .5f);
                    MoteMaker.ThrowDustPuff(caster.Position, caster.Map, 1f);
                }
            }
            return true;
        }
    }
}
