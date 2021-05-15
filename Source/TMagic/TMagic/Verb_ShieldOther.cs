﻿using RimWorld;
using Verse;
using AbilityUser;

namespace TorannMagic
{
    public class Verb_ShieldOther : Verb_UseAbility
    {
        CompAbilityUserMagic comp;

        bool validTarg;
        //Used for non-unique abilities that can be used with shieldbelt
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.Thing != null && targ.Thing == this.caster)
            {
                return this.verbProps.targetParams.canTargetSelf;
            }
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = this.TryFindShootLineFromTo(root, targ, out _);
                }
                else
                {
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            comp = caster.GetComp<CompAbilityUserMagic>();


            if (pawn != null)
            {
                ApplyShield(pawn);
            }
            return true;
        }

        public void ApplyShield(Pawn pawn)
        {
            ApplyHediffs(pawn);
            MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.5f);
            TM_Action.DisplayShield(pawn, 5f);
        }

        private void ApplyHediffs(Pawn target)
        {
            HealthUtility.AdjustSeverity(target, TorannMagicDefOf.TM_MagicShieldHD, (.32f + (.002f * comp.MagicUserLevel)) * comp.arcaneDmg);
        }
    }
}
