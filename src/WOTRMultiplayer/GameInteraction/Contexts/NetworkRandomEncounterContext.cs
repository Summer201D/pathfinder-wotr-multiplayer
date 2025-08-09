using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.GameInteraction.Contexts
{
    public class NetworkRandomEncounterContext
    {
        public bool IsRecording { get; set; }

        public NetworkRandomEncounter Encounter { get; set; }
    }
}
