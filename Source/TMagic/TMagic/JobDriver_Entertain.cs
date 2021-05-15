﻿using System.Collections.Generic;
using Verse.AI;
using Verse;
using RimWorld;


namespace TorannMagic
{
    internal class JobDriver_Entertain : JobDriver
    {
        CompAbilityUserMagic comp;

        int age = -1;
        int duration = 120;

        protected Pawn entertaineePawn
        {
            get
            {
                return (Pawn)this.job.targetA.Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            comp = pawn.GetComp<CompAbilityUserMagic>();
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            this.FailOnDowned(TargetIndex.A);
            this.FailOnMentalState(TargetIndex.A);
            Toil entertain = new Toil()
            {
                initAction = () =>
                {
                    if (!entertaineePawn.Spawned && !entertaineePawn.Awake())
                    {
                        return;
                    }
                },
                tickAction = () =>
                {
                    if (age > duration)
                    {
                        this.pawn.interactions.TryInteractWith(entertaineePawn, TorannMagicDefOf.TM_EntertainID);
                        MoteMaker.ThrowMicroSparks(this.pawn.DrawPos, this.pawn.Map);
                        this.EndJobWith(JobCondition.Succeeded);
                        comp.nextEntertainTick = Find.TickManager.TicksGame + 2000;
                        age = 0;
                    }
                    age++;
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            yield return entertain;
        }
    }
}