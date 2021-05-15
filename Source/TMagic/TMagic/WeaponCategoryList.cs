using System.Collections.Generic;
using Verse;

namespace TorannMagic
{
    public class WeaponCategoryList : Def
    {
        public List<string> weaponDefNames;

        public static WeaponCategoryList Named(string defName)
        {
            return DefDatabase<WeaponCategoryList>.GetNamed(defName);
        }
    }
}
