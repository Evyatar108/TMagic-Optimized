using Verse;

namespace TorannMagic.Enchantment
{
    public class CompEnchantedStuff : ThingComp
    {
        public CompProperties_EnchantedStuff Props
        {
            get
            {
                return (CompProperties_EnchantedStuff)this.props;
            }
        }
    }
}
