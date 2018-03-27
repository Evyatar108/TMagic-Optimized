﻿using System;
using Verse;
using RimWorld;
using AbilityUser;
using System.Linq;
using UnityEngine;

namespace TorannMagic.Weapon
{
    public class Projectile_FireWand : Projectile_AbilityBase
    {
        private float arcaneDmg = 1;

        protected override void Impact(Thing hitThing)
        {
            Pawn pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            if(comp.IsMagicUser)
            {
                this.arcaneDmg = comp.arcaneDmg;
            }
            try
            {
                GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, DamageDefOf.Bomb, this.launcher, Mathf.RoundToInt(this.def.projectile.damageAmountBase * this.arcaneDmg), SoundDefOf.Crunch, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1, 0.1f, true);
            }
            catch
            {
                //don't care
            }
        }

        protected void FireExplosion(IntVec3 pos, Map map, float radius)
        {
            ThingDef def = this.def;
            Explosion(pos, map, radius, DamageDefOf.Flame, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.6f, 1, false, null, 0f, 1);            
        }

        public static void Explosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
        {
            System.Random rnd = new System.Random();
            int modDamAmountRand = (int)GenMath.RoundRandom(rnd.Next(3, projectile.projectile.damageAmountBase/2));
            if (map == null)
            {
                Log.Warning("Tried to do explosion in a null map.");
                return;
            }
            Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map);
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
            explosion.dealMoreDamageAtCenter = true;
            explosion.chanceToStartFire = 0.05f;
            explosion.StartExplosion(explosionSound);

        }
    }
}