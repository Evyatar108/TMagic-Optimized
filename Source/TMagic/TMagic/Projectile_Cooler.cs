﻿using RimWorld;
using AbilityUser;
using Verse;
using System.Linq;

namespace TorannMagic
{
    public class Projectile_Cooler : Projectile_AbilityBase
    {

        private bool primed = false;
        CompAbilityUserMagic comp;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            IntVec3 arg_pos_1;

            Pawn pawn = this.launcher as Pawn;
            comp = pawn.GetComp<CompAbilityUserMagic>();

            CellRect cellRect = CellRect.CenteredOn(base.Position, 1);
            cellRect.ClipInsideMap(map);
            IntVec3 centerCell = cellRect.CenterCell;

            if (!this.primed)
            {
                arg_pos_1 = centerCell;
                if (arg_pos_1.IsValid && arg_pos_1.Standable(map))
                {
                    AbilityUser.SpawnThings tempPod = new SpawnThings();
                    IntVec3 shiftPos = centerCell;
                    centerCell.x++;
                    tempPod.def = ThingDef.Named("TM_Cooler");
                    tempPod.spawnCount = 1;
                    try
                    {
                        this.SingleSpawnLoop(tempPod, shiftPos, map);
                    }
                    catch
                    {
                        Log.Message("Attempted to create a cooler but threw an unknown exception - recovering and ending attempt");
                        return;
                    }

                    this.primed = true;
                }
                else
                {
                    Messages.Message("InvalidSummon".Translate(), MessageTypeDefOf.RejectInput);
                    comp.Mana.GainNeed(comp.ActualManaCost(TorannMagicDefOf.TM_Cooler));
                }
            }
        }

        public void SingleSpawnLoop(SpawnThings spawnables, IntVec3 position, Map map)
        {
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = TM_Action.ResolveFaction(this.launcher as Pawn, spawnables, this.launcher.Faction);
                bool flag2 = spawnables.def.race != null;
                if (flag2)
                {
                    bool flag3 = spawnables.kindDef == null;
                    if (flag3)
                    {
                        Log.Error("Missing kinddef");
                    }
                    else
                    {
                        TM_Action.SpawnPawn(this.launcher as Pawn, spawnables, faction, position, 0, map);
                    }
                }
                else
                {
                    ThingDef def = spawnables.def;
                    ThingDef stuff = null;
                    bool madeFromStuff = def.MadeFromStuff;
                    if (madeFromStuff)
                    {
                        stuff = ThingDefOf.WoodLog;
                    }
                    Thing thing = ThingMaker.MakeThing(def, stuff);
                    if (thing.def.defName != "Portfuel")
                    {
                        thing.SetFaction(faction, null);
                    }
                    CompSummoned bldgComp = thing.TryGetComp<CompSummoned>();
                    bldgComp.Temporary = false;
                    bldgComp.Spawner = this.launcher as Pawn;
                    bldgComp.sustained = true;
                    GenSpawn.Spawn(thing, position, map, Rot4.North, WipeMode.Vanish, false);
                    comp.summonedCoolers.Add(thing);
                    Building_TMCooler cooler = thing as Building_TMCooler;
                    if (cooler != null)
                    {
                        if (comp.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_pwr").level >= 12)
                        {
                            cooler.defensive = true;
                        }
                        if (comp.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level >= 6)
                        {
                            cooler.buffCool = true;
                        }
                        if (comp.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level >= 9)
                        {
                            cooler.buffFresh = true;
                        }
                    }
                }
            }
        }
    }
}
