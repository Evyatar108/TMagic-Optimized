using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic
{
    public class GameCondition_ManaDrain : GameCondition
    {
        List<Pawn> victims;

        public override void Init()
        {
            Map map = base.SingleMap;

            if (map != null)
            {
                victims = map.mapPawns.FreeColonistsAndPrisoners.ToList();
            }
            else
            {
                victims = new List<Pawn>();
                List<Map> allMaps = base.AffectedMaps;
                for (int i = 0; i < allMaps.Count; i++)
                {
                    victims.AddRange(allMaps[i].mapPawns.AllPawnsSpawned);
                }

            }
            int num = victims.Count;
            Pawn pawn;
            for (int i = 0; i < num; i++)
            {
                pawn = victims[i];
                if (pawn != null)
                {
                    CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                    if (comp != null && comp.IsMagicUser && comp.Mana != null)
                    {
                        if (comp.Mana.CurLevel == 1)
                        {
                            comp.Mana.CurLevel -= .01f;
                        }
                    }
                }
            }
        }

        public GameCondition_ManaDrain()
        {
        }
    }
}
