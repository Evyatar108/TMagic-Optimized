﻿using Verse;
using RimWorld;

namespace TorannMagic.Thoughts
{
    public class Inspiration_MightUser : InspirationWorker
    {
        public override bool InspirationCanOccur(Pawn pawn)
        {
            bool baseInspiration = base.InspirationCanOccur(pawn);
            bool mightInspiration = false;
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            if (comp.IsMightUser)
            {
                mightInspiration = true;
            }
            return baseInspiration && mightInspiration;
        }
    }
}
