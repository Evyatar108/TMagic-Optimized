﻿using RimWorld;
using Verse;
using AbilityUser;

namespace TorannMagic
{
    public class Verb_SoL_Equalize : Verb_UseAbility
    {

        protected override bool TryCastShot()
        {
            Pawn pawn = this.CasterPawn;
            if (pawn != null && !pawn.Downed)
            {
                CompAbilityUserMagic comp = pawn.TryGetComp<CompAbilityUserMagic>();
                Hediff hd = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_LightCapacitanceHD);
                if (comp != null && comp.SoL != null && hd != null)
                {
                    HediffComp_LightCapacitance hdlc = hd.TryGetComp<HediffComp_LightCapacitance>();
                    if (hdlc != null)
                    {
                        float val = (comp.SoL.LightEnergy + hdlc.LightEnergy) / 2f;
                        comp.SoL.LightEnergy = val;
                        hdlc.LightEnergy = val;
                        MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.2f);
                        MoteMaker.ThrowLightningGlow(comp.SoL.DrawPos, comp.SoL.Map, 1.2f);
                    }
                }
            }

            this.burstShotsLeft = 0;
            return false;
        }
    }
}
