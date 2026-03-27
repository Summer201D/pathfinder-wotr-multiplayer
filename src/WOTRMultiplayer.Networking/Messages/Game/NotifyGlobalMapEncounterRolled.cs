using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapEncounterRolled)]
    public class NotifyGlobalMapEncounterRolled
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapEncounter Encounter { get; set; }
    }
}
