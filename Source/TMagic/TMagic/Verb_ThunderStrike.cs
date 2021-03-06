using Verse;
using AbilityUser;
using RimWorld;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Verb_ThunderStrike : Verb_UseAbility
    {
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

            if (this.CasterPawn.equipment.Primary == null)
            {
                base.TryCastShot();
                return true;
            }
            else
            {
                Messages.Message("MustBeUnarmed".Translate(
                    this.CasterPawn.LabelCap,
                    this.verbProps.label
                ), MessageTypeDefOf.RejectInput);
                return false;
            }
        }
    }
}
