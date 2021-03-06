using Verse;
using AbilityUser;
using UnityEngine;
using RimWorld;
using System.Collections.Generic;

namespace TorannMagic
{
    public class Verb_LightBurst : Verb_UseAbility
    {
        private int verVal;
        private int pwrVal;
        private int burstCount = 2;
        private bool initialized = false;
        private float arcaneDmg = 1f;
        private float lightPotency = .5f;

        private void Initialize(Pawn pawn)
        {
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwrVal = TM_Calc.GetMagicSkillLevel(pawn, comp.MagicData.MagicPowerSkill_LightBurst, "TM_LightBurst", "_pwr", TorannMagicDefOf.TM_LightBurst.canCopy);
            verVal = TM_Calc.GetMagicSkillLevel(pawn, comp.MagicData.MagicPowerSkill_LightBurst, "TM_LightBurst", "_ver", TorannMagicDefOf.TM_LightBurst.canCopy);
            this.burstCount = 2;
            if (verVal >= 1)
            {
                burstCount++;
                if (verVal >= 3)
                {
                    burstCount++;
                }
            }
            this.arcaneDmg = comp.arcaneDmg;
            if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_LightCapacitanceHD))
            {
                HediffComp_LightCapacitance hd = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_LightCapacitanceHD).TryGetComp<HediffComp_LightCapacitance>();
                this.lightPotency = hd.LightPotency;
                hd.LightEnergy -= 2.5f * this.burstCount;
            }
        }

        protected override bool TryCastShot()
        {
            Pawn pawn = this.CasterPawn;
            if (!initialized)
            {
                this.initialized = true;
                this.Initialize(pawn);
            }

            Map map = this.CasterPawn.Map;
            IntVec3 targetVariation = this.currentTarget.Cell;
            float radius = (this.Ability.Def.MainVerb.TargetAoEProperties.range / 2f) + (.3f * pwrVal);
            targetVariation.x += Mathf.RoundToInt(Rand.Range(-radius, radius));
            targetVariation.z += Mathf.RoundToInt(Rand.Range(-radius, radius));
            this.CreateLightBurst(targetVariation, map, radius);
            this.ApplyEffects(targetVariation, map, radius);
            this.burstCount--;
            bool result = burstCount > 0;
            return result;
        }

        public void CreateLightBurst(IntVec3 center, Map map, float radius)
        {
            GenClamor.DoClamor(this.CasterPawn, 2 * radius, ClamorDefOf.Ability);
            Effecter flashED = TorannMagicDefOf.TM_LightBurstED.Spawn();
            flashED.Trigger(new TargetInfo(center, map, false), new TargetInfo(center, map, false));
            flashED.Cleanup();
            TargetInfo ti = new TargetInfo(center, map, false);
            TM_MoteMaker.MakeOverlay(ti, TorannMagicDefOf.TM_Mote_PsycastAreaEffect, map, Vector3.zero, .1f, 0f, .05f, .1f, .1f, 4.3f);
        }

        public void ApplyEffects(IntVec3 center, Map map, float radius)
        {
            List<Pawn> targets = TM_Calc.FindAllPawnsAround(map, center, radius);
            float baseDamage = 2f * lightPotency * arcaneDmg;
            if (targets != null && targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    Pawn p = targets[i];
                    bool flag = p.RaceProps.IsFlesh || (p.RaceProps.IsMechanoid && pwrVal >= 2);
                    if (flag)
                    {
                        IEnumerable<BodyPartRecord> bpr = p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource);
                        float distanceToCenter = (p.Position - center).LengthHorizontal;
                        float distanceMultiplier = radius - distanceToCenter;
                        if (p.Faction != null && p.Faction == this.CasterPawn.Faction)
                        {
                            distanceMultiplier *= .5f;
                        }
                        foreach (var bodyPartRecord in bpr)
                        {
                            TM_Action.DamageEntities(p, bodyPartRecord, distanceMultiplier * (baseDamage + (.3f * pwrVal)), TMDamageDefOf.DamageDefOf.TM_BurningLight, this.CasterPawn);
                            HealthUtility.AdjustSeverity(p, TorannMagicDefOf.TM_LightBurstHD, distanceMultiplier * lightPotency * (.1f + (.015f * pwrVal)));
                        }
                    }
                }
            }
        }
    }
}
