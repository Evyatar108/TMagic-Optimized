using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;
using AbilityUser;

namespace TorannMagic
{
    public class Verb_Stoneskin : Verb_UseAbility
    {

        int pwrVal;
        int verVal;
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
            MagicPowerSkill pwr = comp.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_pwr");
            MagicPowerSkill ver = comp.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_ver");
            pwrVal = pwr.level;
            verVal = ver.level;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                pwrVal = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr").level;
                verVal = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver").level;
            }
            if (settingsRef.AIHardMode && !caster.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }

            if (pawn != null && pawn.health != null && pawn.health.hediffSet != null)
            {
                IEnumerable<Pawn> enumerable = from geomancer in caster.Map.mapPawns.AllPawnsSpawned
                                               where geomancer.RaceProps.Humanlike && geomancer.story.traits.HasTrait(TorannMagicDefOf.Geomancer)
                                               select geomancer;
                List<Pawn> geomancers = enumerable.ToList();
                foreach (var geomancer in enumerable)
                {
                    CompAbilityUserMagic compGeo = geomancer.GetComp<CompAbilityUserMagic>();
                    if (compGeo != null && compGeo.stoneskinPawns.Contains(pawn))
                    {
                        compGeo.stoneskinPawns.Remove(pawn);
                    }
                }
                if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD"), false))
                {
                    Hediff hediff = new Hediff();
                    hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_StoneskinHD"));
                    if (hediff.Severity < 4 + pwrVal)
                    {
                        ApplyStoneskin(pawn);
                    }
                    else
                    {
                        RemoveHediffs(pawn);
                        comp.stoneskinPawns.Remove(pawn);
                        SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                        info.pitchFactor = .7f;
                        SoundDefOf.EnergyShield_Broken.PlayOneShot(info);
                        MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.5f);
                    }
                }
                else
                {
                    ApplyStoneskin(pawn);
                }
            }

            return true;
        }

        public void ApplyStoneskin(Pawn pawn)
        {
            if (comp != null && !pawn.DestroyedOrNull() && !pawn.Dead && pawn.Map != null)
            {
                var stoneSkinPawns = comp.StoneskinPawns;
                if (stoneSkinPawns.Count() < verVal + 2)
                {
                    ApplyHediffs(pawn);
                    if (!stoneSkinPawns.Contains(pawn))
                    {
                        stoneSkinPawns.Add(pawn);
                    }
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = .7f;
                    SoundDefOf.EnergyShield_Reset.PlayOneShot(info);
                    MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.5f);
                    Effecter stoneskinEffecter = TorannMagicDefOf.TM_Stoneskin_Effecter.Spawn();
                    stoneskinEffecter.def.offsetTowardsTarget = FloatRange.Zero;
                    stoneskinEffecter.Trigger(new TargetInfo(pawn.Position, pawn.Map, false), new TargetInfo(pawn.Position, pawn.Map, false));
                    stoneskinEffecter.Cleanup();
                }
                else
                {
                    string stoneskinPawns = "";
                    int count = stoneSkinPawns.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (i + 1 == count) //last name
                        {
                            stoneskinPawns += stoneSkinPawns[i].LabelShort;
                        }
                        else
                        {
                            stoneskinPawns += stoneSkinPawns[i].LabelShort + " & ";
                        }
                    }
                    if (comp.Pawn.IsColonist)
                    {
                        Messages.Message("TM_TooManyStoneskins".Translate(
                                        caster.LabelShort,
                                        verVal + 2,
                                        stoneskinPawns
                            ), MessageTypeDefOf.RejectInput);
                    }
                }
            }
        }

        private void ApplyHediffs(Pawn target)
        {
            HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), -10);
            if (pwrVal == 3)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 7);
            }
            else if (pwrVal == 2)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 6);
            }
            else if (pwrVal == 1)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 5);
            }
            else
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 4);
            }
        }

        private void RemoveHediffs(Pawn target)
        {
            Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_StoneskinHD"));
            target.health.RemoveHediff(hediff);
        }
    }
}
