using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace TorannMagic
{
    public static class TM_Data
    {
        public static HashSet<ThingDef> SpellList = (from def in DefDatabase<ThingDef>.AllDefs
                                                         where def.defName.Contains("SpellOf_")
                                                         select def).ToHashSet(); 

        public static HashSet<ThingDef> MasterSpellList = new HashSet<ThingDef>()
            {
                TorannMagicDefOf.SpellOf_Firestorm,
                TorannMagicDefOf.SpellOf_Blizzard,
                TorannMagicDefOf.SpellOf_EyeOfTheStorm,
                TorannMagicDefOf.SpellOf_RegrowLimb,
                TorannMagicDefOf.SpellOf_FoldReality,
                TorannMagicDefOf.SpellOf_Resurrection,
                TorannMagicDefOf.SpellOf_HolyWrath,
                TorannMagicDefOf.SpellOf_LichForm,
                TorannMagicDefOf.SpellOf_SummonPoppi,
                TorannMagicDefOf.SpellOf_BattleHymn,
                TorannMagicDefOf.SpellOf_PsychicShock,
                TorannMagicDefOf.SpellOf_Scorn,
                TorannMagicDefOf.SpellOf_Meteor,
                TorannMagicDefOf.SpellOf_OrbitalStrike,
                TorannMagicDefOf.SpellOf_BloodMoon,
                TorannMagicDefOf.SpellOf_Shapeshift,
                TorannMagicDefOf.SpellOf_Recall,
                TorannMagicDefOf.SpellOf_SpiritOfLight,
                TorannMagicDefOf.SpellOf_GuardianSpirit
            };

        public static HashSet<ThingDef> RestrictedAbilities = new HashSet<ThingDef>
                {
                    TorannMagicDefOf.SpellOf_BattleHymn,
                    TorannMagicDefOf.SpellOf_BlankMind,
                    TorannMagicDefOf.SpellOf_Blizzard,
                    TorannMagicDefOf.SpellOf_BloodMoon,
                    TorannMagicDefOf.SpellOf_BriarPatch,
                    TorannMagicDefOf.SpellOf_CauterizeWound,
                    TorannMagicDefOf.SpellOf_ChargeBattery,
                    TorannMagicDefOf.SpellOf_DryGround,
                    TorannMagicDefOf.SpellOf_EyeOfTheStorm,
                    TorannMagicDefOf.SpellOf_FertileLands,
                    TorannMagicDefOf.SpellOf_Firestorm,
                    TorannMagicDefOf.SpellOf_FoldReality,
                    TorannMagicDefOf.SpellOf_HolyWrath,
                    TorannMagicDefOf.SpellOf_LichForm,
                    TorannMagicDefOf.SpellOf_MechaniteReprogramming,
                    TorannMagicDefOf.SpellOf_Meteor,
                    TorannMagicDefOf.SpellOf_OrbitalStrike,
                    TorannMagicDefOf.SpellOf_Overdrive,
                    TorannMagicDefOf.SpellOf_PsychicShock,
                    TorannMagicDefOf.SpellOf_Recall,
                    TorannMagicDefOf.SpellOf_RegrowLimb,
                    TorannMagicDefOf.SpellOf_Resurrection,
                    TorannMagicDefOf.SpellOf_Sabotage,
                    TorannMagicDefOf.SpellOf_Scorn,
                    TorannMagicDefOf.SpellOf_Shapeshift,
                    TorannMagicDefOf.SpellOf_SummonPoppi,
                    TorannMagicDefOf.SpellOf_TechnoShield,
                    TorannMagicDefOf.SpellOf_WetGround,
                    TorannMagicDefOf.SpellOf_SpiritOfLight,
                    TorannMagicDefOf.SpellOf_GuardianSpirit,
                    TorannMagicDefOf.SpellOf_Discord,
                    TorannMagicDefOf.SpellOf_ShieldOther
                };

        public static HashSet<ThingDef> StandardSpellList = SpellList.Except(MasterSpellList).ToHashSet();

        public static HashSet<ThingDef> StandardSkillList = (from def in DefDatabase<ThingDef>.AllDefs
                                                          where def.defName.Contains("SkillOf_")
                                                          select def).ToHashSet();

        public static HashSet<ThingDef> FighterBookList = new HashSet<ThingDef>
            {
                TorannMagicDefOf.BookOfGladiator,
                TorannMagicDefOf.BookOfBladedancer,
                TorannMagicDefOf.BookOfDeathKnight,
                TorannMagicDefOf.BookOfFaceless,
                TorannMagicDefOf.BookOfPsionic,
                TorannMagicDefOf.BookOfRanger,
                TorannMagicDefOf.BookOfSniper,
                TorannMagicDefOf.BookOfMonk,
                TorannMagicDefOf.BookOfCommander,
                TorannMagicDefOf.BookOfSuperSoldier
            };

        public static HashSet<ThingDef> MageBookList = GetMageBookList();

        private static HashSet<ThingDef> GetMageBookList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where def.defName.Contains("BookOf")
                                               select def;

            enumerable = enumerable.Except(GetMageTornScriptList());
            return enumerable.Except(FighterBookList).ToHashSet();
        }

        public static HashSet<ThingDef> AllBooksList = GetAllBooksList();

        private static HashSet<ThingDef> GetAllBooksList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where def.defName.Contains("BookOf")
                                               select def;

            return enumerable.Except(GetMageTornScriptList()).ToHashSet();
        }

        public static HashSet<ThingDef> MageTornScriptList = GetMageTornScriptList();

        private static HashSet<ThingDef> GetMageTornScriptList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where def.defName.Contains("Torn_BookOf")
                                               select def;

            return enumerable.ToHashSet();
        }

        public static HashSet<TraitDef> MagicTraits = new HashSet<TraitDef>
            {
                TorannMagicDefOf.Arcanist,
                TorannMagicDefOf.InnerFire,
                TorannMagicDefOf.HeartOfFrost,
                TorannMagicDefOf.StormBorn,
                TorannMagicDefOf.Druid,
                TorannMagicDefOf.Priest,
                TorannMagicDefOf.Necromancer,
                TorannMagicDefOf.Technomancer,
                TorannMagicDefOf.Geomancer,
                TorannMagicDefOf.Warlock,
                TorannMagicDefOf.Succubus,
                TorannMagicDefOf.ChaosMage,
                TorannMagicDefOf.Paladin,
                TorannMagicDefOf.Summoner,
                TorannMagicDefOf.Lich,
                TorannMagicDefOf.TM_Bard,
                TorannMagicDefOf.Chronomancer,
                TorannMagicDefOf.Enchanter,
                TorannMagicDefOf.BloodMage,
                TorannMagicDefOf.TM_Wanderer,
                TorannMagicDefOf.TM_Gifted,
                TorannMagicDefOf.TM_Brightmage,
                TorannMagicDefOf.TM_Shaman
            };

        public static HashSet<TraitDef> MightTraits = new HashSet<TraitDef>
                {
                    TorannMagicDefOf.Bladedancer,
                    TorannMagicDefOf.Gladiator,
                    TorannMagicDefOf.Faceless,
                    TorannMagicDefOf.TM_Sniper,
                    TorannMagicDefOf.Ranger,
                    TorannMagicDefOf.TM_Psionic,
                    TorannMagicDefOf.TM_Monk,
                    TorannMagicDefOf.TM_Commander,
                    TorannMagicDefOf.TM_SuperSoldier,
                    TorannMagicDefOf.TM_Wayfarer,
                    TorannMagicDefOf.PhysicalProdigy
                };

        public static HashSet<TraitDef> AllClassTraits = GetAllClassTraits();

        private static HashSet<TraitDef> GetAllClassTraits()
        {
            HashSet<TraitDef> allClassTraits = new HashSet<TraitDef>();
            allClassTraits.AddRange(MightTraits);
            allClassTraits.AddRange(MagicTraits);
            allClassTraits.AddRange(TM_ClassUtility.CustomClassTraitDefs);
            return allClassTraits;
        }

        public static HashSet<ThingDef> MagicFociList = GetMagicFociList();

        private static HashSet<ThingDef> GetMagicFociList()
        {
            HashSet<ThingDef> magicFocis = new HashSet<ThingDef>();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where true
                                               select def;
            List<string> magicFociList = WeaponCategoryList.Named("TM_Category_MagicalFoci").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < magicFociList.Count; i++)
                {
                    if (current.defName == magicFociList[i].ToString() || magicFociList[i].ToString() == "*")
                    {
                        //Log.Message("adding magicFoci def " + current.defName);
                        magicFocis.Add(current);
                    }
                }
            }
            return magicFocis;
        }

        public static HashSet<ThingDef> BowList = GetBowList();

        private static HashSet<ThingDef> GetBowList()
        {
            HashSet<ThingDef> bows = new HashSet<ThingDef>();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where true
                                               select def;
            List<string> bowList = WeaponCategoryList.Named("TM_Category_Bows").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < bowList.Count; i++)
                {
                    if (current.defName == bowList[i].ToString() || bowList[i].ToString() == "*")
                    {
                        //Log.Message("adding bow def " + current.defName);
                        bows.Add(current);
                    }
                }
            }
            return bows;
        }

        public static HashSet<ThingDef> PistolList = GetPistolList();

        private static HashSet<ThingDef> GetPistolList()
        {
            HashSet<ThingDef> pistols = new HashSet<ThingDef>();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where true
                                               select def;
            List<string> pistolList = WeaponCategoryList.Named("TM_Category_Pistols").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < pistolList.Count; i++)
                {
                    if (current.defName == pistolList[i].ToString() || pistolList[i].ToString() == "*")
                    {
                        //Log.Message("adding pistol def " + current.defName);
                        pistols.Add(current);
                    }
                }
            }
            return pistols;
        }

        public static HashSet<ThingDef> RifleList = GetRifleList();

        private static HashSet<ThingDef> GetRifleList()
        {
            HashSet<ThingDef> rifles = new HashSet<ThingDef>();
            rifles.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where true
                                               select def;
            List<string> rifleList = WeaponCategoryList.Named("TM_Category_Rifles").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < rifleList.Count; i++)
                {
                    if (current.defName == rifleList[i].ToString() || rifleList[i].ToString() == "*")
                    {
                        //Log.Message("adding rifle def " + current.defName);
                        rifles.Add(current);
                    }
                }
            }
            return rifles;
        }

        public static HashSet<ThingDef> ShotgunList = GetShotgunList();

        private static HashSet<ThingDef> GetShotgunList()
        {
            HashSet<ThingDef> shotguns = new HashSet<ThingDef>();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where true
                                               select def;
            List<string> shotgunList = WeaponCategoryList.Named("TM_Category_Shotguns").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < shotgunList.Count; i++)
                {
                    if (current.defName == shotgunList[i].ToString() || shotgunList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        shotguns.Add(current);
                    }
                }
            }
            return shotguns;
        }

        public static HashSet<HediffDef> AilmentList = GetAilmentList();

        private static HashSet<HediffDef> GetAilmentList()
        {
            HashSet<HediffDef> ailments = new HashSet<HediffDef>();
            ailments.Clear();
            IEnumerable<HediffDef> enumerable = from def in DefDatabase<HediffDef>.AllDefs
                                                where true
                                                select def;
            List<TMDefs.TM_CategoryHediff> ailmentList = HediffCategoryList.Named("TM_Category_Hediffs").ailments;
            foreach (HediffDef current in enumerable)
            {
                for (int i = 0; i < ailmentList.Count; i++)
                {
                    if (current.defName == ailmentList[i].hediffDefname || (ailmentList[i].containsDefnameString && current.defName.Contains(ailmentList[i].hediffDefname)) || ailmentList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        ailments.Add(current);
                    }
                }
            }
            return ailments;
        }

        public static HashSet<HediffDef> AddictionList = GetAddictionList();

        private static HashSet<HediffDef> GetAddictionList()
        {
            HashSet<HediffDef> addictions = new HashSet<HediffDef>();
            IEnumerable<HediffDef> enumerable = from def in DefDatabase<HediffDef>.AllDefs
                                                where true
                                                select def;
            List<TMDefs.TM_CategoryHediff> addictionList = HediffCategoryList.Named("TM_Category_Hediffs").addictions;
            foreach (HediffDef current in enumerable)
            {
                for (int i = 0; i < addictionList.Count; i++)
                {
                    if (current.defName == addictionList[i].hediffDefname || (addictionList[i].containsDefnameString && current.defName.Contains(addictionList[i].hediffDefname)) || addictionList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        addictions.Add(current);
                    }
                }
            }
            return addictions;
        }

        public static HashSet<HediffDef> MechaniteList = GetMechaniteList();

        private static HashSet<HediffDef> GetMechaniteList()
        {
            HashSet<HediffDef> mechanites = new HashSet<HediffDef>();
            IEnumerable<HediffDef> enumerable = from def in DefDatabase<HediffDef>.AllDefs
                                                where true
                                                select def;
            List<TMDefs.TM_CategoryHediff> mechaniteList = HediffCategoryList.Named("TM_Category_Hediffs").mechanites;
            foreach (HediffDef current in enumerable)
            {
                for (int i = 0; i < mechaniteList.Count; i++)
                {
                    if (current.defName == mechaniteList[i].hediffDefname || (mechaniteList[i].containsDefnameString && current.defName.Contains(mechaniteList[i].hediffDefname)) || mechaniteList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        mechanites.Add(current);
                    }
                }
            }
            return mechanites;
        }

        public static HashSet<HediffDef> DiseaseList = GetDiseaseList();

        private static HashSet<HediffDef> GetDiseaseList()
        {
            HashSet<HediffDef> diseases = new HashSet<HediffDef>();
            IEnumerable<HediffDef> enumerable = from def in DefDatabase<HediffDef>.AllDefs
                                                where true
                                                select def;
            List<TMDefs.TM_CategoryHediff> diseaseList = HediffCategoryList.Named("TM_Category_Hediffs").diseases;
            foreach (HediffDef current in enumerable)
            {
                for (int i = 0; i < diseaseList.Count; i++)
                {
                    if (current.defName == diseaseList[i].hediffDefname || (diseaseList[i].containsDefnameString && current.defName.Contains(diseaseList[i].hediffDefname)) || diseaseList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        diseases.Add(current);
                    }
                }
            }
            return diseases;
        }

        public static HashSet<TM_CustomPowerDef> CustomFighterPowerDefs = GetCustomFighterPowerDefs();

        private static HashSet<TM_CustomPowerDef> GetCustomFighterPowerDefs()
        {
            IEnumerable<TM_CustomPowerDef> enumerable = from def in DefDatabase<TM_CustomPowerDef>.AllDefs
                                                        where def.customPower != null && def.customPower.forFighter
                                                        select def;
            return enumerable.ToHashSet();
        }

        public static HashSet<TM_CustomPowerDef> CustomMagePowerDefs = GetCustomMagePowerDefs();

        private static HashSet<TM_CustomPowerDef> GetCustomMagePowerDefs()
        {
            IEnumerable<TM_CustomPowerDef> enumerable = from def in DefDatabase<TM_CustomPowerDef>.AllDefs
                                                        where def.customPower != null && def.customPower.forMage
                                                        select def;
            return enumerable.ToHashSet();
        }

    }
}
