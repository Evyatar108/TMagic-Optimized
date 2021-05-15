using System.Collections.Generic;

namespace TorannMagic.TMDefs
{
    public class TM_CustomPower
    {
        //Abilities
        public List<AbilityUser.AbilityDef> abilityDefs;

        //Skills
        public List<TM_CustomSkill> skills;

        //Autocast features
        public TM_Autocast autocasting;

        //Application
        public bool forMage = false;
        public bool forFighter = false;
        public int maxLevel;
        public int learnCost;
        public bool requiresScroll;
        public bool chaosMageUseable = false;
        public int costToLevel;

    }
}