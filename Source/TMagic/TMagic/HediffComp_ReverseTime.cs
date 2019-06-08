﻿using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Harmony;

namespace TorannMagic
{
    public class HediffComp_ReverseTime : HediffComp
    {
        private bool initialized = true;

        public bool isBad = false;
        public int durationTicks = 6000;
        private int tickEffect = 300;

        private int currentAge = 1;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
            Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 6000, false);
            Scribe_Values.Look<int>(ref this.currentAge, "currentAge", 1, false);
            Scribe_Values.Look<int>(ref this.tickEffect, "tickEffect", 300, false);
        }

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned && base.Pawn.Map != null)
            {
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
            }
            this.currentAge = base.Pawn.ageTracker.AgeBiologicalYears;
            this.tickEffect = Mathf.RoundToInt(this.durationTicks / 500);
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null;
            if (flag)
            {
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }
            }

            if (Find.TickManager.TicksGame % 60 == 0)
            {                

                ReverseHediff(this.Pawn, 60);
                this.durationTicks -= 60;

                if (Find.TickManager.TicksGame % this.tickEffect == 0)
                {
                    ReverseEffects(this.Pawn, 1);
                }

                if (true)
                {
                    this.Pawn.ageTracker.AgeBiologicalTicks = Mathf.RoundToInt(this.Pawn.ageTracker.AgeBiologicalTicks - 10000);
                    if (this.Pawn.ageTracker.AgeBiologicalTicks < 0)
                    {
                        Messages.Message("TM_CeaseToExist".Translate(this.Pawn.LabelShort), MessageTypeDefOf.NeutralEvent);
                        this.Pawn.Destroy(DestroyMode.Vanish);
                    }
                }
            }
        }

        private void ReverseHediff(Pawn pawn, int ticks)
        {
            float totalBleedRate = 0;
            using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {                    
                    Hediff rec = enumerator.Current;
                    if (rec != null)
                    {
                        HediffComp_Immunizable immuneComp = rec.TryGetComp<HediffComp_Immunizable>();
                        if (immuneComp != null)
                        {
                            if (immuneComp.Def.CompProps<HediffCompProperties_Immunizable>() != null)
                            {
                                float immuneSevDay = immuneComp.Def.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
                                if (immuneSevDay != 0 && !rec.FullyImmune())
                                {
                                    rec.Severity -= ((immuneSevDay * ticks * this.parent.Severity) / (2500));
                                }
                            }
                        }
                        HediffComp_SeverityPerDay sevDayComp = rec.TryGetComp<HediffComp_SeverityPerDay>();
                        if (sevDayComp != null)
                        {
                            if (sevDayComp.Def.CompProps<HediffCompProperties_SeverityPerDay>() != null)
                            {
                                float sevDay = sevDayComp.Def.CompProps<HediffCompProperties_SeverityPerDay>().severityPerDay;
                                if (sevDay != 0)
                                {
                                    rec.Severity -= ((sevDay * ticks * this.parent.Severity) / (2500));
                                }
                            }
                        }
                        HediffComp_Disappears tickComp = rec.TryGetComp<HediffComp_Disappears>();
                        if (tickComp != null)
                        {
                            int ticksToDisappear = Traverse.Create(root: tickComp).Field(name: "ticksToDisappear").GetValue<int>();
                            if (ticksToDisappear != 0)
                            {
                                Traverse.Create(root: tickComp).Field(name: "ticksToDisappear").SetValue(ticksToDisappear + (Mathf.RoundToInt(60 * this.parent.Severity)));
                            }
                        }
                        if (rec.Bleeding)
                        {
                            totalBleedRate += rec.BleedRate;
                        }
                    }
                }
                if (totalBleedRate != 0)
                {
                    HealthUtility.AdjustSeverity(pawn, HediffDefOf.BloodLoss, -(totalBleedRate * 60 * this.parent.Severity) / (24 * 2500));
                }
            }
            List<Hediff> hediffList = pawn.health.hediffSet.GetHediffs<Hediff>().ToList();
            if (hediffList != null && hediffList.Count > 0)
            {
                for (int i = 0; i < hediffList.Count; i++)
                {
                    Hediff rec = hediffList[i];
                    if (rec != null && rec != this.parent)
                    {
                        if ((rec.ageTicks - 2500) < 0)
                        {
                            if (rec.def.defName.Contains("TM_"))
                            {
                                if (rec.def.isBad)
                                {
                                    this.Pawn.health.RemoveHediff(rec);                                    
                                    break;
                                }
                            }
                            else
                            {
                                this.Pawn.health.RemoveHediff(rec);
                                break;
                            }
                        }
                        else
                        {
                            rec.ageTicks -= 2500;
                        }
                    }
                }
            }
            //using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        Hediff rec = enumerator.Current;
            //        if (rec != null && rec != this.parent)
            //        {
            //            if ((rec.ageTicks - 2500) < 0)
            //            {
            //                if (rec.def.defName.Contains("TM_"))
            //                {
            //                    if (rec.def.isBad)
            //                    {
            //                        this.Pawn.health.RemoveHediff(rec);
            //                    }
            //                }
            //                else
            //                {
            //                    this.Pawn.health.RemoveHediff(rec);
            //                }
            //            }
            //            else
            //            {
            //                rec.ageTicks -= 2500;
            //            }
            //        }
            //    }
            //}
        }

        public void ReverseEffects(Pawn pawn, int intensity)
        {
            Effecter ReverseEffect = TorannMagicDefOf.TM_TimeReverseEffecter.Spawn();
            ReverseEffect.Trigger(new TargetInfo(pawn), new TargetInfo(pawn));
            ReverseEffect.Cleanup();
        }

        public override bool CompShouldRemove
        {
            get
            {
                return base.CompShouldRemove || this.durationTicks <= 0;
            }
        }
    }
}