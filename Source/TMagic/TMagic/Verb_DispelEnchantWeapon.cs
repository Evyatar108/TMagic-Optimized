using System.Collections.Generic;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_DispelEnchantWeapon : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            bool flag = false;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();

            if (comp.IsMagicUser)
            {
                if (comp.weaponEnchants.Count > 0)
                {
                    for (int i = 0; i < comp.weaponEnchants.Count; i++)
                    {
                        Pawn dispellingPawn = comp.weaponEnchants[i];
                        RemoveExistingEnchantment(dispellingPawn);
                        i--;
                    }
                    comp.weaponEnchants.Clear();
                    comp.RemovePawnAbility(TorannMagicDefOf.TM_DispelEnchantWeapon);
                }
            }

            this.PostCastShot(flag, out flag);
            return flag;
        }

        public static void RemoveExistingEnchantment(Pawn pawn)
        {
            IEnumerable<Hediff> allHediffs = pawn.health.hediffSet.GetHediffs<Hediff>();
            foreach (var hediff in allHediffs)
            {
                if (hediff.def.defName.Contains("TM_WeaponEnchantment"))
                {
                    pawn.health.RemoveHediff(hediff);
                }
            }
        }
    }
}
