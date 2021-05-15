﻿using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class HediffComp_Shapeshift : HediffComp  //not used
    {

        private bool initialized = false;
        private bool removeNow = false;

        private int eventFrequency = 10;
        private int shroudFrequency = 4;
        private int lastHateTick = 0;
        private float lastHate = 0;

        private int hatePwr = 0;
        private int hateVer = 0;
        private int hateEff = 0;

        List<StatModifier> stats;

        public override void CompExposeData()
        {
            base.CompExposeData();
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
            CompAbilityUserMight comp = this.Pawn.GetComp<CompAbilityUserMight>();
            this.stats = new List<StatModifier>();
            this.stats.Clear();
            if (spawned && comp != null && comp.IsMightUser)
            {
                hatePwr = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_pwr").level;
                hateVer = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_ver").level;
                hateEff = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_eff").level;

                //CollectStatModifiers();
            }
            else
            {
                this.removeNow = true;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null && base.Pawn.Map != null;
            if (flag)
            {
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }

                if (Find.TickManager.TicksGame % this.eventFrequency == 0)
                {

                    if ((this.lastHateTick + 1200) < Find.TickManager.TicksGame && this.parent.Severity > (25f + (this.Pawn.health.hediffSet.PainTotal * 50)))
                    {
                        severityAdjustment -= Rand.Range(.1f, .2f);
                    }

                    if (this.parent.Severity < (20f + (this.Pawn.health.hediffSet.PainTotal * 50)))
                    {
                        severityAdjustment += Rand.Range(.1f, .15f);
                    }
                    if ((this.lastHate + .35f) < this.parent.Severity)
                    {
                        this.lastHateTick = Find.TickManager.TicksGame;
                    }
                    this.lastHate = this.parent.Severity;
                }

                if (!this.Pawn.Downed && Find.TickManager.TicksGame % this.shroudFrequency == 0 && this.parent.Severity >= 10f)
                {
                    AbsorbProjectiles();
                }
            }
        }

        public override bool CompShouldRemove
        {
            get
            {
                return base.CompShouldRemove || this.removeNow;
            }
        }

        private void AbsorbProjectiles()
        {
            IEnumerable<IntVec3> shroudCells = GenRadial.RadialCellsAround(this.Pawn.Position, 1.4f + (this.parent.Severity / 100), true);
            foreach (IntVec3 cell in shroudCells)
            {
                List<Thing> thingList = cell.GetThingList(this.Pawn.Map);
                for (int j = 0; j < thingList.Count; j++)
                {
                    Projectile projectile = thingList[j] as Projectile;
                    if (projectile != null)
                    {
                        Vector3 projectileOrigin = Traverse.Create(root: projectile).Field(name: "origin").GetValue<Vector3>();
                        Thing launcher = Traverse.Create(root: projectile).Field(name: "launcher").GetValue<Thing>();
                        float weaponDamageMultiplier = Traverse.Create(root: projectile).Field(name: "weaponDamageMultiplier").GetValue<float>();
                        if (weaponDamageMultiplier > 0 && launcher != null && launcher != this.Pawn && (projectileOrigin - this.Pawn.DrawPos).MagnitudeHorizontal() > 6 && Rand.Chance(.3f + (.06f * this.hateVer)))
                        {
                            Vector3 moteDirection = TM_Calc.GetVector(projectile.ExactPosition, this.Pawn.DrawPos);
                            //Vector3 displayEffect = projectile.DrawPos;
                            //displayEffect.x += Rand.Range(-.3f, .3f);
                            //displayEffect.y += Rand.Range(-.3f, .3f);
                            //displayEffect.z += Rand.Range(-.3f, .3f);

                            float projectileDamage = 1;
                            if (projectile.def.defName != "Spark")
                            {
                                projectileDamage = projectile.def.projectile.GetDamageAmount(1, null);
                            }
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Shadow"), projectile.DrawPos, this.Pawn.Map, Rand.Range(.6f, .8f), 0.3f, Rand.Range(.1f, .2f), Rand.Range(.2f, .5f), Rand.Range(-300, 300), Rand.Range(1.2f, 2f), (Quaternion.AngleAxis(90, Vector3.up) * moteDirection).ToAngleFlat(), Rand.Range(0, 360));
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Shadow"), projectile.DrawPos, this.Pawn.Map, Rand.Range(.4f, .6f), 0.3f, Rand.Range(.1f, .2f), Rand.Range(.2f, .5f), Rand.Range(-300, 300), Rand.Range(.8f, 1.2f), (Quaternion.AngleAxis(90, Vector3.up) * moteDirection).ToAngleFlat(), Rand.Range(0, 360));
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_LightningGlow"), projectile.DrawPos, this.Pawn.Map, projectileDamage / 8f, .2f, .1f, .3f, 0, 0, 0, Rand.Range(0, 360));
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Shadow"), this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.6f, .8f), 0.3f, Rand.Range(.1f, .2f), Rand.Range(.2f, .5f), Rand.Range(-300, 300), Rand.Range(.6f, 1f), (Quaternion.AngleAxis(-90, Vector3.up) * moteDirection).ToAngleFlat(), Rand.Range(0, 360));

                            float sevReduct = projectileDamage / (2 + (.5f * hateEff));
                            HealthUtility.AdjustSeverity(this.Pawn, this.parent.def, -sevReduct);


                            projectile.Destroy(DestroyMode.Vanish);
                        }
                    }
                }
            }
        }
    }
}
