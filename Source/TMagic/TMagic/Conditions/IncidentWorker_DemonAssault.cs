﻿using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;


namespace TorannMagic.Conditions
{
    public class IncidentWorker_DemonAssault : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            bool tempAllow = false;
            Map map = (Map)parms.target;
            MagicMapComponent mmc = map.GetComponent<MagicMapComponent>();
            if (mmc != null && mmc.allowAllIncidents)
            {
                tempAllow = true;
            }
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.demonAssaultChallenge > 0 || tempAllow)
            {
                string str = "";

                int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
                IEnumerable<Faction> lichFaction = Find.FactionManager.AllFactions;
                bool factionFlag = false;
                foreach (var faction in lichFaction)
                {
                    if (faction.def.defName == "TM_SkeletalFaction")
                    {
                        Faction.OfPlayer.TrySetRelationKind(faction, FactionRelationKind.Hostile, false, null, null);
                        factionFlag = true;
                    }
                }
                if (!factionFlag)
                {
                    return false;
                }
                TM_Action.ForceFactionDiscoveryAndRelation(TorannMagicDefOf.TM_SkeletalFaction);
                GameCondition_DemonAssault gameCondition_DemonAssault = (GameCondition_DemonAssault)GameConditionMaker.MakeCondition(GameConditionDef.Named("DemonAssault"), duration);
                map.gameConditionManager.RegisterCondition(gameCondition_DemonAssault);
                base.SendStandardLetter(parms, gameCondition_DemonAssault.lookTarget, str);
                //base.SendStandardLetter(new TargetInfo(gameCondition_ElementalAssault.centerLocation.ToIntVec3, map, false), null, new string[0]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}



