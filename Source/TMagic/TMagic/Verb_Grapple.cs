﻿using RimWorld;
using Verse;
using AbilityUser;
using UnityEngine;

namespace TorannMagic
{
    public class Verb_Grapple : Verb_UseAbility_TrueBurst
    {
        Vector3 pVect;
        bool validTarg;

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

            bool flag10 = false;
            this.TargetsAoE.Clear();
            this.UpdateTargets();
            bool flag2 = this.UseAbilityProps.AbilityTargetCategory != AbilityTargetCategory.TargetAoE && this.TargetsAoE.Count > 1;
            if (flag2)
            {
                this.TargetsAoE.RemoveRange(0, this.TargetsAoE.Count - 1);
            }
            for (int i = 0; i < this.TargetsAoE.Count; i++)
            {
                bool? flag3 = this.TryLaunchProjectile(this.verbProps.defaultProjectile, this.TargetsAoE[i]);
                bool hasValue = flag3.HasValue;
                if (hasValue)
                {
                    bool flag4 = flag3 == true;
                    if (flag4)
                    {
                        flag10 = true;
                    }
                    bool flag5 = flag3 == false;
                    if (flag5)
                    {
                        flag10 = false;
                    }
                }
            }

            CellRect cellRect = CellRect.CenteredOn(this.currentTarget.Cell, 1);
            Map map = caster.Map;
            cellRect.ClipInsideMap(map);

            IntVec3 centerCell = cellRect.CenterCell;
            Pawn victim = null;
            //dinfo.SetAmount(10);            
            //dinfo.SetWeaponHediff(TorannMagicDefOf.TM_GrapplingHook);

            bool pflag = true;

            Thing summonableThing = centerCell.GetFirstPawn(map);
            if (summonableThing == null)
            {
                pflag = false;
                //miss
            }
            else
            {
                pVect = summonableThing.TrueCenter();
                pVect.x = base.caster.TrueCenter().x;
                pVect.z = base.caster.TrueCenter().z;
                pVect.y = 0f;
                victim = summonableThing as Pawn;
                if (victim != null)
                {
                    if (!victim.IsColonist && !victim.IsPrisoner && !victim.Faction.HostileTo(this.CasterPawn.Faction) && victim.Faction != null && victim.RaceProps.Humanlike)
                    {
                        Faction faction = victim.Faction;
                        faction.TrySetRelationKind(this.CasterPawn.Faction, FactionRelationKind.Ally, false, null);
                    }
                }

            }

            bool arg_40_0;
            if (this.currentTarget != null && base.caster != null)
            {
                arg_40_0 = this.caster.Position.IsValid;
            }
            else
            {
                arg_40_0 = false;
            }
            bool flag = arg_40_0;
            if (flag)
            {
                if (summonableThing != null)
                {
                    if (pflag)
                    {
                        DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Stun, 10, 10, -1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, victim);
                        if (!victim.RaceProps.Humanlike || victim.Faction == this.CasterPawn.Faction)
                        {
                            if (ModCheck.Validate.GiddyUp.Core_IsInitialized())
                            {
                                ModCheck.GiddyUp.ForceDismount(victim);
                            }
                            victim.Position = base.Caster.Position;
                            victim.Notify_Teleported();
                            victim.TakeDamage(dinfo2);
                            //summonablePawn = (FlyingObject)GenSpawn.Spawn(ThingDef.Named("TM_SummonedPawn"), summonableThing.Position, summonableThing.Map);
                            //summonablePawn.impactDamage = dinfo2;
                            //summonablePawn.Launch(base.caster, new LocalTargetInfo(pVect.ToIntVec3()), summonableThing);
                        }
                        else if (victim.RaceProps.Humanlike && victim.Faction != this.CasterPawn.Faction && Rand.Chance(TM_Calc.GetSpellSuccessChance(this.CasterPawn, victim, true)))
                        {
                            if (ModCheck.Validate.GiddyUp.Core_IsInitialized())
                            {
                                ModCheck.GiddyUp.ForceDismount(victim);
                            }
                            victim.Position = base.Caster.Position;
                            victim.Notify_Teleported();
                            victim.TakeDamage(dinfo2);
                            //summonablePawn = (FlyingObject)GenSpawn.Spawn(ThingDef.Named("TM_SummonedPawn"), summonableThing.Position, summonableThing.Map);
                            //summonablePawn.impactDamage = dinfo2;
                            //summonablePawn.Launch(base.caster, new LocalTargetInfo(pVect.ToIntVec3()), summonableThing);
                        }
                        else
                        {
                            MoteMaker.ThrowText(victim.DrawPos, victim.Map, "TM_ResistedSpell".Translate(), -1);
                        }
                    }
                    else
                    {
                        //miss
                    }
                }
            }
            else
            {
                Log.Warning("failed to TryCastShot");
            }
            //this.burstShotsLeft = 0;
            //this.ability.TicksUntilCasting = (int)base.UseAbilityProps.SecondsToRecharge * 60;
            this.PostCastShot(flag10, out _);
            return flag;
        }

    }
}
