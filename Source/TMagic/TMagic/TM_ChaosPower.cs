using System.Collections.Generic;

namespace TorannMagic
{
    public class TM_ChaosPowers //: IExposable
    {
        private List<MagicPowerSkill> skills = null;

        public List<MagicPowerSkill> Skills
        {
            get
            {
                if (skills == null)
                {
                    skills = new List<MagicPowerSkill>();
                    skills.Clear();
                }
                return skills;
            }
            set
            {
                if (skills == null)
                {
                    skills = new List<MagicPowerSkill>();
                    skills.Clear();
                }
                skills = value;
            }
        }

        public TMAbilityDef Ability { get; set; } = null;

        public TM_ChaosPowers(TMAbilityDef _ability, List<MagicPowerSkill> _skills)
        {
            Ability = _ability;
            Skills = _skills;
        }

        //public void ExposeData()
        //{
        //    Scribe_Defs.Look<TMAbilityDef>(ref this.ability, "ability");
        //    Scribe_Collections.Look<MagicPowerSkill>(ref this.skills, "skills", LookMode.Deep, new object[0]);
        //}
    }
}
