using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TorannMagic.TMDefs;
using TorannMagic.ModOptions;

namespace TorannMagic
{
    public static class TM_ClassUtility
    {

        public static List<TMDefs.TM_CustomClass> CustomClasses()
        {
            return TM_CustomClassDef.Named("TM_CustomClasses").customClasses;
        }

        public static List<TraitDef> CustomClassTraitDefs = GetCustomClassTraitDefs();

        private static List<TraitDef> GetCustomClassTraitDefs()
        {
            var customClasses = CustomClasses();
            List<TraitDef> customTraits = new List<TraitDef>(customClasses.Count);
            for (int i = 0; i < customClasses.Count; i++)
            {
                customTraits.Add(customClasses[i].classTrait);
            }
            return customTraits;
        }

        public static IEnumerable<TM_CustomClass> CustomMageClasses = GetCustomMageClasses();

        private static IEnumerable<TM_CustomClass> GetCustomMageClasses()
        {
            var customClasses = CustomClasses();
            for (int i = 0; i < customClasses.Count; i++)
            {
                var customClass = customClasses[i];
                bool classEnabled = Settings.Instance.CustomClass[customClass.classTrait.ToString()];
                if (customClass.isMage && classEnabled & ModOptions.Settings.Instance.CustomClass[customClass.classTrait.ToString()])
                {
                    yield return customClass;
                }
            }
        }

        public static List<TM_CustomClass> CustomFighterClasses = GetCustomFighterClasses();

        private static List<TM_CustomClass> GetCustomFighterClasses()
        {
            List<TM_CustomClass> fighterClasses = new List<TM_CustomClass>();
            fighterClasses.Clear();
            var customClasses = CustomClasses();
            for (int i = 0; i < CustomClasses().Count; i++)
            {
                if (CustomClasses()[i].isFighter && ModOptions.Settings.Instance.CustomClass[CustomClasses()[i].classTrait.ToString()])
                {
                    fighterClasses.Add(CustomClasses()[i]);
                }
            }
            return fighterClasses;
        }

        public static int IsCustomClassIndex(List<Trait> allTraits)
        {
                var customClasses = CustomClasses();
            for (int i = 0; i < CustomClasses().Count; i++)
            {
                for (int j = 0; j < allTraits.Count; j++)
                {
                    if (CustomClasses()[i].classTrait == allTraits[j].def)
                    {
                        return i;
                    }
                }
            }
            return -2;
        }

        public static int CustomClassIndexOfTraitDef(TraitDef trait)
        {
                var customClasses = CustomClasses();
            for (int i = 0; i < CustomClasses().Count; i++)
            {
                if (CustomClasses()[i].classTrait.defName == trait.defName)
                {
                    return i;
                }
            }
            return -2;
        }

        public static List<MagicPowerSkill> GetAssociatedMagicPowerSkill(CompAbilityUserMagic comp, MagicPower power)
        {
            string str = power.TMabilityDefs.FirstOrDefault().defName.ToString() + "_";
            List<MagicPowerSkill> skills = new List<MagicPowerSkill>();
            for (int i = 0; i < comp.MagicData.AllMagicPowerSkills.Count; i++)
            {
                MagicPowerSkill mps = comp.MagicData.AllMagicPowerSkills[i];
                if (mps.label.Contains(str))
                {
                    skills.Add(mps);
                }
            }
            return skills;
        }

        public static List<MightPowerSkill> GetAssociatedMightPowerSkill(CompAbilityUserMight comp, TMAbilityDef abilityDef, string var)
        {
            string str = abilityDef.defName.ToString() + "_" + var;
            List<MightPowerSkill> skills = new List<MightPowerSkill>();
            for (int i = 0; i < comp.MightData.AllMightPowerSkills.Count; i++)
            {
                MightPowerSkill mps = comp.MightData.AllMightPowerSkills[i];
                if (mps.label.Contains(str))
                {
                    skills.Add(mps);
                }
            }
            return skills;
        }

        public static MightPowerSkill GetMightPowerSkillFromLabel(CompAbilityUserMight comp, string label)
        {
            if (comp != null && comp.MightData != null && comp.MightData.AllMightPowerSkills.Count > 0)
            {
                for (int i = 0; i < comp.MightData.AllMightPowerSkills.Count; i++)
                {
                    MightPowerSkill mps = comp.MightData.AllMightPowerSkills[i];
                    if (mps.label == label)
                    {
                        return mps;
                    }
                }
            }
            return null;
        }

        public static MagicPowerSkill GetMagicPowerSkillFromLabel(CompAbilityUserMagic comp, string label)
        {
            if (comp != null && comp.MagicData != null && comp.MagicData.AllMagicPowerSkills.Count > 0)
            {
                for (int i = 0; i < comp.MagicData.AllMagicPowerSkills.Count; i++)
                {
                    MagicPowerSkill mps = comp.MagicData.AllMagicPowerSkills[i];
                    if (mps.label == label)
                    {
                        return mps;
                    }
                }
            }
            return null;
        }

        public static TMDefs.TM_CustomClass GetRandomCustomFighter()
        {
            List<TMDefs.TM_CustomClass> customFighters = CustomFighterClasses;
            if (customFighters.Count > 0)
            {
                return customFighters.RandomElement();
            }
            return null;
        }

        public static TMDefs.TM_CustomClass GetRandomCustomMage()
        {
            List<TMDefs.TM_CustomClass> customMages = CustomMageClasses.ToList();
            if (customMages.Count > 0)
            {
                return customMages.RandomElement();
            }
            return null;
        }

        public static bool ClassHasAbility(TMAbilityDef ability, CompAbilityUserMagic compMagic = null, CompAbilityUserMight compMight = null)
        {
            if (compMagic != null && compMagic.customClass != null && compMagic.customClass.HasAbility(ability))
            {
                return true;
            }
            else if (compMight != null && compMight.customClass != null && compMight.customClass.HasAbility(ability))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ClassHasHediff(HediffDef hdDef, CompAbilityUserMagic compMagic = null, CompAbilityUserMight compMight = null)
        {
            if (compMagic != null && compMagic.customClass != null && compMagic.customClass.classHediff == hdDef)
            {
                return true;
            }
            else if (compMight != null && compMight.customClass != null && compMight.customClass.classHediff == hdDef)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
