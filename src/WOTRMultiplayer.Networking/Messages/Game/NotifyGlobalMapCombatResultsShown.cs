using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCombatResultsShown)]
    public class NotifyGlobalMapCombatResultsShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
