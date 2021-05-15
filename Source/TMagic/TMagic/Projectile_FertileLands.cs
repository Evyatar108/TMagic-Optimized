using Verse;
using AbilityUser;
using System.Collections.Generic;

namespace TorannMagic
{
    public class Projectile_FertileLands : Projectile_AbilityBase
    {
        bool initialized = false;
        Pawn caster;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            this.caster = this.launcher as Pawn;

            if (!this.initialized)
            {
                this.initialized = true;
            }

            CompAbilityUserMagic comp = this.caster.GetComp<CompAbilityUserMagic>();
            comp.fertileLands = new List<IntVec3>();
            comp.fertileLands.Clear();
            List<IntVec3> affectedCells = new List<IntVec3>();
            affectedCells.Clear();
            affectedCells = ModOptions.Constants.GetGrowthCells();
            IEnumerable<IntVec3> targetCells = GenRadial.RadialCellsAround(base.Position, 6, true);
            foreach (IntVec3 targetCell in targetCells)
            {
                bool uniqueCell = true;
                for (int j = 0; j < affectedCells.Count; j++)
                {
                    if (affectedCells[j] == targetCell)
                    {
                        uniqueCell = false;
                    }
                }
                if (uniqueCell)
                {
                    comp.fertileLands.Add(targetCell);
                }
            }
            TM_MoteMaker.ThrowTwinkle(base.Position.ToVector3Shifted(), map, 1f);

            ModOptions.Constants.SetGrowthCells(comp.fertileLands);
            comp.RemovePawnAbility(TorannMagicDefOf.TM_FertileLands);
            comp.AddPawnAbility(TorannMagicDefOf.TM_DismissFertileLands);
        }
    }
}


