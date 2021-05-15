using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic.Conditions
{
    public class GameCondition_DivineBlessing : GameCondition
    {
        int age = -1;

        List<Corpse> potentialResurrection = new List<Corpse>();
        HashSet<Pawn> injuredPawns = new HashSet<Pawn>();

        public override void Init()
        {
            base.Init();
            if (this.SingleMap != null)
            {

                List<Thing> allThings = (from x in this.SingleMap.listerThings.AllThings
                                         where true
                                         select x).ToList<Thing>();

                potentialResurrection.Clear();

                for (int i = 0; i < allThings.Count; i++)
                {
                    Thing t = allThings[i];
                    if (t != null && t is Corpse)
                    {
                        Corpse c = t as Corpse;
                        if (c.InnerPawn.IsColonist && !c.IsDessicated())
                        {
                            potentialResurrection.Add(c);
                        }
                    }
                }

                if (Rand.Chance(.15f))
                {
                    Corpse c = this.potentialResurrection.RandomElement();
                    LocalTargetInfo targ = c;
                    TM_CopyAndLaunchProjectile.CopyAndLaunchThing(ThingDef.Named("Projectile_Resurrection"), c, targ, targ, ProjectileHitFlags.All);
                }

                for (int i = 0; i < this.SingleMap.mapPawns.FreeColonistsSpawned.Count; i++)
                {
                    Pawn p = this.SingleMap.mapPawns.FreeColonistsSpawned[i];
                    if (TM_Calc.IsPawnInjured(p, 0))
                    {
                        this.injuredPawns.Add(p);
                    }
                }

                foreach (var injuredPawn in injuredPawns)
                {
                    if (Rand.Chance(.6f))
                    {
                        HealthUtility.AdjustSeverity(injuredPawn, TorannMagicDefOf.TM_Regeneration_I, 2f);
                    }

                    if (Rand.Chance(.7f))
                    {
                        HealthUtility.AdjustSeverity(injuredPawn, TorannMagicDefOf.TM_DiseaseImmunityHD, 2f);
                    }
                }
            }
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();
            age++;

            if (age > 10)
            {
                End();
            }
        }
    }
}
