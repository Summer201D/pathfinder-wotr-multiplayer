using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapEncounterMessageShown)]
    public class NotifyGlobalMapEncounterMessageShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
