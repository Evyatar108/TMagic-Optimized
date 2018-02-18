﻿using System.Linq;
using Verse;
using AbilityUser;

namespace TorannMagic
{
    class Projectile_Firebolt : Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            Pawn pawn = this.launcher as Pawn;
            Pawn victim = hitThing as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firebolt_pwr");

            //GenExplosion.DoExplosion(base.Position, map, 0.4f, TMDamageDefOf.DamageDefOf.Firebolt, this.launcher, this.def.projectile.soundExplode, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1);
            GenExplosion.DoExplosion(base.Position, map, 0.4f, TMDamageDefOf.DamageDefOf.Firebolt, this.launcher, this.def.projectile.damageAmountBase, this.def.projectile.soundExplode, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1, 0.6f, false);
            CellRect cellRect = CellRect.CenteredOn(base.Position, 3);
            cellRect.ClipInsideMap(map);

            victim = base.Position.GetFirstPawn(map);
            if (victim != null)
            {                
                int dmg = (this.def.projectile.damageAmountBase / 2) * pwr.level;  //projectile = 22
                if (settingsRef.AIHardMode && !pawn.IsColonistPlayerControlled)
                {
                    dmg += 10;
                }
                damageEntities(victim, dmg, TMDamageDefOf.DamageDefOf.Firebolt);
            }
        }

        public void damageEntities(Pawn e, int amt, DamageDef type)
        {
            DamageInfo dinfo = new DamageInfo(type, amt, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
            }
        }
    }    
}


