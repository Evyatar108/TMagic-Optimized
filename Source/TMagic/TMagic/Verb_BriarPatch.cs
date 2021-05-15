using RimWorld;
using Verse;
using AbilityUser;
using System.Collections.Generic;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class Verb_BriarPatch : Verb_UseAbility
    {
        bool validTarg;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    //out of range
                    validTarg = true;
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
            bool result = false;
            Pawn p = this.CasterPawn;
            Pawn hitPawn = this.currentTarget.Thing as Pawn;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            Faction hitPawnFaction = null;

            if (this.currentTarget != null && p != null)
            {
                Map map = this.CasterPawn.Map;
                bool hostileAction = false;
                IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(this.currentTarget.Cell, this.verbProps.defaultProjectile.projectile.explosionRadius, true);
                foreach (var cell in cells)
                {
                    Thing briar = ThingMaker.MakeThing(TorannMagicDefOf.TM_Plant_Briar, null);
                    GenPlace.TryPlaceThing(briar, cell, this.CasterPawn.Map, ThingPlaceMode.Near);
                    Pawn aPawn = cell.GetFirstPawn(p.Map);
                    if (aPawn != null && aPawn.Faction != null && aPawn.Faction != p.Faction && !aPawn.Faction.HostileTo(p.Faction))
                    {
                        hitPawnFaction = aPawn.Faction;
                        hostileAction = true;
                    }
                }
                if (hostileAction)
                {
                    hitPawnFaction.TryAffectGoodwillWith(p.Faction, -20, true, false, null, null);
                }
                result = true;
            }

            this.burstShotsLeft = 0;
            return result;
        }
    }
}
