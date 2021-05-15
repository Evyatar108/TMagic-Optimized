using System.Collections.Generic;
using Verse.AI;
using RimWorld;
using Verse;
using System;


namespace TorannMagic
{
    public class JobDriver_DoMagicBill : JobDriver_DoBill
    {
        public int durationTicks = 60;

        public float workLeft;

        public int billStartTick;

        public int ticksSpentDoingRecipeWork;

        public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

        public const TargetIndex BillGiverInd = TargetIndex.A;

        public const TargetIndex IngredientInd = TargetIndex.B;

        public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f);
            Scribe_Values.Look(ref billStartTick, "billStartTick", 0);
            Scribe_Values.Look(ref ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = base.pawn;
            LocalTargetInfo target = base.job.GetTarget(TargetIndex.A);
            Job job = base.job;
            bool errorOnFailed2 = errorOnFailed;
            if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed2))
            {
                return false;
            }
            base.pawn.ReserveAsManyAsPossible(base.job.GetTargetQueue(TargetIndex.B), base.job);
            return true;
        }

        public IBillGiver BillGiver
        {
            get
            {
                IBillGiver billGiver = job.GetTarget(TargetIndex.A).Thing as IBillGiver;
                if (billGiver == null)
                {
                    throw new InvalidOperationException("DoBill on non-Billgiver.");
                }
                return billGiver;
            }
        }

        public override string GetReport()
        {
            if (job.RecipeDef != null)
            {
                return ReportStringProcessed(job.RecipeDef.jobString);
            }
            return base.GetReport();
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            //Log.Message("doing magic bill");
            //Log.Message("actor is " + this.GetActor().LabelShort);
            //Log.Message("doing job " + this.GetActor().CurJobDef);
            //Log.Message("bill thing is " + this.GetActor().CurJob.GetTarget(TargetIndex.A).Thing.Label);
            //if(this.GetActor().CurJob.targetA.Thing is Building_TMMagicCircle)
            //{
            //    Log.Message("target building is a magic circle");
            //}
            //Log.Message("toil is " + base.MakeNewToils().ToString());
            return base.MakeNewToils();
        }
    }
}