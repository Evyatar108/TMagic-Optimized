﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using AbilityUser;
using Verse.Sound;

namespace TorannMagic.Weapon
{
    class SeerRing_Lightning : Projectile_AbilityLaser
    {


        public override void Impact_Override(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact_Override(hitThing);
            Pawn pawn = this.launcher as Pawn;


            bool flag = hitThing != null;
            if (flag)
            {
                int damageAmountBase = this.def.projectile.damageAmountBase;
                DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, damageAmountBase, this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);
                hitThing.TakeDamage(dinfo);

                bool flag2 = this.canStartFire && Rand.Range(0f, 1f) > this.startFireChance;
                if (flag2)
                {
                    hitThing.TryAttachFire(0.05f);
                }
                Pawn hitTarget;
                bool flag3 = (hitTarget = (hitThing as Pawn)) != null;
                if (flag3)
                {
                    this.PostImpactEffects(this.launcher as Pawn, hitTarget);
                    MoteMaker.ThrowMicroSparks(this.destination, base.Map);
                    MoteMaker.MakeStaticMote(this.destination, base.Map, ThingDefOf.Mote_ShotHit_Dirt, 1f);
                }
            }
            else
            {
                MoteMaker.MakeStaticMote(this.ExactPosition, base.Map, ThingDefOf.Mote_ShotHit_Dirt, 1f);
                MoteMaker.ThrowMicroSparks(this.ExactPosition, base.Map);
            }
            for (int i = 0; i <= 1; i++)
            {
                SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None);
                SoundDefOf.Thunder_OnMap.PlayOneShot(info);
            }
            CellRect cellRect = CellRect.CenteredOn(hitThing.Position, 2);
            cellRect.ClipInsideMap(map);
            for (int i = 0; i < Rand.Range(1, 8); i++)
            {
                IntVec3 randomCell = cellRect.RandomCell;
                this.StaticExplosion(randomCell, map, 0.4f);
            }
        }

        protected void StaticExplosion(IntVec3 pos, Map map, float radius)
        {
            ThingDef def = this.def;
            Explosion(pos, map, radius, TMDamageDefOf.DamageDefOf.TM_Lightning, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_MicroSparks, 0.4f, 1, false, null, 0f, 1);

        }

        public static void Explosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = true, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
        {
            System.Random rnd = new System.Random();
            int modDamAmountRand = GenMath.RoundRandom(Rand.Range(2, projectile.projectile.damageAmountBase / 2));
            if (map == null)
            {
                Log.Warning("Tried to do explosion in a null map.");
                return;
            }
            Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map);
            explosion.dealMoreDamageAtCenter = false;
            explosion.chanceToStartFire = 0.0f;
            explosion.Position = center;
            explosion.radius = radius;
            explosion.damType = damType;
            explosion.instigator = instigator;
            explosion.damAmount = ((projectile == null) ? GenMath.RoundRandom((float)damType.explosionDamage) : modDamAmountRand);
            explosion.weapon = source;
            explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
            explosion.preExplosionSpawnChance = preExplosionSpawnChance;
            explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
            explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
            explosion.postExplosionSpawnChance = postExplosionSpawnChance;
            explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
            explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
            explosion.StartExplosion(explosionSound);
        }
    }
}
