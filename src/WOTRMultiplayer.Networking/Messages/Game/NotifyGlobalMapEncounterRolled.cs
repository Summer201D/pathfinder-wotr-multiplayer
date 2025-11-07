using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapEncounterRolled)]
    public class NotifyGlobalMapEncounterRolled
    {
        [ProtoMember(1)]
        public NetworkGlobalMapEncounter Encounter { get; set; }
    }
}
