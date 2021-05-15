using RimWorld;
using Verse;

namespace TorannMagic.TMDefs
{
    public sealed class TM_Incident
    {
        //Incident the recipe generates
        public IncidentDef resultIncident = new IncidentDef();
        public int incidentPoints = 0;
        public bool incidentHostile = false;

        //How many times to execute
        public IntRange countRange = new IntRange(1, 1);
    }
}
