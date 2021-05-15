using RimWorld;
using System.Collections.Generic;
using Verse;
using UnityEngine;
using System.Linq;

namespace TorannMagic.TMDefs
{
    public class TM_CustomClass
    {

        private HashSet<TMAbilityDef> classMageAbilitiesSet;
        public HashSet<TMAbilityDef> classFighterAbilitiesSet;

        //Class Defining Trait
        public TraitDef classTrait = null;
        public int traitDegree = 4;
        public string classIconPath = "";
        public Color classIconColor = new Color(1f, 1f, 1f);
        public string classTexturePath = "";

        //Class Hediff
        public HediffDef classHediff = null;
        public float hediffSeverity = 1f;

        //Class Abilities
        public List<TMAbilityDef> classMageAbilities = new List<TMAbilityDef>();
        public List<TMAbilityDef> classFighterAbilities = new List<TMAbilityDef>();
        public List<ThingDef> learnableSpells = new List<ThingDef>();
        public List<ThingDef> learnableSkills = new List<ThingDef>();

        //Class Designations
        public bool isMage = false;
        public int maxMageLevel = 200;
        public bool isFighter = false;
        public int maxFighterLevel = 200;
        public bool isNecromancer = false;
        public bool isUndead = false;
        public bool isAndroid = false;
        public bool isAdvancedClass = false;
        public bool shouldShow = true;

        //Class Items
        public ThingDef tornScript = null;
        public ThingDef fullScript = null;

        public bool HasAbility(TMAbilityDef abilityDef)
        {
            if (classMageAbilitiesSet == null)
                classMageAbilitiesSet = classMageAbilities.ToHashSet();
            if (classFighterAbilitiesSet == null)
                classFighterAbilitiesSet = classFighterAbilities.ToHashSet();

            return classFighterAbilities.Contains(abilityDef) || classMageAbilities.Contains(abilityDef);
        }
    }
}