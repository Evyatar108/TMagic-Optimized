﻿using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using AbilityUser;

namespace TorannMagic
{
    public class JobGiver_MonkMeditate : ThinkNode_JobGiver 
    {
        int verVal = 0;
        public override float GetPriority(Pawn pawn)
        {
            
            Hediff chiHD = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD);
            this.verVal = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_ThunderStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ThunderStrike_ver").level;
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            if (chiHD == null)
            {
                return 0f;
            }
            if(comp == null)
            {
                return 0f;
            }
            if ((int)chiHD.Severity > 80)
            {
                return 0f;
            }
            if (Find.TickManager.TicksGame < comp.allowMeditateTick)
            {
                return 0f;
            }
            TimeAssignmentDef timeAssignmentDef;
            if (pawn.RaceProps.Humanlike)
            {
                timeAssignmentDef = ((pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything);
            }
            else
            {
                int num = GenLocalDate.HourOfDay(pawn);
                timeAssignmentDef = ((num >= 7 && num <= 21) ? TimeAssignmentDefOf.Anything : TimeAssignmentDefOf.Sleep);
            }
            float curLevel = chiHD.Severity;
            if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
            {
                if (curLevel < 30.0f)
                {
                    return 8f;
                }
                else if(curLevel < 70.0f)
                {
                    return 4f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Work)
            {
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
            {
                if (curLevel < 70f)
                {
                    return 8f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
            {
                if (curLevel < 50 && verVal >= 3)
                {
                    return 8f;
                }
                else if(curLevel < 70 && verVal >= 2)
                {
                    return 6f;
                }
                return 0f;
            }
            throw new NotImplementedException();
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn != null && pawn.Map != null && pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ChiHD) && !pawn.Drafted)
            {
                if (pawn.InBed() || HealthAIUtility.ShouldSeekMedicalRest(pawn))
                {
                    return null;
                }
                Need_Joy curJoy = pawn.needs.joy;
                if(curJoy == null)
                {
                    return null;
                }
                CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                if (comp != null && curJoy.CurLevel < .8f)
                {
                    MightPower mightPower = comp.MightData.MightPowersM.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Meditate);
                     
                    if (mightPower == null)
                    {
                        return null;
                    }

                    if (!mightPower.AutoCast)
                    {
                        return null;
                    }

                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD);
                    PawnAbility ability = pawn.GetComp<CompAbilityUserMight>().AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Meditate);

                    if (ability.CooldownTicksLeft > 0 || hediff.Severity >= 70)
                    {
                        return null;
                    }

                    Building_Bed building_Bed = pawn.ownership.OwnedBed;
                    if (building_Bed != null)
                    {
                        if (building_Bed.GetRoom() != null && !building_Bed.GetRoom().PsychologicallyOutdoors)
                        {
                            List<IntVec3> roomCells = building_Bed.GetRoom().Cells.ToList();
                            for (int i = 0; i < roomCells.Count; i++)
                            {
                                if (roomCells[i].IsValid && roomCells[i].Walkable(pawn.Map) && roomCells[i].GetFirstBuilding(pawn.Map) == null)
                                {
                                    return new Job(TorannMagicDefOf.JobDriver_TM_Meditate, roomCells[i]);
                                }
                            }
                        }

                    }
                    return new Job(TorannMagicDefOf.JobDriver_TM_Meditate, pawn.Position);
                }
            }
            return null;
        }


    }
}
