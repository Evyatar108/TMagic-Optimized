﻿using Verse;
using AbilityUser;

namespace TorannMagic
{
    public class Effect_DirtDevil : Verb_UseAbility
    {
        public virtual void Effect()
        {
            LocalTargetInfo t = this.TargetsAoE[0];
            bool flag = t.Cell != default(IntVec3);
            if (flag)
            {
                Thing dirtDevil = new Thing();
                dirtDevil.def = TorannMagicDefOf.FlyingObject_DirtDevil;
                Pawn casterPawn = base.CasterPawn;
                LongEventHandler.QueueLongEvent(delegate
                {
                    FlyingObject_DirtDevil flyingObject = (FlyingObject_DirtDevil)GenSpawn.Spawn(ThingDef.Named("FlyingObject_DirtDevil"), this.CasterPawn.Position, this.CasterPawn.Map);
                    flyingObject.Launch(this.CasterPawn, t.Cell, dirtDevil);
                }, "LaunchingFlyer", false, null);
            }
        }

        public override void PostCastShot(bool inResult, out bool outResult)
        {
            if (inResult)
            {
                this.Effect();
            }
            outResult = inResult;
        }
    }
}
