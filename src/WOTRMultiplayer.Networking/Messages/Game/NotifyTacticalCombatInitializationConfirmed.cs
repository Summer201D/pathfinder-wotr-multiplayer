using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyTacticalCombatInitializationConfirmed)]
    public class NotifyTacticalCombatInitializationConfirmed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
