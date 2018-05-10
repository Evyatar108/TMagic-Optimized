﻿using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TorannMagic
{
    class Projectile_FogOfTorment : Projectile_AbilityBase
    {
        int age = -1;
        int duration = 1800;
        int verVal = 0;
        int pwrVal = 0;
        float arcaneDmg = 1;
        int strikeDelay = 180;
        int lastStrike = 0;
        bool initialized = false;
        ThingDef fog;

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                base.Destroy(mode);
            }
        }

        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;

            Pawn pawn = this.launcher as Pawn;
            Pawn victim = null;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_pwr");
            MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_ver");
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            pwrVal = pwr.level;
            verVal = ver.level;
            this.arcaneDmg = comp.arcaneDmg;
            if (settingsRef.AIHardMode && !pawn.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }

            if (!this.initialized)
            {
                fog = TorannMagicDefOf.Fog_Torment;
                this.duration = this.duration + (180 * verVal);
                this.strikeDelay = this.strikeDelay - (18 * verVal);

                fog.gas.expireSeconds.min = this.duration/60;
                fog.gas.expireSeconds.max = this.duration/60;
                GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius + verVal, TMDamageDefOf.DamageDefOf.TM_Torment, this.launcher, 0, this.def.projectile.soundExplode, def, this.equipmentDef, fog, 1f, 1, false, null, 0f, 0, 0.0f, false);
                
                this.initialized = true;
            }

            if (this.age > this.lastStrike + this.strikeDelay)
            {
                IntVec3 curCell;
                IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(base.Position, this.def.projectile.explosionRadius + verVal, true);
                for (int i = 0; i < targets.Count(); i++)
                {
                    curCell = targets.ToArray<IntVec3>()[i];

                    if (curCell.InBounds(map) && curCell.IsValid)
                    {
                        victim = curCell.GetFirstPawn(map);
                        if(victim != null && !victim.Dead && victim.RaceProps.IsFlesh)
                        {
                            if(victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD) || victim.needs.food == null)
                            {
                                //heals undead
                                int num = 1;
                                int num2 = 1;
                                using (IEnumerator<BodyPartRecord> enumerator = victim.health.hediffSet.GetInjuredParts().GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        BodyPartRecord rec = enumerator.Current;
                                        bool flag2 = num > 0;

                                        if (flag2)
                                        {
                                            IEnumerable<Hediff_Injury> arg_BB_0 = victim.health.hediffSet.GetHediffs<Hediff_Injury>();
                                            Func<Hediff_Injury, bool> arg_BB_1;

                                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                            foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                            {
                                                bool flag3 = num2 > 0;
                                                if (flag3)
                                                {
                                                    bool flag5 = current.CanHealNaturally() && !current.IsOld();
                                                    if (flag5)
                                                    {
                                                        current.Heal(2.0f + pwrVal);
                                                        num--;
                                                        num2--;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //kills living
                                damageEntities(victim, Mathf.RoundToInt(Rand.Range(2f + (1f * pwrVal), 4f + (1f * pwrVal)) * this.arcaneDmg), TMDamageDefOf.DamageDefOf.TM_Torment);
                            }
                        }
                    }
                }
                this.lastStrike = this.age;
            }
        }            

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
            Scribe_Values.Look<int>(ref this.age, "age", -1, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 1800, false);
            Scribe_Values.Look<int>(ref this.strikeDelay, "shockDelay", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrike, "lastStrike", 0, false);
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_Defs.Look<ThingDef>(ref this.fog, "fog");
        }

        public void damageEntities(Pawn e, float d, DamageDef type)
        {
            int amt = Mathf.RoundToInt(Rand.Range(.5f, 1.5f) * d);
            DamageInfo dinfo = new DamageInfo(type, amt, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
            }
        }
    }    
}


